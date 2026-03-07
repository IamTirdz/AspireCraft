using AspireCraft.Runtime;

namespace AspireCraft.Builder.Generators;

public sealed class SolutionGenerator
{
    public string Create(string name, string output)
    {
        var solutionRoot = Path.Combine(output, name);

        Directory.CreateDirectory(solutionRoot);
        DotnetRunner.Run($"new sln -n {name}", solutionRoot);

        return Path.Combine(solutionRoot, $"{name}.sln");
    }
}
