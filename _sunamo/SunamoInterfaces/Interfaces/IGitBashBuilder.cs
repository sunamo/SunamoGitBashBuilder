namespace SunamoGitBashBuilder;


internal interface IGitBashBuilder
{
    List<string> Commands { get; }
    void Add(string v);
    void AddNewRemote(string s);
    void Append(string text);
    void AppendLine();
    void AppendLine(string text);
    void Cd(string key);
    void Checkout(string arg);
    void Clean(string v);
    void Clear();
    void Clone(string repoUri, string args);
    void Commit(bool addAllUntrackedFiles, string commitMessage);
    void Config(string v);
    void Fetch(string s = "");
    void Init();
    void Merge(string v);
    void Pull();
    void Push(bool force);
    void Push(string arg);
    void Remote(string arg);
    void Status();
    string ToString();
}