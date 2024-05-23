namespace SunamoGitBashBuilder;

public class SHTrim
{
    public static string TrimStart(string v, string s)
    {
        while (v.StartsWith(s))
        {
            v = v.Substring(s.Length);
        }

        return v;
    }
}
