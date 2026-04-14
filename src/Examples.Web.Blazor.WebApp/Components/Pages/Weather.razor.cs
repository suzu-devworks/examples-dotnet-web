using Microsoft.JSInterop;

namespace Examples.Web.Blazor.WebApp.Components.Pages;

public partial class Weather : IAsyncDisposable
{
    private IJSObjectReference? _module;

    protected async Task CreateGraphAsync()
    {
        // Convert to Chart.js model
        var temperatures = new LineGraphData()
        {
            Labels = forecasts?.Select(f => f.Date.ToShortDateString()) ?? [],

            Datasets = new List<LineData>()
            {
                new LineData()
                {
                    Label = "Temp. (C)",
                    Data = forecasts?.Select(f => f.TemperatureC) ?? [],
                    BorderColor = "coral",
                },
                new LineData()
                {
                    Label = "Temp. (F)",
                    Data = forecasts?.Select(f => f.TemperatureF) ?? [],
                    BorderColor = "lightgreen",
                    // spell-checker: words lightgreen
                }
            }
        };

        _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./js/chart-module.js"
        );

        await _module.InvokeVoidAsync(
           "createGraph",
           graphCanvas,
           new LineGraph() { Data = temperatures }
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    private class LineData
    {
        public string Label { get; set; } = default!;
        public IEnumerable<int> Data { get; set; } = default!;
        public double Tension { get; set; } = 0.5;
        public string BorderColor { get; set; } = default!;
    }

    private class LineGraphData
    {
        public IEnumerable<string> Labels { get; set; } = default!;
        public IEnumerable<LineData> Datasets { get; set; } = default!;
    }

    private class LineGraph
    {
        public string Type { get; } = "line";
        public LineGraphData Data { get; set; } = default!;

        public LineOptions Options { get; set; } = new();
    }

    public class LineOptions
    {
        public bool Responsive { get; set; } = true;
        public bool MaintainAspectRatio { get; set; } = false;
    }
}
