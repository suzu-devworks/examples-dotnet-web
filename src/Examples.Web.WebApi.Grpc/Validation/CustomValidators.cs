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
        const string dateFormat = "yyyy-MM-dd";
        return ruleBuilder.Must(dateString =>
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return false;
            }

            return DateOnly.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }).WithMessage($"The date must be in the format '{dateFormat}'.");
    }

    /// <summary>
    /// Validates that a string is a valid time in the format "HH:mm:ss".
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsValidTime<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        const string timeFormat = "HH:mm:ss";
        return ruleBuilder.Must(timeString =>
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return false;
            }

            return TimeOnly.TryParseExact(timeString, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }).WithMessage($"The time must be in the format '{timeFormat}   '.");
    }

    /// <summary>
    /// Validates that a date string (format "yyyy-MM-dd") is after a reference date obtained from the model.
    /// If either value cannot be parsed, validation passes to avoid blocking independent validation errors.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="getFromValue">A function to retrieve the "from" date string from the model.</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> IsAfterAsDate<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> getFromValue)
    {
        const string dateFormat = "yyyy-MM-dd";
        return ruleBuilder.Must((request, dateTo) =>
        {
            if (!TryParse(getFromValue(request), dateFormat, out var from))
            {
                return true;
            }

            if (!TryParse(dateTo, dateFormat, out var to))
            {
                return true;
            }

            return from < to;
        }).WithMessage($"The date must be after the from date in the format '{dateFormat}'.");

        static bool TryParse(string dateTo, string dateFormat, out DateOnly to)
        {
            return DateOnly.TryParseExact(dateTo, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out to);
        }
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
        const string timeFormat = "HH:mm:ss";
        return ruleBuilder.Must((request, timeTo) =>
        {
            if (!TryParse(getFromValue(request), timeFormat, out var from))
            {
                return true;
            }

            if (!TryParse(timeTo, timeFormat, out var to))
            {
                return true;
            }

            return from < to;
        }).WithMessage($"The time must be after the from time in the format '{timeFormat}'.");

        static bool TryParse(string timeTo, string timeFormat, out TimeOnly to)
        {
            return TimeOnly.TryParseExact(timeTo, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out to);
        }
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
