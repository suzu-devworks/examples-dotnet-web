using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

public class OpenApiTagDescriptionSortDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = swaggerDoc.Tags
             .OrderBy(x => x.Description)
             .ThenBy(x => x.Name)
             .ToArray();

        return;
    }
}