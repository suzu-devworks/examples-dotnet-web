using System;
using System.ComponentModel.DataAnnotations;

namespace Examples.Web.Infrastructure.DataAnnotations;

#if NET8_0_OR_GREATER

public class MinValueAttribute<T>(T minValue) : ValidationAttribute
    where T : IComparable<T>
{
    public override bool IsValid(object? value)
    {
        return minValue.CompareTo((T?)value) <= 0;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} should be greater than or equal to {minValue}.";
    }

}

#endif