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

//using System.Reflection;
//using Autodesk.Revit.DB;

//namespace RevitMCPSDK.API.Extensions;

///// <summary>
/////     Extension methods for Revit View classes
///// </summary>
//public static class ViewExtensions
//{
//    /// <summary>
//    ///     Checks if a view is a 3D view
//    /// </summary>
//    /// <param name="view">The view to check</param>
//    /// <returns>True if the view is a 3D view</returns>
//    public static bool Is3DView(this View view)
//    {
//        if (view == null)
//            return false;

//        return view is View3D;
//    }

//    /// <summary>
//    ///     Sets the scale of a view
//    /// </summary>
//    /// <param name="view">The view</param>
//    /// <param name="scale">The scale denominator (e.g., 100 for 1:100)</param>
//    /// <returns>True if successful</returns>
//    public static bool SetScale(this View view, int scale)
//    {
//        if (view == null)
//            return false;

//        bool supportsScale = view is ViewPlan ||
//                             view is ViewSection ||
//                             view.GetType().Name == "ViewElevation" ||
//                             view.GetType().Name == "ViewDetail";

//        if (!supportsScale)
//            return false;

//        try
//        {
//            var scaleParam = view.get_Parameter(BuiltInParameter.VIEW_SCALE);
//            if (scaleParam != null && !scaleParam.IsReadOnly)
//            {
//                scaleParam.Set(scale);
//                return true;
//            }

//            return false;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    ///     Gets the bounding box of the view in model coordinates
//    /// </summary>
//    /// <param name="view">The view</param>
//    /// <returns>The bounding box, or null if not available</returns>
//    public static BoundingBoxXYZ GetViewBoundingBox(this View view)
//    {
//        if (view == null)
//            return null;

//        try
//        {
//            return view.CropBox;
//        }
//        catch
//        {
//            return null;
//        }
//    }

//    /// <summary>
//    ///     Alternative method to check if an element is hidden when the standard method fails
//    /// </summary>
//    /// <param name="view">The view to check</param>
//    /// <param name="element">The element to check</param>
//    /// <returns>True if the element is hidden</returns>
//    public static bool TryAlternativeIsHidden(View view, Element element)
//    {
//        try
//        {
//            var methods = view.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                .Where(m => m.Name == "IsHidden")
//                .ToList();

//            foreach (var method in methods)
//            {
//                var parameters = method.GetParameters();
//                if (parameters.Length == 1)
//                {
//                    var paramType = parameters[0].ParameterType;

//                    if (paramType == typeof(ElementId))
//                        return (bool)method.Invoke(view, [element.Id])!;
//                    if (paramType == typeof(Element))
//                        return (bool)method.Invoke(view, [element])!;
//                    if (paramType == typeof(int))
//                        return (bool)method.Invoke(view, [element.Id.IntegerValue]);
//                }
//            }

//            return false;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}