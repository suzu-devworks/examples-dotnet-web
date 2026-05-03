using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Examples.Web.Infrastructure;

/// <summary>
/// Represents a file system path that has been validated against
/// a whitelist of allowed paths to prevent directory traversal attacks.
/// </summary>
public readonly record struct SafePath
{
    private readonly string _path;
    private static readonly string[] AllowedRoots;

    static SafePath()
    {
        var rawRoots = new List<string> { AppContext.BaseDirectory };
        var envPaths = Environment.GetEnvironmentVariable("ALLOWED_SAFE_PATHS");

        if (!string.IsNullOrWhiteSpace(envPaths))
            rawRoots.AddRange(envPaths.Split(',', StringSplitOptions.RemoveEmptyEntries));

        AllowedRoots = rawRoots
            .Select(p => Path.GetFullPath(p.Trim()))
            .Select(p => p.EndsWith(Path.DirectorySeparatorChar) ? p : p + Path.DirectorySeparatorChar)
            .Distinct()
            .OrderByDescending(p => p.Length)
            .ToArray();
    }

    public SafePath(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
        {
            throw new ArgumentException("Path cannot be empty");
        }

        var normalizedInput = Path.GetFullPath(userInput);

        if (!IsPathAllowed(normalizedInput))
        {
            throw new UnauthorizedAccessException($"Access denied: '{normalizedInput}' is not in allowed locations.");
        }

        _path = normalizedInput;

        bool IsPathAllowed(string path)
        {
            var inputSpan = path.AsSpan();

            foreach (var root in AllowedRoots)
            {
                var rootSpan = root.AsSpan();

                if (inputSpan.StartsWith(rootSpan, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public override readonly string ToString() => _path;

    public static implicit operator string(SafePath safePath) => safePath._path;
}
