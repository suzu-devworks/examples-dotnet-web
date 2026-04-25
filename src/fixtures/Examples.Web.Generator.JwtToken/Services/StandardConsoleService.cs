using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using Spectre.Console.Json;
using static Crayon.Output;

namespace Examples.Web.Generator.JwtToken.Services;

/// <summary>
/// A standard implementation of the IConsoleService interface that interacts with the console using System.Console.
/// </summary>
/// <remarks>
/// To account for redirects, output data (token, JWKS JSON) is written to standard output,
/// and all other messages (status, errors, etc.) are written to standard error.
/// </remarks>
public class StandardConsoleService : IConsoleService
{
    private static readonly IAnsiConsole ErrorConsole = AnsiConsole.Create(new AnsiConsoleSettings
    {
        Ansi = AnsiSupport.Detect,
        ColorSystem = ColorSystemSupport.Detect,
        Out = new AnsiConsoleOutput(Console.Error),
    });

    public string PromptPassword(string message, bool showAsterisk = true)
    {
        Console.Error.Write($"{Yellow(message)}");
        return ReadPasswordLine(showAsterisk);

        static string ReadPasswordLine(bool showAsterisk)
        {
            var password = new System.Text.StringBuilder();
            while (true)
            {
                // If you pass true, the entered key will not be displayed in the console.
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    // Move the cursor back one step, erase with a blank space, and then move it back one step again.
                    if (showAsterisk) { Console.Write("\b \b"); }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    if (showAsterisk) { Console.Write("*"); }
                }
            }
            Console.Error.WriteLine();
            return password.ToString();
        }
    }

    public void WriteException(Exception exception)
    {
        Console.Error.WriteLine($"{Red("Error:")} {exception}");
    }

    public void WriteLine()
    {
        Console.Error.WriteLine();
    }

    public void WriteMessage(string message, ConsoleColor? color = null)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }
        else
        {
            Console.Error.WriteLine(message);
        }
    }

    public void WriteSuccess(string message)
    {
        Console.Error.WriteLine($"{Green("Success:")} {message}");
    }

    public void WriteToken(string message, string token)
    {
        Console.Error.WriteLine($"----- {Blue(message)} -----");

        if (Console.IsOutputRedirected)
        {
            // If output is redirected, write the token to standard output without color.
            Console.Out.WriteLine(token);
        }
        else
        {
            Console.WriteLine($"{Underline().Cyan(token)}");
        }
    }

    public void WriteClaims(string message, TokenValidationResult result)
    {
        Console.Error.WriteLine($"----- {Blue(message)} -----");

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Claim Type[/]");
        table.AddColumn("[yellow]Value[/]");

        foreach (var claim in result.Claims)
        {
            var value = claim.Value is IList<object> list
                ? $"[ {string.Join(", ", list.Select(item => item.ToString()))} ]"
                : claim.Value;
            table.AddRow(claim.Key, Markup.Escape(value?.ToString() ?? "null"));
        }

        ErrorConsole.Write(table);
    }

    public void WriteJson(string message, string jsonString)
    {
        Console.Error.WriteLine($"----- {Blue(message)} -----");

        if (Console.IsOutputRedirected)
        {
            // If output is redirected, write the JSON string to standard output without color.
            Console.Out.WriteLine(jsonString);
        }
        else
        {
            AnsiConsole.Write(new JsonText(jsonString));
            Console.Error.WriteLine();
        }
    }
}
