using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Services;
using HR_AUTOMATION_WEB_AGENT;
using Shared.Kernel.IServices;
using Shared.Kernel.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();

builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<IScraperService, ScraperService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();