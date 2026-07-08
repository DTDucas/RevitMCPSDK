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

using Autodesk.Revit.ApplicationServices;

namespace RevitMCPSDK.API.Utils;

/// <summary>
///     Revit version adapter, used to handle compatibility issues between different versions
/// </summary>
public class RevitVersionAdapter(Application app)
{
    private readonly Application _app = app ?? throw new ArgumentNullException(nameof(app));

    /// <summary>
    ///     Gets the current Revit version number
    /// </summary>
    public string GetRevitVersion()
    {
        // Get the major version number, e.g., "2022"
        return _app.VersionNumber;
    }

    /// <summary>
    ///     Checks if the current Revit version supports the specified command
    /// </summary>
    /// <param name="supportedVersions">List of supported versions for the command</param>
    /// <returns>Whether the current version is supported</returns>
    public bool IsVersionSupported(IEnumerable<string> supportedVersions)
    {
        if (supportedVersions == null || !supportedVersions.GetEnumerator().MoveNext())
            return true; // If no supported versions are specified, default to supporting all versions

        var currentVersion = GetRevitVersion();

        foreach (var version in supportedVersions)
            if (currentVersion == version)
                return true;

        return false;
    }
}