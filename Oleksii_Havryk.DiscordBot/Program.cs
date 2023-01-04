using Oleksii_Havryk.DiscordBot.Extensions;
using Oleksii_Havryk.DiscordBot.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

services.AddControllersWithViews();
services.AddHttpClient();
services.AddDiscordBot(configuration: config);

var app = builder.Build();

app.UseRouting();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}

app.UseStaticFiles();

//endpoints
/* 1. Run
 * 2. Stop
 * 3. Get new logger messages
 * 4. Get last logger messages */
app.UseConfiguredEndpoints();

app.Run();
