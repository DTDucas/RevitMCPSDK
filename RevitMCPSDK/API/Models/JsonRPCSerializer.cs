﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RevitMCPSDK.API.Models
{
    /// <summary>
    /// JSON-RPC request and response serialization/deserialization utility class
    /// </summary>
    public static class JsonRPCSerializer
    {
        /// <summary>
        /// Deserializes a JSON string into a JSON-RPC request object
        /// </summary>
        /// <param name="jsonString">The JSON-RPC request string</param>
        /// <returns>The deserialized request object</returns>
        public static JsonRPCRequest DeserializeRequest(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<JsonRPCRequest>(jsonString);
            }
            catch (JsonException ex)
            {
                throw new JsonRPCSerializationException("Failed to deserialize JSON-RPC request", ex);
            }
        }

        /// <summary>
        /// Creates a success response
        /// </summary>
        /// <param name="id">The request ID</param>
        /// <param name="result">The response result</param>
        /// <returns>The JSON-formatted success response</returns>
        public static string CreateSuccessResponse(string id, object result)
        {
            var response = new JsonRPCSuccessResponse
            {
                Id = id,
                Result = result is JToken jToken ? jToken : JToken.FromObject(result)
            };

            return response.ToJson();
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        /// <param name="id">The request ID</param>
        /// <param name="code">The error code</param>
        /// <param name="message">The error message</param>
        /// <param name="data">Additional error data</param>
        /// <returns>The JSON-formatted error response</returns>
        public static string CreateErrorResponse(string id, int code, string message, object data = null)
        {
            var response = new JsonRPCErrorResponse
            {
                Id = id,
                Error = new JsonRPCError
                {
                    Code = code,
                    Message = message,
                    Data = data != null ? JToken.FromObject(data) : null
                }
            };

            return response.ToJson();
        }

        /// <summary>
        /// Creates a parse error response
        /// </summary>
        /// <returns>The JSON-formatted parse error response</returns>
        public static string CreateParseErrorResponse()
        {
            return CreateErrorResponse(null, JsonRPCErrorCodes.ParseError, "Parse error");
        }

        /// <summary>
        /// Creates an invalid request response
        /// </summary>
        /// <returns>The JSON-formatted invalid request response</returns>
        public static string CreateInvalidRequestResponse()
        {
            return CreateErrorResponse(null, JsonRPCErrorCodes.InvalidRequest, "Invalid Request");
        }

        /// <summary>
        /// Tries to parse a JSON-RPC request
        /// </summary>
        /// <param name="jsonString">The JSON string</param>
        /// <param name="request">The parsed request object</param>
        /// <param name="errorResponse">The error response</param>
        /// <returns>Whether the parsing was successful</returns>
        public static bool TryParseRequest(string jsonString, out JsonRPCRequest request, out string errorResponse)
        {
            request = null;
            errorResponse = null;

            try
            {
                request = DeserializeRequest(jsonString);

                if (request == null || !request.IsValid())
                {
                    errorResponse = CreateInvalidRequestResponse();
                    return false;
                }

                return true;
            }
            catch (JsonException)
            {
                errorResponse = CreateParseErrorResponse();
                return false;
            }
            catch (Exception ex)
            {
                errorResponse = CreateErrorResponse(
                    null,
                    JsonRPCErrorCodes.InternalError,
                    $"Error processing request: {ex.Message}"
                );
                return false;
            }
        }
    }

    /// <summary>
    /// JSON-RPC serialization exception
    /// </summary>
    public class JsonRPCSerializationException : Exception
    {
        public JsonRPCSerializationException(string message) : base(message)
        {
        }

        public JsonRPCSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
