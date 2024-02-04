

using SunamoCollectionsChangeContent.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunamoGitBashBuilder._sunamo;

internal class CAChangeContent
{


    internal static List<string> ChangeContentSwitch12<Arg1>(List<string> files_in, Func<Arg1, string, string> func, Arg1 arg)
    {
        for (int i = 0; i < files_in.Count; i++)
        {
            files_in[i] = func.Invoke(arg, files_in[i]);
        }
        return files_in;
    }

    internal static List<string> ChangeContent<Arg1>(ChangeContentArgs a, List<string> files_in, Func<string, Arg1, string> func, Arg1 arg, Func<Arg1, string, string> funcSwitch12 = null)
    {
        if (a == null)
        {
            a = new ChangeContentArgs();
        }

        if (a.switchFirstAndSecondArg)
        {
            files_in = ChangeContentSwitch12<Arg1>(files_in, funcSwitch12, arg);
        }
        else
        {
            for (int i = 0; i < files_in.Count; i++)
            {
                files_in[i] = func.Invoke(files_in[i], arg);
            }
        }

        files_in = files_in.Where(d => !string.IsNullOrWhiteSpace(d)).ToList();

        return files_in;
    }

    internal static List<string> ChangeContent1(ChangeContentArgs a, List<string> files_in, Func<string, string, string> func, string a1)
    {
        var result = ChangeContent<string>(a, files_in, func, a1);
        return result;
    }


}
