namespace Examples.Web.Infrastructure.Extensions;

/// <summary>
/// Extension methods for working with file system paths.
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// Converts a user-provided path string into a <see cref="SafePath"/> instance,
    /// which validates the path against a whitelist of allowed locations to prevent directory traversal attacks.
    /// </summary>
    /// <param name="input">The user-provided path string.</param>
    /// <returns>A <see cref="SafePath"/> instance representing the validated path.</returns>
    public static SafePath ToSafePath(this string input)
        => new SafePath(input);
}
