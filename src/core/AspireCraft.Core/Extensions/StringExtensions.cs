namespace AspireCraft.Core.Extensions;

public static class StringExtensions
{
    public static string TrimAll(this string str)
    {
        return str?.Replace(" ", "") ?? string.Empty;
    }
}
