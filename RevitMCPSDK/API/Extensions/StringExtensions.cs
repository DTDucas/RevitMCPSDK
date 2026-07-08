//// 
////                       RevitAPI-Solutions
//// Copyright (c) Duong Tran Quang (DTDucas) (contact@dtducas.com / baymax.contact@gmail.com)
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//// SOFTWARE.
////

//using System.Globalization;
//using System.Text.RegularExpressions;

//namespace RevitMCPSDK.API.Extensions;

///// <summary>
/////     Extension methods for string operations
///// </summary>
//public static class StringExtensions
//{
//    /// <summary>
//    ///     Safely converts a string to an integer
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <param name="defaultValue">Default value if conversion fails</param>
//    /// <returns>The parsed integer or default value</returns>
//    public static int ToInt(this string value, int defaultValue = 0)
//    {
//        if (string.IsNullOrWhiteSpace(value))
//            return defaultValue;

//        return int.TryParse(value, out var result) ? result : defaultValue;
//    }

//    /// <summary>
//    ///     Safely converts a string to a double
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <param name="defaultValue">Default value if conversion fails</param>
//    /// <returns>The parsed double or default value</returns>
//    public static double ToDouble(this string value, double defaultValue = 0.0)
//    {
//        if (string.IsNullOrWhiteSpace(value))
//            return defaultValue;

//        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ||
//               double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result)
//            ? result
//            : defaultValue;
//    }

//    /// <summary>
//    ///     Safely converts a string to a boolean
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <param name="defaultValue">Default value if conversion fails</param>
//    /// <returns>The parsed boolean or default value</returns>
//    public static bool ToBool(this string value, bool defaultValue = false)
//    {
//        if (string.IsNullOrWhiteSpace(value))
//            return defaultValue;

//        value = value.Trim().ToLowerInvariant();

//        if (value == "1" || value == "yes" || value == "true" || value == "y" || value == "t")
//            return true;

//        if (value == "0" || value == "no" || value == "false" || value == "n" || value == "f")
//            return false;

//        return bool.TryParse(value, out var result) ? result : defaultValue;
//    }

//    /// <summary>
//    ///     Converts a string to PascalCase
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <returns>The PascalCase string</returns>
//    public static string ToPascalCase(this string value)
//    {
//        if (string.IsNullOrEmpty(value))
//            return value;

//        var words = Regex.Split(value, @"[\W_]+");

//        for (var i = 0; i < words.Length; i++)
//            if (!string.IsNullOrEmpty(words[i]))
//                words[i] = char.ToUpperInvariant(words[i][0]) +
//                           (words[i].Length > 1 ? words[i].Substring(1).ToLowerInvariant() : "");

//        return string.Join("", words);
//    }

//    /// <summary>
//    ///     Converts a string to camelCase
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <returns>The camelCase string</returns>
//    public static string ToCamelCase(this string value)
//    {
//        if (string.IsNullOrEmpty(value))
//            return value;

//        var pascal = value.ToPascalCase();
//        if (pascal.Length > 0) return char.ToLowerInvariant(pascal[0]) + pascal.Substring(1);

//        return pascal;
//    }

//    /// <summary>
//    ///     Ensures a string is not null or empty
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <param name="defaultValue">Default value if null or empty</param>
//    /// <returns>The original string or default value</returns>
//    public static string EnsureNotEmpty(this string value, string defaultValue = "")
//    {
//        return string.IsNullOrEmpty(value) ? defaultValue : value;
//    }

//    /// <summary>
//    ///     Truncates a string to a maximum length
//    /// </summary>
//    /// <param name="value">The string value</param>
//    /// <param name="maxLength">Maximum length</param>
//    /// <param name="suffix">Suffix to add if truncated</param>
//    /// <returns>The truncated string</returns>
//    public static string Truncate(this string value, int maxLength, string suffix = "...")
//    {
//        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
//            return value;

//        var truncateLength = maxLength - suffix.Length;
//        if (truncateLength < 1)
//            return suffix;

//        return value.Substring(0, truncateLength) + suffix;
//    }
//}