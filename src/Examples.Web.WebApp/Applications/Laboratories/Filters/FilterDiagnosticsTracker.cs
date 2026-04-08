using System.Globalization;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters;

public static class FilterDiagnosticsTracker
{
    private const string PassedFiltersKey = "Laboratory.FilterDiagnostics.PassedFilters";
    private const string TimelineKey = "Laboratory.FilterDiagnostics.Timeline";

    public static void Record(HttpContext context, string filterName, string stage)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(filterName);
        ArgumentException.ThrowIfNullOrWhiteSpace(stage);

        var normalizedName = filterName.Trim();
        var normalizedStage = stage.Trim();

        var passedFilters = GetOrCreatePassedFilters(context);
        if (!passedFilters.Contains(normalizedName, StringComparer.Ordinal))
        {
            passedFilters.Add(normalizedName);
        }

        var timeline = GetOrCreateTimeline(context);
        timeline.Add(string.Create(CultureInfo.InvariantCulture, $"{normalizedName}.{normalizedStage}"));
    }

    public static IReadOnlyList<string> GetPassedFilters(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return GetOrCreatePassedFilters(context).AsReadOnly();
    }

    public static IReadOnlyList<string> GetTimeline(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return GetOrCreateTimeline(context).AsReadOnly();
    }

    public static string GetPassedFiltersHeaderValue(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return string.Join(",", GetOrCreatePassedFilters(context));
    }

    public static string GetTimelineHeaderValue(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return string.Join("|", GetOrCreateTimeline(context));
    }

    private static List<string> GetOrCreatePassedFilters(HttpContext context)
    {
        if (context.Items.TryGetValue(PassedFiltersKey, out var value) && value is List<string> list)
        {
            return list;
        }

        var created = new List<string>();
        context.Items[PassedFiltersKey] = created;
        return created;
    }

    private static List<string> GetOrCreateTimeline(HttpContext context)
    {
        if (context.Items.TryGetValue(TimelineKey, out var value) && value is List<string> list)
        {
            return list;
        }

        var created = new List<string>();
        context.Items[TimelineKey] = created;
        return created;
    }
}
