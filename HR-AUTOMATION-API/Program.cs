using Asp.Versioning;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Services;
using HR_AUTOMATION.Infrastructure.Authentication;
using HR_AUTOMATION.Infrastructure.Constants;
using HR_AUTOMATION.Infrastructure.Hubs;
using HR_AUTOMATION.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Serilog;
using Serilog.Debugging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Repositories;
using Shared.Kernel.Responses;
using Shared.Kernel.Services;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;
using StackExchange.Redis;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

SelfLog.Enable(Console.Error);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    string connectionString = builder.Configuration.GetValue<string>(AppConstants.RedisConnectionStringKey)!;

    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

builder.Services.AddScoped<ICacheService, RedisService>();
builder.Services.AddScoped<ISharedRepository, SqlServerRepository>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IHttpContextService, HttpContextService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ISkillCategoryService, SkillCategoryService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISkillLevelService, SkillLevelService>();
builder.Services.AddScoped<IAreaLevelService, AreaLevelService>();
builder.Services.AddScoped<ICriticalityLevelService, CriticalityLevelService>();
builder.Services.AddScoped<IRejectionReasonService, RejectionReasonService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IPaymentPeriodService, PaymentPeriodService>();
builder.Services.AddScoped<IEmploymentTypeService, EmploymentTypeService>();
builder.Services.AddScoped<IWorkModalityService, WorkModalityService>();
builder.Services.AddScoped<ISeniorityLevelService, SeniorityLevelService>();
builder.Services.AddScoped<IQuestionCategoryService, QuestionCategoryService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IVacancyService, VacancyService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IScraperService, ScraperService>();
builder.Services.AddScoped<IScolarityLevelService, ScolarityLevelService>();
builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ISearchRequestService, SearchRequestService>();

builder.Services.AddScoped<IAuthService, AuthService>();

// resultados (candidatos)
builder.Services.AddScoped<ISearchRequestService, SearchRequestService>();
builder.Services.AddScoped<ISearchResultsService, SearchResultsService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string key = builder.Configuration.GetValue<string>(AppConstants.JwtSecretKey)!;
        string issuer = builder.Configuration.GetValue<string>(AppConstants.JwtIssuerKey)!;
        string audience = builder.Configuration.GetValue<string>(AppConstants.JwtDefaultAudienceKey)!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            // JwtTokenService issues the role claim under the literal type "role" rather than the
            // long ClaimTypes.Role URI; without this, [Authorize(Roles = "...")] would never match.
            RoleClaimType = "role"
        };

        // WebSocket handshakes cannot carry the Authorization header, so the SignalR client sends
        // the token as the "access_token" query string. Pull it in for the notification hub path so
        // Context.User is populated inside the hub.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments(HubConstants.NotificationEndpoint))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = VersioningConstants.AssumeDefaultVersionWhenUnspecified;
    options.DefaultApiVersion = new ApiVersion(VersioningConstants.DefaultApiVersion);
    options.ReportApiVersions = VersioningConstants.ReportApiVersions;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = VersioningConstants.GroupNameFormat;
    options.SubstituteApiVersionInUrl = VersioningConstants.SubstituteApiVersionInUrl;
});
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        Response response = new()
        {
            Code = Exceptions.TooManyRequests.GetValue(),
            ResponseMessage = Exceptions.TooManyRequests.GetDescription()
        };

        string json = JsonConvert.SerializeObject(response);

        context.HttpContext.Response.StatusCode = Exceptions.TooManyRequests.GetValue();
        context.HttpContext.Response.ContentType = MediaTypes.Json;

        await context.HttpContext.Response.WriteAsync(json, token);
    };

    options.AddPolicy(RateLimitConstants.DefaultPolicy, httpContext =>
    {
        string ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? RateLimitConstants.Unknown;

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = RateLimitConstants.DefaultPermitLimit,
            Window = TimeSpan.FromMilliseconds(RateLimitConstants.DefaultWindowMilliseconds),
            QueueLimit = RateLimitConstants.DefaultQueueLimit
        });
    });
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AppConstants.Bearer.ToLower(), new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = AppConstants.Bearer.ToLower(),
        BearerFormat = AppConstants.BearerFormat,
        Description = AppConstants.BearerFormatDescription
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(AppConstants.Bearer.ToLower(), document)] = []
    });
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseCors(options =>
{
    string[] allowedOrigins = builder.Configuration.GetSection(CorsConstants.AllowedOriginsKey).Get<string[]>() ?? [];
    string[] allowedHeader = builder.Configuration.GetSection(CorsConstants.AllowedHeadersKey).Get<string[]>() ?? [];
    string[] allowedMethods = builder.Configuration.GetSection(CorsConstants.AllowedMethodsKey).Get<string[]>() ?? [];
    string[] exposedHeaders = builder.Configuration.GetSection(CorsConstants.ExposedHeadersKey).Get<string[]>() ?? [];

    options
        .WithOrigins(allowedOrigins)
        .WithHeaders(allowedHeader)
        .WithMethods(allowedMethods)
        .WithExposedHeaders(exposedHeaders)
        .AllowCredentials();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>(HubConstants.NotificationEndpoint);

app.Run();