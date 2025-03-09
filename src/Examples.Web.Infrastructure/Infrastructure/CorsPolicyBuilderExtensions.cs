using System;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Examples.Web.Infrastructure;

#pragma warning disable IDE0305 // Use collection expression for fluent

public static class CorsPolicyBuilderExtensions
{
    public sealed class CorsPolicyOptions : CorsPolicy
    {
    }

    /// <summary>
    /// Configures CORS policy options.
    /// </summary>
    /// <param name="builder">The policy builder.</param>
    /// <param name="action"></param>
    /// <returns>The current policy builder.</returns>
    public static CorsPolicyBuilder Configure(this CorsPolicyBuilder builder, Action<CorsPolicyOptions> action)
    {
        var options = new CorsPolicyOptions();
        action.Invoke(options);

        builder
            .ConfigureOrigins(options)
            .ConfigureMethods(options)
            .ConfigureHeaders(options)
            .ConfigureExposedHeaders(options)
            .ConfigurePreflightMaxAge(options)
            .ConfigureCredentials(options)
            ;

        return builder;
    }

    private static CorsPolicyBuilder ConfigureOrigins(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.AllowAnyOrigin)
        {
            builder.AllowAnyOrigin();
        }
        else if (options.Origins.Count > 0)
        {
            // Verify origins?
            builder.WithOrigins(options.Origins.ToArray());
        }

        return builder;
    }

    private static CorsPolicyBuilder ConfigureMethods(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.AllowAnyMethod)
        {
            builder.AllowAnyMethod();
        }
        else if (options.Methods.Count > 0)
        {
            // Verify methods ?
            builder.WithMethods(options.Methods.ToArray());
        }

        return builder;
    }

    private static CorsPolicyBuilder ConfigureHeaders(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.AllowAnyHeader)
        {
            builder.AllowAnyHeader();
        }
        else if (options.Headers.Count > 0)
        {
            builder.WithHeaders(options.Headers.ToArray());
        }

        return builder;
    }

    private static CorsPolicyBuilder ConfigureExposedHeaders(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.ExposedHeaders.Count > 0)
        {
            builder.WithExposedHeaders(options.ExposedHeaders.ToArray());
        }

        return builder;
    }

    private static CorsPolicyBuilder ConfigurePreflightMaxAge(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.PreflightMaxAge is not null)
        {
            builder.SetPreflightMaxAge(options.PreflightMaxAge.Value);
        }

        return builder;
    }

    private static CorsPolicyBuilder ConfigureCredentials(this CorsPolicyBuilder builder, CorsPolicyOptions options)
    {
        if (options.SupportsCredentials)
        {
            builder.AllowCredentials();
        }

        return builder;
    }

}