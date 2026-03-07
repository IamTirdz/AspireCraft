namespace AspireCraft.Runtime;

public sealed class DirectoryBuilder
{
    public void CopyDirectory(string source, string destination)
    {
        foreach (var dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dir.Replace(source, destination));

        foreach (var file in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            File.Copy(file, file.Replace(source, destination), true);
    }
}
