namespace SunamoGitBashBuilder;



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

    private static string GitStatic(StringBuilder sb, string remainCommand)
    {
        sb.Append("git " + remainCommand);
        return sb.ToString();
    }

    /// <summary>
    /// Not automatically append new line - due to conditionals adding arguments
    /// 
    /// Musí být tato metoda statická? další taková tu není
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
