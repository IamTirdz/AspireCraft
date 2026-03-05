namespace AspireCraft.Core.Utilities;

public static class TemplateHelper
{
    public static string GetTemplatesPath()
    {
        var devPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "templates"));
        if (Directory.Exists(devPath))
            return devPath;

        var toolPath = Path.Combine(AppContext.BaseDirectory, "templates");
        if (Directory.Exists(toolPath))
            return toolPath;

        throw new DirectoryNotFoundException("Templates folder not found. Checked:\n" +
            $"- {devPath}\n- {toolPath}");
    }
}
