

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoGitBashBuilder._sunamo
{
    internal class SHTrim
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
}
