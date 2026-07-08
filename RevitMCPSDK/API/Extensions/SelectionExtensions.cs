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
//using Autodesk.Revit.UI;
//using Autodesk.Revit.UI.Selection;
//using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

//namespace RevitMCPSDK.API.Extensions;

///// <summary>
/////     Extension methods for Revit selection operations
///// </summary>
//public static class SelectionExtensions
//{
//    /// <summary>
//    ///     Gets all selected elements
//    /// </summary>
//    /// <param name="uiDoc">The UI document</param>
//    /// <returns>Collection of selected elements</returns>
//    public static ICollection<Element> GetSelectedElements(this UIDocument uiDoc)
//    {
//        if (uiDoc == null)
//            throw new ArgumentNullException(nameof(uiDoc));

//        var selectedIds = uiDoc.Selection.GetElementIds();
//        if (selectedIds == null || selectedIds.Count == 0)
//            return new List<Element>();

//        var elements = new List<Element>();
//        foreach (var id in selectedIds)
//        {
//            var element = uiDoc.Document.GetElement(id);
//            if (element != null) elements.Add(element);
//        }

//        return elements;
//    }

//    /// <summary>
//    ///     Gets selected elements of a specific type
//    /// </summary>
//    /// <typeparam name="T">The element type</typeparam>
//    /// <param name="uiDoc">The UI document</param>
//    /// <returns>Collection of selected elements of the specified type</returns>
//    public static ICollection<T> GetSelectedElementsOfType<T>(this UIDocument uiDoc) where T : Element
//    {
//        if (uiDoc == null)
//            throw new ArgumentNullException(nameof(uiDoc));

//        return uiDoc.GetSelectedElements()
//            .OfType<T>()
//            .ToList();
//    }

//    /// <summary>
//    ///     Gets selected elements of a specific category
//    /// </summary>
//    /// <param name="uiDoc">The UI document</param>
//    /// <param name="category">The built-in category</param>
//    /// <returns>Collection of selected elements of the specified category</returns>
//    public static ICollection<Element> GetSelectedElementsByCategory(this UIDocument uiDoc, BuiltInCategory category)
//    {
//        if (uiDoc == null)
//            throw new ArgumentNullException(nameof(uiDoc));

//        return uiDoc.GetSelectedElements()
//            .Where(e => e.Category?.Id.IntegerValue == (int)category)
//            .ToList();
//    }

//    /// <summary>
//    ///     Selects an element in the Revit UI
//    /// </summary>
//    /// <param name="uiDoc">The UI document</param>
//    /// <param name="element">The element to select</param>
//    /// <param name="clearSelection">Whether to clear current selection first</param>
//    /// <returns>True if successful</returns>
//    public static bool SelectElement(this UIDocument uiDoc, Element element, bool clearSelection = true)
//    {
//        if (uiDoc == null || element == null)
//            return false;

//        try
//        {
//            if (clearSelection)
//                uiDoc.Selection.SetElementIds(new HashSet<ElementId>());

//            var ids = new HashSet<ElementId> { element.Id };
//            uiDoc.Selection.SetElementIds(ids);

//            return uiDoc.Selection.GetElementIds().Contains(element.Id);
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    ///     Selects multiple elements in the Revit UI
//    /// </summary>
//    /// <param name="uiDoc">The UI document</param>
//    /// <param name="elements">The elements to select</param>
//    /// <param name="clearSelection">Whether to clear current selection first</param>
//    /// <returns>Number of elements selected</returns>
//    public static int SelectElements(this UIDocument uiDoc, IEnumerable<Element> elements, bool clearSelection = true)
//    {
//        if (uiDoc == null || elements == null)
//            return 0;

//        try
//        {
//            var elementIds = new HashSet<ElementId>(
//                elements.Where(e => e != null).Select(e => e.Id)
//            );

//            if (elementIds.Count == 0)
//                return 0;

//            if (!clearSelection)
//            {
//                var currentSelection = new HashSet<ElementId>(uiDoc.Selection.GetElementIds());
//                foreach (var id in elementIds) currentSelection.Add(id);
//                elementIds = currentSelection;
//            }

//            uiDoc.Selection.SetElementIds(elementIds);

//            var selectedCount = 0;
//            foreach (var id in uiDoc.Selection.GetElementIds())
//                if (elementIds.Contains(id))
//                    selectedCount++;
//            return selectedCount;
//        }
//        catch
//        {
//            return 0;
//        }
//    }

//    /// <summary>
//    ///     Prompts the user to select an element of a specific type
//    /// </summary>
//    /// <typeparam name="T">The element type</typeparam>
//    /// <param name="uiDoc">The UI document</param>
//    /// <param name="statusPrompt">The prompt to display</param>
//    /// <returns>The selected element, or null if canceled</returns>
//    public static T PromptForElementSelection<T>(this UIDocument uiDoc, string statusPrompt) where T : Element
//    {
//        if (uiDoc == null)
//            return null;

//        try
//        {
//            var reference = uiDoc.Selection.PickObject(
//                ObjectType.Element,
//                new ElementTypeFilter(typeof(T)),
//                statusPrompt);

//            if (reference == null)
//                return null;

//            var element = uiDoc.Document.GetElement(reference);
//            return element as T;
//        }
//        catch (OperationCanceledException)
//        {
//            return null;
//        }
//        catch
//        {
//            return null;
//        }
//    }

//    /// <summary>
//    ///     Element type selection filter
//    /// </summary>
//    private class ElementTypeFilter(Type type) : ISelectionFilter
//    {
//        public bool AllowElement(Element element)
//        {
//            return element != null && type.IsAssignableFrom(element.GetType());
//        }

//        public bool AllowReference(Reference refer, XYZ point)
//        {
//            return false;
//        }
//    }
//}