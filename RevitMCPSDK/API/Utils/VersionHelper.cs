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

namespace RevitMCPSDK.API.Utils;

/// <summary>
///     Version comparison helper class
/// </summary>
public static class VersionHelper
{
    /// <summary>
    ///     Parses a version number string into a Version object
    /// </summary>
    public static Version ParseVersion(string versionString)
    {
        if (string.IsNullOrEmpty(versionString))
            return null;

        if (Version.TryParse(versionString, out var version))
            return version;

        // Handle cases with only the year, e.g., "2022"
        if (int.TryParse(versionString, out var year))
            return new Version(year, 0, 0, 0);

        return null;
    }

    /// <summary>
    ///     Compares two version numbers
    /// </summary>
    public static int CompareVersions(string version1, string version2)
    {
        var v1 = ParseVersion(version1);
        var v2 = ParseVersion(version2);

        if (v1 == null && v2 == null)
            return 0;

        if (v1 == null)
            return -1;

        if (v2 == null)
            return 1;

        return v1.CompareTo(v2);
    }
}