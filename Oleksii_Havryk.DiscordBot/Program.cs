using Oleksii_Havryk.DiscordBot.Extensions;
using Oleksii_Havryk.DiscordBot.Core.Extensions;
using Oleksii_Havryk.DiscordBot.Dto;
using LoggerMessage = Oleksii_Havryk.DiscordBot.Domain.LoggerMessage;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

services.AddControllers();

services.AddHttpClient();

services.AddSwaggerGen();
services.AddEndpointsApiExplorer();

services.AddAutoMapper(opt =>
{
    opt.CreateMap<LoggerMessage, LoggerMessageDto>();
});

services.AddCors(opt =>
{
    const string defaultPolicy = "Default";

    opt.DefaultPolicyName = defaultPolicy;
    opt.AddPolicy(name: defaultPolicy, opt =>
    {
        opt.AllowAnyHeader();
        opt.WithOrigins("https://localhost:8000");
        opt.AllowAnyMethod();
    });
});

services.AddDiscordBot(configuration: config);

var app = builder.Build();

app.UseRouting();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();

    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.RoutePrefix = string.Empty;
        opt.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
    });
}

app.UseStaticFiles();

//endpoints
/* 1. Run
 * 2. Stop
 * 3. Get logger messages */
app.UseConfiguredEndpoints();

app.Run();
