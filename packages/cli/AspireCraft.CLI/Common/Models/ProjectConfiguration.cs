namespace AspireCraft.CLI.Common.Models;

public class ProjectConfiguration
{
    public ProjectConfiguration(string name, string framework, string database, string auth, bool redis)
    {
        Name = name;
        Framework = framework;
        Database = database;
        Auth = auth;
        Redis = redis;
    }

    public string Name { get; set; }
    public string Framework { get; set; }
    public string Database { get; set; }
    public string Auth { get; set; }
    public bool Redis { get; set; }
}