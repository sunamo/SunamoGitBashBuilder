namespace SunamoGitBashBuilder;

internal class SHTrim
{
    internal static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s))
        {
            v = v.Substring(s.Length);
        }

        return v;
    }
}
