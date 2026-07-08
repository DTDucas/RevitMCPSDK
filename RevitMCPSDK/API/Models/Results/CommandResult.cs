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

namespace RevitMCPSDK.API.Models.Results;

/// <summary>
///     Command execution result
/// </summary>
public class CommandResult
{
    /// <summary>
    ///     Indicates if the command was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    ///     Result data
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    ///     Error message, if any
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    ///     Creates a successful result
    /// </summary>
    public static CommandResult CreateSuccess(object data = null)
    {
        return new CommandResult
        {
            Success = true,
            Data = data,
            ErrorMessage = null
        };
    }

    /// <summary>
    ///     Creates a failed result
    /// </summary>
    public static CommandResult CreateError(string errorMessage, object data = null)
    {
        return new CommandResult
        {
            Success = false,
            Data = data,
            ErrorMessage = errorMessage
        };
    }
}