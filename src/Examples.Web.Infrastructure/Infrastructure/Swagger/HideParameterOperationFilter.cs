using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property,
    AllowMultiple = false, Inherited = false)]
public class HideParameterAttribute : Attribute
{
}

public class HideParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        var parametersToHide = context.ApiDescription.ParameterDescriptions
            .Where(p => ParameterHasAttribute<HideParameterAttribute>(p));

        foreach (var param in parametersToHide)
        {
            var target = operation.Parameters
                .FirstOrDefault(p => p.Name == param.Name);

            if (target is not null)
            {
                operation.Parameters.Remove(target);
            }
        }

        return;
    }

    private static bool ParameterHasAttribute<TAttribute>(ApiParameterDescription description)
        where TAttribute : Attribute
    {
        if (description.ModelMetadata is DefaultModelMetadata metadata)
        {
            var result = metadata.Attributes.Attributes
                ?.Any(attr => attr is TAttribute);

            return result ?? false;
        }

        return false;
    }

}