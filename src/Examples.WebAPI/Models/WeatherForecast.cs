using System;

namespace Examples.WebAPI.Models;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public TimeSpan Timestamp { get; set; } = DateTime.Now.TimeOfDay;

    public Range ValueRange { get; set; } = new Range(4, 12);

}
