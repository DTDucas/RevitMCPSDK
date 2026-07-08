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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RevitMCPSDK.API.Models.JsonRPC;

/// <summary>
///     Base interface for JSON-RPC 2.0 responses
/// </summary>
public interface IJsonRPCResponse
{
    /// <summary>
    ///     JSON-RPC version, always "2.0"
    /// </summary>
    string JsonRpc { get; }

    /// <summary>
    ///     Request ID, used to associate requests and responses
    /// </summary>
    string Id { get; set; }

    /// <summary>
    ///     Converts the response to a JSON string
    /// </summary>
    string ToJson();
}

/// <summary>
///     JSON-RPC 2.0 success response
/// </summary>
public class JsonRPCSuccessResponse : IJsonRPCResponse
{
    /// <summary>
    ///     Response result
    /// </summary>
    [JsonProperty("result")]
    public JToken Result { get; set; }

    /// <summary>
    ///     JSON-RPC version
    /// </summary>
    [JsonProperty("jsonrpc")]
    public string JsonRpc { get; set; } = "2.0";

    /// <summary>
    ///     Request ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Converts the response to a JSON string
    /// </summary>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

/// <summary>
///     JSON-RPC 2.0 error response
/// </summary>
public class JsonRPCErrorResponse : IJsonRPCResponse
{
    /// <summary>
    ///     Error information
    /// </summary>
    [JsonProperty("error")]
    public JsonRPCError Error { get; set; }

    /// <summary>
    ///     JSON-RPC version
    /// </summary>
    [JsonProperty("jsonrpc")]
    public string JsonRpc { get; set; } = "2.0";

    /// <summary>
    ///     Request ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Converts the response to a JSON string
    /// </summary>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

/// <summary>
///     JSON-RPC 2.0 error object
/// </summary>
public class JsonRPCError
{
    /// <summary>
    ///     Error code
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    ///     Error message
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; }

    /// <summary>
    ///     Optional error data
    /// </summary>
    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public JToken Data { get; set; }
}