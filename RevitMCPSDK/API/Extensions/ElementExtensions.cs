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
/////     Extension methods for Revit Element class
///// </summary>
//public static class ElementExtensions
//{
//    /// <summary>
//    ///     Gets all parameters of an element as a dictionary
//    /// </summary>
//    /// <param name="element">The Revit element</param>
//    /// <returns>Dictionary of parameter name/value pairs</returns>
//    public static Dictionary<string, string> GetParameterDictionary(this Element element)
//    {
//        if (element == null)
//            throw new ArgumentNullException(nameof(element));

//        var result = new Dictionary<string, string>();

//        foreach (Parameter param in element.Parameters)
//            if (param.HasValue)
//            {
//                var name = param.Definition.Name;
//                var value = GetParameterValueAsString(param);

//                if (!string.IsNullOrEmpty(value)) result.TryAdd(name, value);
//            }

//        return result;
//    }

//    /// <summary>
//    ///     Gets a parameter value as string, regardless of storage type
//    /// </summary>
//    /// <param name="parameter">The parameter</param>
//    /// <returns>String representation of the parameter value</returns>
//    public static string GetParameterValueAsString(this Parameter parameter)
//    {
//        if (parameter == null || !parameter.HasValue)
//            return string.Empty;

//        switch (parameter.StorageType)
//        {
//            case StorageType.String:
//                return parameter.AsString();

//            case StorageType.Integer:
//                return parameter.AsInteger().ToString();

//            case StorageType.Double:
//                return parameter.AsDouble().ToString("0.####");

//            case StorageType.ElementId:
//                return parameter.AsElementId().IntegerValue.ToString();

//            default:
//                return string.Empty;
//        }
//    }

//    /// <summary>
//    ///     Checks if the element belongs to a specific category
//    /// </summary>
//    /// <param name="element">The element</param>
//    /// <param name="builtInCategory">The built-in category</param>
//    /// <returns>True if the element belongs to the category</returns>
//    public static bool IsCategory(this Element element, BuiltInCategory builtInCategory)
//    {
//        if (element?.Category == null)
//            return false;

//        return element.Category.Id.IntegerValue == (int)builtInCategory;
//    }
//}