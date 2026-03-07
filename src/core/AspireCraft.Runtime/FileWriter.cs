namespace AspireCraft.Runtime;

public sealed class FileWriter
{
    public void Write(string path, string content)
    {
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, content);
    }
}
