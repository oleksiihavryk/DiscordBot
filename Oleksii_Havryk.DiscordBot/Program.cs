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

const string defaultPolicy = "Default";
services.AddCors(opt =>
{
    opt.DefaultPolicyName = defaultPolicy;
    opt.AddPolicy(name: defaultPolicy, opt =>
    {
        opt.AllowAnyHeader();
        opt.WithOrigins("http://localhost:8000", "https://localhost:8001");
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

app.UseCors(defaultPolicy);

app.UseStaticFiles();

//endpoints
/* 1. Run
 * 2. Stop
 * 3. Get logger messages */
app.UseConfiguredEndpoints();

app.Run();
