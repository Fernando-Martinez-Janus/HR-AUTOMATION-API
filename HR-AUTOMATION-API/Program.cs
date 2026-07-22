using Asp.Versioning;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Services;
using HR_AUTOMATION.Infrastructure.Constants;
using HR_AUTOMATION.Infrastructure.Hubs;
using HR_AUTOMATION.Infrastructure.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Serilog;
using Shared.Kernel.IRepositories;
using Shared.Kernel.IServices;
using Shared.Kernel.Repositories;
using Shared.Kernel.Responses;
using Shared.Kernel.Services;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Enums;
using StackExchange.Redis;
using System.Reflection;
using System.Threading.RateLimiting;

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
builder.Services.AddControllers();

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