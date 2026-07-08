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
/////     Extension methods for Revit Parameter operations
///// </summary>
//public static class ParameterExtensions
//{
//    /// <summary>
//    ///     Attempts to get a parameter by name
//    /// </summary>
//    /// <param name="element">The element</param>
//    /// <param name="parameterName">The parameter name</param>
//    /// <returns>The parameter if found, otherwise null</returns>
//    public static Parameter GetParameterByName(this Element element, string parameterName)
//    {
//        if (element == null || string.IsNullOrEmpty(parameterName))
//            return null;

//        return element.Parameters
//            .Cast<Parameter>()
//            .FirstOrDefault(p => p.Definition.Name == parameterName);
//    }

//    /// <summary>
//    ///     Gets a parameter by built-in parameter
//    /// </summary>
//    /// <param name="element">The element</param>
//    /// <param name="builtInParameter">The built-in parameter</param>
//    /// <returns>The parameter if found, otherwise null</returns>
//    public static Parameter GetParameter(this Element element, BuiltInParameter builtInParameter)
//    {
//        if (element == null)
//            return null;

//        return element.get_Parameter(builtInParameter);
//    }

//    /// <summary>
//    ///     Sets a parameter value with automatic type conversion
//    /// </summary>
//    /// <param name="parameter">The parameter</param>
//    /// <param name="value">The value to set</param>
//    /// <returns>True if successful</returns>
//    public static bool SetParameterValue(this Parameter parameter, object value)
//    {
//        if (parameter == null || !parameter.HasValue || parameter.IsReadOnly)
//            return false;

//        try
//        {
//            switch (parameter.StorageType)
//            {
//                case StorageType.Double:
//                    if (value is double doubleValue)
//                        return parameter.Set(doubleValue);
//                    if (double.TryParse(value.ToString(), out var parsedDouble)) return parameter.Set(parsedDouble);
//                    break;

//                case StorageType.Integer:
//                    if (value is int intValue)
//                        return parameter.Set(intValue);
//                    if (int.TryParse(value.ToString(), out var parsedInt)) return parameter.Set(parsedInt);
//                    break;

//                case StorageType.String:
//                    return parameter.Set(value?.ToString() ?? string.Empty);

//                case StorageType.ElementId:
//                    if (value is ElementId elementId)
//                        return parameter.Set(elementId);
//                    if (value is int idInt)
//                        return parameter.Set(new ElementId(idInt));
//                    if (int.TryParse(value.ToString(), out var parsedIdInt))
//                        return parameter.Set(new ElementId(parsedIdInt));
//                    break;
//            }

//            return false;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    ///     Gets a parameter value with automatic type conversion
//    /// </summary>
//    /// <typeparam name="T">The target type</typeparam>
//    /// <param name="parameter">The parameter</param>
//    /// <param name="defaultValue">Default value if conversion fails</param>
//    /// <returns>The parameter value</returns>
//    public static T GetParameterValue<T>(this Parameter parameter, T defaultValue = default)
//    {
//        if (parameter == null || !parameter.HasValue)
//            return defaultValue;

//        try
//        {
//            object value = null;

//            switch (parameter.StorageType)
//            {
//                case StorageType.Double:
//                    value = parameter.AsDouble();
//                    break;

//                case StorageType.Integer:
//                    value = parameter.AsInteger();
//                    break;

//                case StorageType.String:
//                    value = parameter.AsString();
//                    break;

//                case StorageType.ElementId:
//                    value = parameter.AsElementId();
//                    break;

//                default:
//                    return defaultValue;
//            }

//            if (value == null)
//                return defaultValue;

//            if (typeof(T) == value.GetType())
//                return (T)value;

//            if (typeof(T) == typeof(string))
//                return (T)(object)value.ToString();

//            if (typeof(T) == typeof(double) && value is int intVal)
//                return (T)(object)Convert.ToDouble(intVal);

//            if (typeof(T) == typeof(int) && value is double doubleVal)
//                return (T)(object)Convert.ToInt32(doubleVal);

//            if (typeof(T) == typeof(ElementId) && value is int idIntVal)
//                return (T)(object)new ElementId(idIntVal);

//            if (typeof(T) == typeof(int) && value is ElementId elementId)
//                return (T)(object)elementId.IntegerValue;

//            return (T)Convert.ChangeType(value, typeof(T));
//        }
//        catch
//        {
//            return defaultValue;
//        }
//    }

//    /// <summary>
//    ///     Sets a parameter value by name
//    /// </summary>
//    /// <param name="element">The element</param>
//    /// <param name="parameterName">The parameter name</param>
//    /// <param name="value">The value to set</param>
//    /// <returns>True if successful</returns>
//    public static bool SetParameterValue(this Element element, string parameterName, object value)
//    {
//        if (element == null || string.IsNullOrEmpty(parameterName))
//            return false;

//        var param = element.GetParameterByName(parameterName);
//        return param != null && param.SetParameterValue(value);
//    }

//    /// <summary>
//    ///     Sets a parameter value by built-in parameter
//    /// </summary>
//    /// <param name="element">The element</param>
//    /// <param name="builtInParameter">The built-in parameter</param>
//    /// <param name="value">The value to set</param>
//    /// <returns>True if successful</returns>
//    public static bool SetParameterValue(this Element element, BuiltInParameter builtInParameter, object value)
//    {
//        if (element == null)
//            return false;

//        var param = element.GetParameter(builtInParameter);
//        return param != null && param.SetParameterValue(value);
//    }

//    /// <summary>
//    ///     Gets a parameter value by name
//    /// </summary>
//    /// <typeparam name="T">The target type</typeparam>
//    /// <param name="element">The element</param>
//    /// <param name="parameterName">The parameter name</param>
//    /// <param name="defaultValue">Default value if parameter not found or conversion fails</param>
//    /// <returns>The parameter value</returns>
//    public static T GetParameterValue<T>(this Element element, string parameterName, T defaultValue = default)
//    {
//        if (element == null || string.IsNullOrEmpty(parameterName))
//            return defaultValue;

//        var param = element.GetParameterByName(parameterName);
//        return param != null ? param.GetParameterValue(defaultValue) : defaultValue;
//    }

//    /// <summary>
//    ///     Gets a parameter value by built-in parameter
//    /// </summary>
//    /// <typeparam name="T">The target type</typeparam>
//    /// <param name="element">The element</param>
//    /// <param name="builtInParameter">The built-in parameter</param>
//    /// <param name="defaultValue">Default value if parameter not found or conversion fails</param>
//    /// <returns>The parameter value</returns>
//    public static T GetParameterValue<T>(this Element element, BuiltInParameter builtInParameter,
//        T defaultValue = default)
//    {
//        if (element == null)
//            return defaultValue;

//        var param = element.GetParameter(builtInParameter);
//        return param != null ? param.GetParameterValue(defaultValue) : defaultValue;
//    }
//}