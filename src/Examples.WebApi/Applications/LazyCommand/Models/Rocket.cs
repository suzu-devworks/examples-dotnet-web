using System.Collections.Generic;

namespace Examples.WebApi.Applications.LazyCommand.Models
{
    public class Rocket
    {
        public int RocketId { get; init; }

        public IEnumerable<Planet>? VisitedPlanets { get; set; }

    }
}