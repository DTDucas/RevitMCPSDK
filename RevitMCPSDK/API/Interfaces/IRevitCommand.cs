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

using Newtonsoft.Json.Linq;

namespace RevitMCPSDK.API.Interfaces;

/// <summary>
///     Interface that all Revit commands must implement
/// </summary>
public interface IRevitCommand
{
    /// <summary>
    ///     The unique name of the command, used to identify the command in JSON-RPC requests
    /// </summary>
    string CommandName { get; }

    /// <summary>
    ///     Executes the command
    /// </summary>
    /// <param name="parameters">JSON-RPC parameters</param>
    /// <param name="requestId">Request ID</param>
    /// <returns>The command execution result</returns>
    object Execute(JObject parameters, string requestId);
}