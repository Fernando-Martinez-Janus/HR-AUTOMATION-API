
using Asp.Versioning;
using HR_AUTOMATION.Application;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Services;
using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Infrastructure;
using HR_AUTOMATION.Infrastructure.Hubs;
using HR_AUTOMATION.Infrastructure.Middlewares;
using HR_AUTOMATION.Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Shared.Kernel.Appsettings;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Repositories;
using Shared.Kernel.Responses;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        Response response = new()
        {
            Code = StatusCodes.Status429TooManyRequests,
            ResponseMessage = "Too many requests. Please try again later."
        };

        string json = JsonConvert.SerializeObject(response);

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsync(json, token);
    };

    options.AddPolicy("general", httpContext =>
    {
        string ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 50,
            Window = TimeSpan.FromSeconds(60),
            QueueLimit = 0
        });
    });
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ISharedRepository, SharedRepository>();
builder.Services.AddScoped<ISeniorityLevelRepository, SeniorityLevelRepository>();
builder.Services.AddScoped<ISeniorityLevelService, SeniorityLevelService>();
builder.Services.AddScoped<IAreaLevelRepository, AreaLevelRepository>();
builder.Services.AddScoped<IAreaLevelService, AreaLevelService>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISkillCategoryRepository, SkillCategoryRepository>();
builder.Services.AddScoped<ISkillCategoryService, SkillCategoryService>();
builder.Services.AddScoped<IProfileSkillRepository, ProfileSkillRepository>();
builder.Services.AddScoped<IProfileSkillService, ProfileSkillService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

builder.Services.AddScoped<IVacancyStatusRepository, VacancyStatusRepository>();
builder.Services.AddScoped<IVacancyStatusService, VacancyStatusService>();

builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
builder.Services.AddScoped<IVacancyService, VacancyService>();

builder.Services.AddScoped<ICriticalityLevelRepository, CriticalityLevelRepository>();
builder.Services.AddScoped<ICriticalityLevelService, CriticalityLevelService>();
builder.Services.AddScoped<ISkillLevelRepository, SkillLevelRepository>();
builder.Services.AddScoped<ISkillLevelService, SkillLevelService>();

builder.Services.AddScoped<Shared.Kernel.IServices.IHttpService, Shared.Kernel.Services.HttpService>();
builder.Services.AddScoped<IOllamaRequestService, OllamaRequestService>();

builder.Services.Configure<OllamaConfigurations>(builder.Configuration.GetSection("OllamaConfigurations"));

builder.Services.AddScoped<Shared.Kernel.JWT.JWTService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();


string cns = builder.Configuration.GetValue<string>("ConnectionString")!;
builder.Services.AddDbContext<Context>((options) => options.UseNpgsql(cns));
builder.Services.Configure<ConnectionConfigurations>(builder.Configuration.GetSection("ConnectionConfigurations"));

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.UseRouting();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseCors(options =>
{
    options.WithOrigins(builder.Configuration.GetSection("Cors").GetSection("AllowedOrigins").GetChildren().Select(child => child.Value).ToArray()!)
        .WithHeaders(builder.Configuration.GetSection("Cors").GetSection("AllowedHeaders").GetChildren().Select(child => child.Value).ToArray()!)
        .WithMethods(builder.Configuration.GetSection("Cors").GetSection("AllowedMethods").GetChildren().Select(child => child.Value).ToArray()!)
        .WithExposedHeaders(builder.Configuration.GetSection("Cors").GetSection("ExposedHeaders").GetChildren().Select(child => child.Value).ToArray()!)
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/alerts");

app.Run();
