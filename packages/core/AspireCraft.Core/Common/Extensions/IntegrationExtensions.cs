using AspireCraft.Core.Common.Enums;

namespace AspireCraft.Core.Common.Extensions;

public static class IntegrationExtensions
{
    public static List<IntegrationType> AddIntegrations(this IReadOnlyList<IntegrationType> integrations, IntegrationType[] categories)
    {
        var filtered = integrations
            .Where(i => categories.Contains(i))
            .Select(i => i)
            .ToList();

        return filtered;
    }
}
