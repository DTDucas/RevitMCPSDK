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

//using Autodesk.Revit.DB;

//namespace RevitMCPSDK.API.Extensions;

///// <summary>
/////     Extension methods for Revit geometry classes
///// </summary>
//public static class GeometryExtensions
//{
//    /// <summary>
//    ///     Creates a Revit XYZ point from coordinates in millimeters
//    /// </summary>
//    /// <param name="xMm">X coordinate in millimeters</param>
//    /// <param name="yMm">Y coordinate in millimeters</param>
//    /// <param name="zMm">Z coordinate in millimeters</param>
//    /// <returns>XYZ point with coordinates converted to feet</returns>
//    public static XYZ CreateXYZFromMillimeters(double xMm, double yMm, double zMm)
//    {
//        return new XYZ(
//            xMm.ToFeet(),
//            yMm.ToFeet(),
//            zMm.ToFeet()
//        );
//    }

//    /// <summary>
//    ///     Converts an XYZ point to a string representation
//    /// </summary>
//    /// <param name="point">The XYZ point</param>
//    /// <returns>String representation of the point</returns>
//    public static string ToDisplayString(this XYZ point)
//    {
//        if (point == null)
//            return "null";

//        return $"({point.X.RoundToDecimals(3)}, {point.Y.RoundToDecimals(3)}, {point.Z.RoundToDecimals(3)})";
//    }

//    /// <summary>
//    ///     Converts an XYZ point to a 2D point by ignoring the Z coordinate
//    /// </summary>
//    /// <param name="point">The 3D XYZ point</param>
//    /// <returns>A new XYZ point with Z=0</returns>
//    public static XYZ To2D(this XYZ point)
//    {
//        if (point == null)
//            throw new ArgumentNullException(nameof(point));

//        return new XYZ(point.X, point.Y, 0);
//    }

//    /// <summary>
//    ///     Checks if an XYZ point is almost equal to another point within a tolerance
//    /// </summary>
//    /// <param name="point1">The first point</param>
//    /// <param name="point2">The second point</param>
//    /// <param name="tolerance">The tolerance (default is 1e-6)</param>
//    /// <returns>Whether the points are almost equal</returns>
//    public static bool IsAlmostEqualTo(this XYZ point1, XYZ point2, double tolerance = 1e-6)
//    {
//        if (point1 == null || point2 == null)
//            return false;

//        return point1.DistanceTo(point2) < tolerance;
//    }

//    /// <summary>
//    ///     Creates a line from two XYZ points
//    /// </summary>
//    /// <param name="startPoint">The start point</param>
//    /// <param name="endPoint">The end point</param>
//    /// <returns>A bounded line</returns>
//    public static Line CreateLine(XYZ startPoint, XYZ endPoint)
//    {
//        if (startPoint == null)
//            throw new ArgumentNullException(nameof(startPoint));
//        if (endPoint == null)
//            throw new ArgumentNullException(nameof(endPoint));

//        return Line.CreateBound(startPoint, endPoint);
//    }
//}