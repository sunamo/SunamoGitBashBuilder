
namespace SunamoGitBashBuilder;
/// <summary>
/// GitBashBuilder
/// </summary>
public partial class GitBashBuilder 
{
    private static Type type = typeof(GitBashBuilder);
    public ITextBuilder sb = null;
    //public GitBashBuilder()
    //{
    //    sb = new TextBuilder();
    //    sb.prependEveryNoWhite = AllStrings.space;
    //}
    public GitBashBuilder(ITextBuilder sb)
    {
        this.sb = sb;
        //this.sb.sb = sb.sb;
    }
    public bool GitForDebug = false;
    public List<string> Commands { get => SHGetLines.GetLines(ToString()); }
    /// <summary>
    /// A2 must be files prepared to cmd
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="linesFiles"></param>
    public static string CreateGitAddForFiles(StringBuilder sb, List<string> linesFiles)
    {
        return CreateGitCommandForFiles("add", sb, linesFiles);
    }
    /// <summary>
    /// Support:
    /// {dir}/* for add all files
    /// */{filename} - add files from all dirs
    /// automatically add .cs extension where is not
    ///
    ///
    /// A2 - full path or name in Projects folder
    /// A3 - with or without full path, without extension, can be slash and backslash
    /// A5 - must be filled, because is stripped all extension then passed will be suffixed
    /// </summary>
    /// <param name="tlb"></param>
    /// <param name="solution"></param>
    /// <param name="linesFiles"></param>
    /// <param name="searchOnlyWithExtension"></param>
    public static string GenerateCommandForGit(/*TypedLoggerBaseGitBashBuilder*/ object tlb, string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string command, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat(tlb, solution, linesFiles, out anyError, searchOnlyWithExtension, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null || filesToCommit.Count == 0)
        {
            return "";
        }
        string result = GitBashBuilder.CreateGitCommandForFiles(command, new StringBuilder(), filesToCommit);
        //ClipboardHelper.SetText(result);
        return result;
    }
    /// <summary>
    /// A2 - must be filled, because is stripped all extension then passed will be suffixed
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="typedExt"></param>
    /// <param name="files"></param>
    public static string CheckoutWithExtension(string folder, string typedExt, List<string> files, string basePathIfA2SolutionsWontExistsOnFilesystem, ITextBuilder ci, ITypedLoggerBase typedLoggerBase)
    {
        ThrowEx.IsNull("typedExt", typedExt);
        GitBashBuilder bashBuilder = new GitBashBuilder(ci);
        bool anyError = false;
        var filesToCommit = GitBashBuilder.PrepareFilesToSimpleGitFormat(typedLoggerBase, folder, files, out anyError, typedExt, basePathIfA2SolutionsWontExistsOnFilesystem);
        if (filesToCommit == null)
        {
            //SunamoTemplateLogger.Instance.SomeErrorsOccuredSeeLog();
        }
        //string result = GitBashBuilder.CreateGitCommandForFiles("checkout", new StringBuilder(), filesToCommit);
        string result = GitBashBuilder.GenerateCommandForGit(typedLoggerBase, folder, files, out anyError, typedExt, "checkout", basePathIfA2SolutionsWontExistsOnFilesystem);
        return result;
    }
    /// <summary>
    /// A2 - path in which search for files by extension
    /// A5 - must be filled, because is stripped all extension then passed will be suffixed
    /// 
    /// basePathIfA2SolutionsWontExistsOnFilesystem - pass SourceCodePaths.CsProjects or null if A2 exists
    /// </summary>
    /// <param name="tlb"></param>
    /// <param name="solution"></param>
    /// <param name="linesFiles"></param>
    /// <param name="searchOnlyWithExtension"></param>
    /// <param name="command"></param>
    public static List<string> PrepareFilesToSimpleGitFormat(/*TypedLoggerBaseGitBashBuilder*/ object tlb, string solution, List<string> linesFiles, out bool anyError, string searchOnlyWithExtension, string basePathIfA2SolutionsWontExistsOnFilesystem)
    {
        searchOnlyWithExtension = searchOnlyWithExtension.TrimStart(AllChars.asterisk);
        anyError = false;
        // removing notes and description
        //TypedLoggerBase tlb = TypedConsoleLogger.Instance;
        string pathSearchForFiles = null;
        if (Directory.Exists(solution))
        {
            pathSearchForFiles = solution;
        }
        else
        {
            pathSearchForFiles = Path.Combine(basePathIfA2SolutionsWontExistsOnFilesystem, solution);
        }
        string pathRepository = pathSearchForFiles;
        if (solution == Consts.Cz)
        {
            //tlb.Information("Is sunamo.cz");
            pathSearchForFiles += AllStrings.bs + solution;
        }
        //tlb.Information("Path" + ": " + pathSearchForFiles);
        FS.WithEndSlash(ref pathRepository);
        var files = Directory.GetFiles(pathSearchForFiles, "*.*", System.IO.SearchOption.AllDirectories/*, new GetFilesArgs { excludeFromLocationsCOntains = SunamoCollections.new List<string>(@"\.git\") }*/).ToList();
        files = files.Where(d => !d.Contains(@"\.git\")).ToList();
        CA.Replace(linesFiles, solution, string.Empty);
        CAChangeContent.ChangeContent1(null, linesFiles, SHParts.RemoveAfterFirst, AllStrings.swd);
        CA.Trim(linesFiles);
        CAChangeContent.ChangeContent1(null, linesFiles, FS.AddExtensionIfDontHave, searchOnlyWithExtension);
        CAChangeContent.ChangeContent<bool>(null, linesFiles, FS.Slash, true);
        CAChangeContent.ChangeContent1(null, linesFiles, SHTrim.TrimStart, AllStrings.slash);
        var linesFilesOnlyFilename = FS.OnlyNamesNoDirectEdit(linesFiles);
        anyError = false;
        List<string> filesToCommit = new List<string>();
        // In key are filenames, in value full paths to files backslashed
        Dictionary<string, List<string>> dictPsychicallyExistsFiles = FS.GetDictionaryByFileNameWithExtension(files);
        CA.Replace(files, AllStrings.bs, AllStrings.slash);
        pathRepository = FS.Slash(pathRepository, false);
        // process full path files
        for (int i = 0; i < linesFiles.Count; i++)
        {
            var item = linesFilesOnlyFilename[i];
            // full path with backslash on end
            var itemWithoutTrim = linesFiles[i];
            #region Directory\*
            if (item[item.Length - 1] == AllChars.asterisk)
            {
                item = itemWithoutTrim.TrimEnd(AllChars.asterisk);
                string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(item, false));
                if (Directory.Exists(itemWithoutTrimBackslashed))
                {
                    filesToCommit.Add(item + AllStrings.asterisk);
                }
                else
                {
                    anyError = true;
                    //tlb.Error(Exceptions.DirectoryWasntFound(null, itemWithoutTrimBackslashed));
                }
            }
            #endregion
            #region *File - add all files without specify root directory
            else if (item[0] == AllChars.asterisk)
            {
                string file = item.Substring(1);
                if (dictPsychicallyExistsFiles.ContainsKey(file))
                {
                    foreach (var item2 in dictPsychicallyExistsFiles[file])
                    {
                        filesToCommit.Add(FS.Slash(item2.Replace(pathRepository, string.Empty), true));
                    }
                }
            }
            #endregion
            #region Exactly defined file
            else
            {
                var fullPath = item;
                item = Path.GetFileName(item);
                #region File isnt in dict => Dont exists
                if (!dictPsychicallyExistsFiles.ContainsKey(item))
                {
                    anyError = true;
                    //tlb.Error(Exceptions.FileWasntFoundInDirectory(null, fullPath));
                }
                #endregion
                else
                {
                    string itemWithoutTrimBackslashed = Path.Combine(pathRepository, FS.Slash(itemWithoutTrim, false));
                    #region Add as relative file
                    if (itemWithoutTrim.Contains(AllStrings.slash))
                    {
                        if (File.Exists(itemWithoutTrimBackslashed))
                        {
                            filesToCommit.Add(itemWithoutTrim.Replace(pathRepository, string.Empty));
                        }
                        else
                        {
                            anyError = true;
                            //tlb.Error(Exceptions.FileWasntFoundInDirectory(null, itemWithoutTrimBackslashed));
                        }
                    }
                    #endregion
                    #region Add file in root of repository
                    else
                    {
                        if (dictPsychicallyExistsFiles[item].Count == 1)
                        {
                            filesToCommit.Add(FS.Slash(dictPsychicallyExistsFiles[item][0].Replace(pathRepository, string.Empty), true));
                        }
                        else
                        {
                            anyError = true;
                            //tlb.Error(Exceptions.MoreCandidates(null, dictPsychicallyExistsFiles[item], item));
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }
        if (anyError)
        {
            //tlb.Error(xSomeErrorsOccured);
            return null;
        }
        return filesToCommit;
    }
    public static string xSomeErrorsOccured = "SomeErrorsOccured";
    public static string CreateGitCommandForFiles(string command, StringBuilder sb, List<string> linesFiles)
    {
        return null;
        //return GitStatic(sb, command + AllStrings.space + string.Join(AllChars.space, linesFiles));
    }
    public void Cd(string key)
    {
        sb.AppendLine("cd " + SH.WrapWith(key, AllStrings.qm));
    }
    public void Clear()
    {
        sb.Clear();
    }
    public void Append(string text)
    {
        sb.Append(text);
    }
    public void AppendLine(string text)
    {
        sb.AppendLine(text);
    }
    public void AppendLine()
    {
        sb.AppendLine();
    }
    public override string ToString()
    {
        return sb.ToString();
    }
   
}
/// <summary>
/// To easy create interface
/// </summary>
public partial class GitBashBuilder : IGitBashBuilder
{
    public void Pull()
    {
        Git("pull");
        AppendLine();
    }
    #region Git commands
    public void Clone(string repoUri, string args)
    {
        Git("clone " + repoUri + " " + args);
        AppendLine();
    }
    public void Commit(bool addAllUntrackedFiles, string commitMessage)
    {
        ThrowEx.IsNullOrWhitespace("commitMessage", commitMessage);
        Git("commit ");
        if (addAllUntrackedFiles)
        {
            Append("-a");
        }
        if (!string.IsNullOrWhiteSpace(commitMessage))
        {
            Append("-m " + SH.WrapWithQm(commitMessage));
        }
        AppendLine();
    }
    public void Push(bool force)
    {
        Git("push");
        if (force)
        {
            Append("-f");
        }
        AppendLine();
    }
    public void Push(string arg)
    {
        Git("push");
        AppendLine(arg);
        AppendLine();
    }
    /// <summary>
    /// myslim si ze chyba spise ne z v initu byla v clone, init se musi udelat i kdyz chci udelat git remote
    /// nikdy nepoustet na adresar ktery ma jiz adresar .git!! jinak se mi zapise s prazdnym obsahem a pri pristim pushi mam po vsem!!! soubory mi odstrani z disku a ne do zadneho kose!!!
    /// </summary>
    public void Init()
    {
        Git("init");
        AppendLine();
    }
    public void Add(string v)
    {
        Git("add");
        Append(v);
        AppendLine();
    }
    public void Config(string v)
    {
        Git("config");
        Append(v);
        AppendLine();
    }
    /// <summary>
    /// never use, special with dfx argument
    /// d - Remove untracked directories in addition to untracked files.
    /// f - delete all files although conf variable clean.requireForce
    /// x - ignore rules from all .gitignore
    ///
    /// A1 - arguments without dash
    /// </summary>
    /// <param name="v"></param>
    public void Clean(string v)
    {
        Git("clean");
        Arg(v);
        AppendLine();
    }
    public static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }
    /// <summary>
    /// Not automatically append new line - due to conditionals adding arguments
    /// 
    /// Mus� b�t tato metoda statick�? dal�� takov� tu nen�
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="remainCommand"></param>
    private void Git(string remainCommand)
    {
        sb.Append((GitForDebug ? "GitForDebug " : "git ") + remainCommand);
    }
    #endregion
    private void Arg(string v)
    {
        Append(AllStrings.dash + v);
    }
    public void Remote(string arg)
    {
        Git("remote");
        Append(arg);
        AppendLine();
    }
    public void Status()
    {
        Git("status");
        AppendLine();
    }
    public void Fetch(string s = Consts.se)
    {
        Git("fetch " + s);
        AppendLine();
    }
    public void Merge(string v)
    {
        Git("merge " + v);
        AppendLine();
    }
    public void AddNewRemote(string s)
    {
        Remote("add origin " + s);
        Fetch("origin");
        Checkout("-b master --track origin/master");
        AppendLine("vsGitIgnoreGitHub");
        AppendLine("gaacipuu");
    }
    public void Checkout(string arg)
    {
        Git("checkout");
        AppendLine(arg);
    }
}
