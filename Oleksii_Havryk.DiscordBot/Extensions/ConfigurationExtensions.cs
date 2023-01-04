namespace Oleksii_Havryk.DiscordBot.Extensions;
/// <summary>
///     Application configurations.
/// </summary>
internal static class ConfigurationExtensions
{
    /// <summary>
    ///     Use pre-configured endpoints in application chain of request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <returns>
    ///     IApplicationBuilder instance after completing the operation.
    /// </returns>
    internal static IApplicationBuilder UseConfiguredEndpoints(this IApplicationBuilder app)
        => app.UseEndpoints(cfg => cfg.MapControllers());
}