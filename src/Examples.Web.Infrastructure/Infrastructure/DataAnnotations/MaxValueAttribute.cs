using System;
using System.ComponentModel.DataAnnotations;

namespace Examples.Web.Infrastructure.DataAnnotations;

#if NET8_0_OR_GREATER

public class MaxValueAttribute<T>(T maxValue) : ValidationAttribute
    where T : IComparable<T>
{
    public override bool IsValid(object? value)
    {
        return maxValue.CompareTo((T?)value) >= 0;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} should be less than or equal to {maxValue}.";
    }

}

#endif