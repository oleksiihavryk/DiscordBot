namespace Oleksii_Havryk.DiscordBot.Core.Extensions;
/// <summary>
///     Some generic extensions in system.
/// </summary>
public static class GenericExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> enumerable)
    {
        var safeEnumerable = enumerable as T[] ?? enumerable.ToArray();
        var ran = new Random();
        var pos = ran.Next(0, safeEnumerable.Count());

        return safeEnumerable.Skip(pos).First();
    }
}