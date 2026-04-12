using System.Globalization;
using FluentValidation;

namespace Examples.Web.WebApi.Grpc.Validation;

/// <summary>
/// Custom validators for FluentValidation to validate date strings, time strings, and integer ranges.
/// </summary>
public static class CustomValidators
{
    /// <summary>
    /// Validates that a string is a valid date in the format "yyyy-MM-dd".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(dateString =>
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return false;
            }

            return DateOnly.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }).WithMessage("The date must be in the format 'yyyy-MM-dd'.");
    }

    /// <summary>
    /// Validates that a string is a valid time in the format "H:mm:ss".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsValidTime<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(timeString =>
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return false;
            }

            return TimeOnly.TryParseExact(timeString, "H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }).WithMessage("The time must be in the format 'H:mm:ss'.");
    }

    /// <summary>
    /// Validates that an integer value is within a specified range (inclusive).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, int> IsWithinRange<T>(this IRuleBuilder<T, int> ruleBuilder, int min, int max)
    {
        return ruleBuilder.Must(value => value >= min && value <= max)
            .WithMessage($"The value must be between {min} and {max}.");
    }
}
