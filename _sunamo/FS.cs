


using SunamoValues;
namespace SunamoGitBashBuilder._sunamo;

internal class FS
{
    #region WithEndSlash
    internal static string WithEndSlash(ref string v)
    {
        if (v != string.Empty)
        {
            v = v.TrimEnd(AllCharsSE.bs) + AllCharsSE.bs;
        }

        SH.FirstCharUpper(ref v);
        return v;
    }

    internal static string WithEndSlash(string v)
    {
        return WithEndSlash(ref v);
    }
    #endregion
    internal static string AddExtensionIfDontHave(string file, string ext)
    {
        // For *.* and git paths {dir}/*
        if (file[file.Length - 1] == AllChars.asterisk)
        {
            return file;
        }
        if (Path.GetExtension(file) == string.Empty)
        {
            return file + ext;
        }

        return file;
    }
    internal static List<string> OnlyNamesNoDirectEdit(List<string> files2)
    {
        List<string> files = new List<string>(files2.Count);
        for (int i = 0; i < files2.Count; i++)
        {
            files.Add(Path.GetFileName(files2[i]));
        }
        return files;
    }
    internal static Dictionary<string, List<string>> GetDictionaryByFileNameWithExtension(List<string> files)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var item in files)
        {
            string filename = Path.GetFileName(item);
            DictionaryHelper.AddOrCreateIfDontExists<string, string>(result, filename, item);
        }

        return result;
    }

    internal static string Slash(string path, bool slash)
    {
        string result = null;
        if (slash)
        {
            result = path.Replace(AllStrings.bs, AllStrings.slash); //SHReplace.ReplaceAll2(path, AllStrings.slash, AllStrings.bs);
        }
        else
        {
            result = path.Replace(AllStrings.slash, AllStrings.bs); //SHReplace.ReplaceAll2(path, AllStrings.bs, AllStrings.slash);
        }

        SH.FirstCharUpper(ref result);
        return result;
    }

}
