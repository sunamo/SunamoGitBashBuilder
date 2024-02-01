

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunamoGitBashBuilder._sunamo
{
    internal class CA
    {
        internal static List<string> Trim(List<string> l)
        {
            for (var i = 0; i < l.Count; i++) l[i] = l[i].Trim();

            return l;
        }
        static string Replace(string s, string from, string to)
        {
            return s.Replace(from, to);
        }

        /// <summary>
        /// Direct edit
        /// </summary>
        /// <param name="files_in"></param>
        /// <param name="what"></param>
        /// <param name="forWhat"></param>
        internal static void Replace(List<string> files_in, string what, string forWhat)
        {
            for (int i = 0; i < files_in.Count; i++)
            {
                files_in[i] = Replace(files_in[i], what, forWhat);
            }
            //CAChangeContent.ChangeContent2(null, files_in, Replace, what, forWhat);
        }
    }
}
