using System;

namespace DomainModel
{
    internal static class Guard
    {
        public static void NotNullOrWhiteSpace(string value, string param)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", param);
        }

        public static void NonNegative(decimal value, string param)
        {
            if (value < 0m)
                throw new ArgumentOutOfRangeException(param, "Must be >= 0.");
        }
    }
}
