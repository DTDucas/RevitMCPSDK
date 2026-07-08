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
///     Represents a JSON-RPC 2.0 request object
/// </summary>
public class JsonRPCRequest
{
    /// <summary>
    ///     JSON-RPC version, must be "2.0"
    /// </summary>
    [JsonProperty("jsonrpc")]
    public string JsonRpc { get; set; }

    /// <summary>
    ///     The name of the method to call
    /// </summary>
    [JsonProperty("method")]
    public string Method { get; set; }

    /// <summary>
    ///     Parameters for the method call
    ///     Can be an object or an array
    /// </summary>
    [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
    public JToken Params { get; set; }

    /// <summary>
    ///     Request ID, used to match responses
    ///     For notification requests, ID is null
    /// </summary>
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    ///     Checks if the request is a notification
    ///     Notifications are requests without an ID and do not require a response
    /// </summary>
    [JsonIgnore]
    public bool IsNotification => string.IsNullOrEmpty(Id);

    /// <summary>
    ///     Validates whether the request is valid
    /// </summary>
    /// <summary>
    ///     Validates whether the request is valid
    /// </summary>
    /// <returns>True if the request is valid, false otherwise</returns>
    public bool IsValid()
    {
        // jsonrpc must be "2.0"
        if (JsonRpc != "2.0")
            return false;

        // method cannot be empty
        if (string.IsNullOrEmpty(Method))
            return false;

        // The ID of a notification request must be null, the ID of a normal request cannot be null
        // Note: When a request is deserialized from JSON, a missing id property will make Id null, which is a valid notification request

        return true;
    }

    /// <summary>
    ///     Tries to get the parameters as an object of the specified type
    /// </summary>
    /// <typeparam name="T">The target type</typeparam>
    /// <param name="result">The conversion result</param>
    /// <returns>True if the conversion was successful, false otherwise</returns>
    public bool TryGetParamsAs<T>(out T result)
    {
        result = default;

        try
        {
            if (Params == null)
                return false;

            result = Params.ToObject<T>();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Gets the parameters as an object of the specified type
    ///     Throws an exception if the conversion fails
    /// </summary>
    /// <typeparam name="T">The target type</typeparam>
    /// <returns>The converted object</returns>
    /// <exception cref="JsonRPCSerializationException">Thrown when parameter conversion fails</exception>
    public T GetParamsAs<T>()
    {
        try
        {
            if (Params == null) throw new JsonRPCSerializationException("Request params is null");

            return Params.ToObject<T>();
        }
        catch (Exception ex) when (!(ex is JsonRPCSerializationException))
        {
            throw new JsonRPCSerializationException(
                $"Failed to convert params to type {typeof(T).Name}: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Gets the parameter value array
    /// </summary>
    /// <returns>The parameter array, or null if the parameter is not an array</returns>
    public JArray GetParamsArray()
    {
        return Params as JArray;
    }

    /// <summary>
    ///     Gets the parameter value object
    /// </summary>
    /// <returns>The parameter object, or null if the parameter is not an object</returns>
    public JObject GetParamsObject()
    {
        return Params as JObject;
    }

    /// <summary>
    ///     Tries to get the parameter value at the specified index from the array parameters
    /// </summary>
    /// <typeparam name="T">The target type</typeparam>
    /// <param name="index">The parameter index</param>
    /// <param name="value">The conversion result</param>
    /// <returns>True if the retrieval was successful, false otherwise</returns>
    public bool TryGetParamAt<T>(int index, out T value)
    {
        value = default;

        try
        {
            var array = GetParamsArray();
            if (array == null || index < 0 || index >= array.Count)
                return false;

            value = array[index].ToObject<T>();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Tries to get the parameter value of the specified property from the object parameters
    /// </summary>
    /// <typeparam name="T">The target type</typeparam>
    /// <param name="propertyName">The property name</param>
    /// <param name="value">The conversion result</param>
    /// <returns>True if the retrieval was successful, false otherwise</returns>
    public bool TryGetParam<T>(string propertyName, out T value)
    {
        value = default;

        try
        {
            var obj = GetParamsObject();
            if (obj == null || !obj.ContainsKey(propertyName))
                return false;

            value = obj[propertyName].ToObject<T>();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Creates a new request object
    /// </summary>
    /// <param name="method">The method name</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="id">The request ID, if null, a notification request is created</param>
    /// <returns>The created request object</returns>
    public static JsonRPCRequest Create(string method, object parameters = null, string id = null)
    {
        var paramsToken = parameters != null
            ? parameters is JToken token ? token : JToken.FromObject(parameters)
            : null;

        return new JsonRPCRequest
        {
            JsonRpc = "2.0",
            Method = method,
            Params = paramsToken,
            Id = id
        };
    }

    /// <summary>
    ///     Creates a notification request
    ///     Notifications are requests that do not require a response
    /// </summary>
    /// <param name="method">The method name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>The created notification request object</returns>
    public static JsonRPCRequest CreateNotification(string method, object parameters = null)
    {
        return Create(method, parameters);
    }

    /// <summary>
    ///     Converts the request to a JSON string
    /// </summary>
    /// <returns>The serialized JSON string</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}