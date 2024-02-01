

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace SunamoGitBashBuilder._sunamo
{
    internal class SH
    {


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string WrapWith(string value, string h)
        {
            return h + value + h;
        }

        public static string WrapWithQm(string value)
        {
            var h = "\"";
            return h + value + h;
        }

        #region FirstCharUpper
        public static string FirstCharUpper(ref string result)
        {
            result = FirstCharUpper(result);
            return result;
        }

        public static string FirstCharUpper(string nazevPP, bool only = false)
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
}
