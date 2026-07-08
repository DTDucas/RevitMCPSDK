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

namespace RevitMCPSDK.API.Models.JsonRPC;

/// <summary>
///     JSON-RPC 2.0 Error Code Definitions
///     Contains standard error codes and custom error codes
/// </summary>
public static class JsonRPCErrorCodes
{
    /// <summary>
    ///     Gets the error description
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <returns>The description text of the error</returns>
    public static string GetErrorDescription(int errorCode)
    {
        switch (errorCode)
        {
            // Standard JSON-RPC errors
            case ParseError: return "Invalid JSON was received by the server.";
            case InvalidRequest: return "The JSON sent is not a valid Request object.";
            case MethodNotFound: return "The method does not exist / is not available.";
            case InvalidParams: return "Invalid method parameter(s).";
            case InternalError: return "Internal JSON-RPC error.";

            // Revit API errors
            case RevitApiError: return "Revit API operation failed.";
            case CommandExecutionTimeout: return "Command execution timed out.";
            case DocumentNotAvailable: return "Revit document is not available.";
            case TransactionFailed: return "Revit transaction failed.";
            case ElementNotFound: return "Revit element not found.";
            case ElementCreationFailed: return "Failed to create Revit element.";
            case ElementModificationFailed: return "Failed to modify Revit element.";
            case ElementDeletionFailed: return "Failed to delete Revit element.";
            case InvalidGeometryData: return "Invalid geometry data.";
            case ViewNotFound: return "Revit view not found.";

            // Plugin-specific errors
            case CommandRegistrationFailed: return "Failed to register command.";
            case ServiceStartupFailed: return "Failed to start service.";
            case ExternalEventCreationFailed: return "Failed to create external event.";
            case ExternalEventExecutionFailed: return "External event execution failed.";
            case CommandCancelled: return "Command was cancelled.";
            case CommandParameterParsingFailed: return "Failed to parse command parameters.";

            // General application errors
            case Unauthorized: return "Unauthorized access.";
            case ResourceUnavailable: return "Resource is unavailable.";
            case RequestTimeout: return "Request timed out.";
            case InvalidSession: return "Invalid session.";
            case ConfigurationError: return "Configuration error.";
            case IOError: return "I/O error.";

            // Server error range
            default:
                if (errorCode >= ServerErrorStart && errorCode <= ServerErrorEnd)
                    return "Server error.";
                return "Unknown error.";
        }
    }

    #region Standard JSON-RPC 2.0 Error Codes (-32768 to -32000)

    /// <summary>
    ///     Invalid JSON format.
    ///     The server received invalid JSON.
    /// </summary>
    public const int ParseError = -32700;

    /// <summary>
    ///     Invalid JSON-RPC request.
    ///     The JSON sent is not a valid Request object.
    /// </summary>
    public const int InvalidRequest = -32600;

    /// <summary>
    ///     The requested method does not exist or is unavailable.
    /// </summary>
    public const int MethodNotFound = -32601;

    /// <summary>
    ///     Invalid method parameters.
    ///     Method parameters are invalid or incorrectly formatted.
    /// </summary>
    public const int InvalidParams = -32602;

    /// <summary>
    ///     Internal JSON-RPC error.
    ///     Generic server error occurred while processing the request.
    /// </summary>
    public const int InternalError = -32603;

    /// <summary>
    ///     Start range for server errors.
    ///     Reserved for implementation-defined server errors.
    /// </summary>
    public const int ServerErrorStart = -32000;

    /// <summary>
    ///     End range for server errors.
    /// </summary>
    public const int ServerErrorEnd = -32099;

    #endregion

    #region Revit API Related Error Codes (-33000 to -33099)

    /// <summary>
    ///     Revit API error.
    ///     An error occurred while executing a Revit API operation.
    /// </summary>
    public const int RevitApiError = -33000;

    /// <summary>
    ///     Command execution timeout.
    ///     Revit command execution time exceeded the predefined timeout period.
    /// </summary>
    public const int CommandExecutionTimeout = -33001;

    /// <summary>
    ///     Current document unavailable.
    ///     Unable to get or access the current Revit document.
    /// </summary>
    public const int DocumentNotAvailable = -33002;

    /// <summary>
    ///     Transaction failed.
    ///     Revit transaction could not be committed or rolled back.
    /// </summary>
    public const int TransactionFailed = -33003;

    /// <summary>
    ///     Element not found.
    ///     The requested Revit element does not exist or has been deleted.
    /// </summary>
    public const int ElementNotFound = -33004;

    /// <summary>
    ///     Element creation failed.
    ///     Unable to create a new Revit element.
    /// </summary>
    public const int ElementCreationFailed = -33005;

    /// <summary>
    ///     Element modification failed.
    ///     Unable to modify the existing Revit element.
    /// </summary>
    public const int ElementModificationFailed = -33006;

    /// <summary>
    ///     Element deletion failed.
    ///     Unable to delete the Revit element.
    /// </summary>
    public const int ElementDeletionFailed = -33007;

    /// <summary>
    ///     Invalid geometry data.
    ///     The provided geometry data is invalid or incorrectly formatted.
    /// </summary>
    public const int InvalidGeometryData = -33008;

    /// <summary>
    ///     View not found.
    ///     The requested Revit view does not exist.
    /// </summary>
    public const int ViewNotFound = -33009;

    #endregion

    #region Plugin Specific Error Codes (-33100 to -33199)

    /// <summary>
    ///     Command registration failed.
    ///     Unable to register a new command.
    /// </summary>
    public const int CommandRegistrationFailed = -33100;

    /// <summary>
    ///     Service startup failed.
    ///     Unable to start the Socket service.
    /// </summary>
    public const int ServiceStartupFailed = -33101;

    /// <summary>
    ///     Unable to create external event.
    ///     Failed to create Revit external event.
    /// </summary>
    public const int ExternalEventCreationFailed = -33102;

    /// <summary>
    ///     External event execution failed.
    ///     Revit external event execution failed.
    /// </summary>
    public const int ExternalEventExecutionFailed = -33103;

    /// <summary>
    ///     Command cancelled.
    ///     The command was cancelled by the user or system.
    /// </summary>
    public const int CommandCancelled = -33104;

    /// <summary>
    ///     Command parameter parsing failed.
    ///     Unable to parse or convert command parameters.
    /// </summary>
    public const int CommandParameterParsingFailed = -33105;

    #endregion

    #region General Application Error Codes (-33200 to -33299)

    /// <summary>
    ///     Unauthorized access.
    ///     The client does not have permission to perform the requested operation.
    /// </summary>
    public const int Unauthorized = -33200;

    /// <summary>
    ///     Resource unavailable.
    ///     The requested resource is unavailable or does not exist.
    /// </summary>
    public const int ResourceUnavailable = -33201;

    /// <summary>
    ///     Request timeout.
    ///     Request processing timed out.
    /// </summary>
    public const int RequestTimeout = -33202;

    /// <summary>
    ///     Invalid session.
    ///     The session ID is invalid or has expired.
    /// </summary>
    public const int InvalidSession = -33203;

    /// <summary>
    ///     Configuration error.
    ///     Plugin configuration error.
    /// </summary>
    public const int ConfigurationError = -33204;

    /// <summary>
    ///     IO error.
    ///     File read/write or network IO error.
    /// </summary>
    public const int IOError = -33205;

    #endregion
}