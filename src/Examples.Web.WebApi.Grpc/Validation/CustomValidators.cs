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
    /// Validates that a string is a valid time in the format "HH:mm:ss".
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

            return TimeOnly.TryParseExact(timeString, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }).WithMessage("The time must be in the format 'HH:mm:ss'.");
    }

    /// <summary>
    /// Validates that a date string (format "yyyy-MM-dd") is after or equal to a reference date obtained from the model.
    /// If either value cannot be parsed, validation passes to avoid blocking independent validation errors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="getFromValue">A function to retrieve the "from" date string from the model.</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsAfterAsDate<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> getFromValue)
    {
        return ruleBuilder.Must((request, dateTo) =>
        {
            if (!DateOnly.TryParseExact(getFromValue(request), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
                return true;
            if (!DateOnly.TryParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
                return true;
            return to >= from;
        }).WithMessage("The date must be after or equal to the from date.");
    }

    /// <summary>
    /// Validates that a time string (format "HH:mm:ss") is after a reference time obtained from the model.
    /// If either value cannot be parsed, validation passes to avoid blocking independent validation errors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="getFromValue">A function to retrieve the "from" time string from the model.</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsAfterAsTime<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> getFromValue)
    {
        return ruleBuilder.Must((request, timeTo) =>
        {
            if (!TimeOnly.TryParseExact(getFromValue(request), "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
                return true;
            if (!TimeOnly.TryParseExact(timeTo, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
                return true;
            return to > from;
        }).WithMessage("The time must be after the from time.");
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
