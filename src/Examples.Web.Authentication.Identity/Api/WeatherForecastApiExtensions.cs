namespace Examples.Web.Authentication.Identity.Api;

public static class WeatherForecastApiExtensions
{
    public static RouteHandlerBuilder MapWeatherForecastApi(this IEndpointRouteBuilder endpoints)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // spell-checker: words weatherforecast
        return endpoints.MapGet("/weatherforecast", () =>
         {
             var forecast = Enumerable.Range(1, 5).Select(index =>
                 new WeatherForecast
                 (
                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                     Random.Shared.Next(-20, 55),
                     summaries[Random.Shared.Next(summaries.Length)]
                 ))
                 .ToArray();
             return forecast;
         })
         .WithName("GetWeatherForecast")
         .AddOpenApiOperationTransformer((operation, context, token) =>
         {
             operation.Summary = "Get weather forecast";
             operation.Description = "Get a list of weather forecasts for the next 5 days.";
             return Task.FromResult(operation);
         });
    }

}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}