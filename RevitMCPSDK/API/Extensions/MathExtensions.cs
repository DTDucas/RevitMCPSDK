// 
//                       RevitAPI-Solutions
// Copyright (c) Duong Tran Quang (DTDucas) (contact@dtducas.com / baymax.contact@gmail.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

namespace RevitMCPSDK.API.Extensions;

/// <summary>
///     Extension methods for mathematical operations and unit conversions
/// </summary>
public static class MathExtensions
{
    /// <summary>
    ///     The conversion factor from millimeters to feet (1 foot = 304.8 mm)
    /// </summary>
    public const double MM_TO_FEET = 1.0 / 304.8;

    /// <summary>
    ///     The conversion factor from feet to millimeters (1 foot = 304.8 mm)
    /// </summary>
    public const double FEET_TO_MM = 304.8;

    /// <summary>
    ///     The conversion factor from inches to feet
    /// </summary>
    public const double INCH_TO_FEET = 1.0 / 12.0;

    /// <summary>
    ///     The conversion factor from feet to inches
    /// </summary>
    public const double FEET_TO_INCH = 12.0;

    /// <summary>
    ///     Converts millimeters to feet (Revit internal unit)
    /// </summary>
    /// <param name="millimeters">Length in millimeters</param>
    /// <returns>Length in feet</returns>
    public static double ToFeet(this double millimeters)
    {
        return millimeters * MM_TO_FEET;
    }

    /// <summary>
    ///     Converts feet (Revit internal unit) to millimeters
    /// </summary>
    /// <param name="feet">Length in feet</param>
    /// <returns>Length in millimeters</returns>
    public static double ToMillimeters(this double feet)
    {
        return feet * FEET_TO_MM;
    }

    /// <summary>
    ///     Converts inches to feet
    /// </summary>
    /// <param name="inches">Length in inches</param>
    /// <returns>Length in feet</returns>
    public static double InchesToFeet(this double inches)
    {
        return inches * INCH_TO_FEET;
    }

    /// <summary>
    ///     Converts feet to inches
    /// </summary>
    /// <param name="feet">Length in feet</param>
    /// <returns>Length in inches</returns>
    public static double FeetToInches(this double feet)
    {
        return feet * FEET_TO_INCH;
    }

    /// <summary>
    ///     Rounds a double value to the specified number of decimal places
    /// </summary>
    /// <param name="value">The value to round</param>
    /// <param name="decimals">Number of decimal places</param>
    /// <returns>The rounded value</returns>
    public static double RoundToDecimals(this double value, int decimals)
    {
        return Math.Round(value, decimals);
    }

    /// <summary>
    ///     Checks if a value is almost equal to another value within a tolerance
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <param name="target">The target value</param>
    /// <param name="tolerance">The tolerance (default is 1e-9)</param>
    /// <returns>Whether the values are almost equal</returns>
    public static bool IsAlmostEqual(this double value, double target, double tolerance = 1e-9)
    {
        return Math.Abs(value - target) < tolerance;
    }

    /// <summary>
    ///     Clamps a value between a minimum and maximum
    /// </summary>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>The clamped value</returns>
    public static double Clamp(this double value, double min, double max)
    {
        return Math.Max(min, Math.Min(max, value));
    }
}