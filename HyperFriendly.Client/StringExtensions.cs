using System;

namespace HyperFriendly.Client
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
    }
}