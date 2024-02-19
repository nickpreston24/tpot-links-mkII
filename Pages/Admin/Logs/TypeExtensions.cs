namespace TPOT_Links.Pages.Logs;

public static class TypeExtensions
{
    public static uint ToUInt(this string text, uint fallback = 0)
    {
        return uint.TryParse(text, out var result) ? fallback : result;
    }
}