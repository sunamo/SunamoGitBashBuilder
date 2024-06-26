using System.Runtime.CompilerServices;
namespace SunamoGitBashBuilder;

internal class SH
{


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string WrapWith(string value, string h)
    {
        return h + value + h;
    }

    internal static string WrapWithQm(string value)
    {
        var h = "\"";
        return h + value + h;
    }

    #region FirstCharUpper
    internal static string FirstCharUpper(ref string result)
    {
        result = FirstCharUpper(result);
        return result;
    }

    internal static string FirstCharUpper(string nazevPP, bool only = false)
    {
        if (nazevPP != null)
        {
            string sb = nazevPP.Substring(1);
            if (only)
            {
                sb = sb.ToLower();
            }

            return nazevPP[0].ToString().ToUpper() + sb;
        }

        return null;
    }
    #endregion
}
