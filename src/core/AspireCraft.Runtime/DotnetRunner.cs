using System.Diagnostics;

namespace AspireCraft.Runtime;

public static class DotnetRunner
{
    public static void Run(string args, string? workingDirectory = null)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory()
            }
        };

        ///process.StartInfo.EnvironmentVariables["DOTNET_INTERACTIVE"] = "false";

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (!process.WaitForExit(60000))
        {
            process.Kill();
            throw new Exception($"Command 'dotnet {args}' timed out. Output: {output}")!;
        }

        if (process.ExitCode != 0)
        {
            throw new Exception($"Dotnet CLI Error: {error} \nOutput: {output}")!;
        }
    }

    public static string GetLatestSdkVersion(string targetVersion)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "--list-sdks",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        var output = process?.StandardOutput.ReadToEnd() ?? "";
        process?.WaitForExit();

        return output.Split('\n')
            .Select(line => line.Split('[')[0].Trim())
            .Where(v => v.StartsWith(targetVersion))
            .OrderByDescending(v => v)
            .FirstOrDefault() ?? "";
    }
}
