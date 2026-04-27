using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Generator.JwtToken.Services;

/// <summary>
/// An interface that abstracts console interactions.
/// </summary>
public interface IConsoleService
{
    /// <summary>
    /// Prompts the user for a password input.
    /// </summary>
    /// <param name="message">The message to display to the user.</param>
    /// <param name="showAsterisk">Whether to show asterisks for each character typed.</param>
    /// <returns>The password entered by the user.</returns>
    string PromptPassword(string message, bool showAsterisk = true);

    /// <summary>
    /// Writes an exception message to the console, potentially with formatting or additional details.
    /// </summary>
    /// <param name="exception">The exception to write to the console.</param>
    void WriteException(Exception exception);

    /// <summary>
    /// Writes a line to the console.
    /// </summary>
    void WriteLine();

    /// <summary>
    /// Writes a message to the console with an optional color.
    /// </summary>
    /// <param name="message">The message to write to the console.</param>
    /// <param name="color">The color to use for the message.</param>
    void WriteMessage(string message, ConsoleColor? color = null);

    /// <summary>
    /// Writes a success message to the console.
    /// </summary>
    /// <param name="message">The success message to write to the console.</param>
    void WriteSuccess(string message);

    /// <summary>
    /// Writes a JWT token to the console.
    /// </summary>
    /// <param name="message">The message to display before the token.</param>
    /// <param name="token">The JWT token to write to the console.</param>
    void WriteToken(string message, string token);

    /// <summary>
    /// Writes the claims from a token validation result to the console.
    /// </summary>
    /// <param name="message">The message to display before the claims.</param>
    /// <param name="result">The token validation result containing the claims.</param>
    void WriteClaims(string message, TokenValidationResult result);

    /// <summary>
    /// Writes a JSON string to the console.
    /// </summary>
    /// <param name="message">The message to display before the JSON string.</param>
    /// <param name="jsonString">The JSON string to write to the console.</param>
    void WriteJson(string message, string jsonString);
}
