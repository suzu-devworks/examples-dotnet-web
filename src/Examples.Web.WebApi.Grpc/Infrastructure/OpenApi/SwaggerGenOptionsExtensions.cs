using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure.OpenApi;

/// <summary>
/// Provides extension methods for configuring <see cref="SwaggerGenOptions"/>.
/// </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Adding triple-slash comments to an action enhances the Swagger UI
    /// by adding the description to the section header.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static SwaggerGenOptions IncludeXmlComments<TAssemblyMarker>(this SwaggerGenOptions options)
    {
        return options.IncludeXmlComments(typeof(TAssemblyMarker).Assembly);
    }

    /// <summary>
    /// Adding triple-slash comments to an action enhances the Swagger UI
    /// by adding the description to the section header.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static SwaggerGenOptions IncludeXmlComments(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFilePath = $"{assembly.GetName().Name}.xml";
        var path = Path.Combine(AppContext.BaseDirectory, xmlFilePath);
        options.IncludeXmlComments(path);

        return options;
    }




}
