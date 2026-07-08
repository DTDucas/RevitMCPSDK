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

using System.IO;
using System.Text;
using RevitMCPSDK.API.Interfaces;

namespace RevitMCPSDK.API.Helpers;

/// <summary>
///     Default implementation of the ILogger interface
/// </summary>
public class LoggingHelper : ILogger
{
    private readonly bool _consoleOutput;
    private readonly object _lockObj = new();
    private readonly string _logFilePath;
    private readonly LogLevel _minimumLogLevel;

    /// <summary>
    ///     Creates a new logger with the specified settings
    /// </summary>
    /// <param name="logFilePath">Path to the log file</param>
    /// <param name="minimumLogLevel">Minimum log level to record</param>
    /// <param name="consoleOutput">Whether to also output to console</param>
    public LoggingHelper(string logFilePath, LogLevel minimumLogLevel = LogLevel.Info, bool consoleOutput = false)
    {
        _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
        _minimumLogLevel = minimumLogLevel;
        _consoleOutput = consoleOutput;

        var directory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
    }

    /// <summary>
    ///     Logs a message with the specified level
    /// </summary>
    public void Log(LogLevel level, string message, params object[] args)
    {
        if (level < _minimumLogLevel)
            return;

        var formattedMessage = FormatLogMessage(level, message, args);

        lock (_lockObj)
        {
            try
            {
                File.AppendAllText(_logFilePath, formattedMessage + Environment.NewLine);

                if (_consoleOutput) Console.WriteLine(formattedMessage);
            }
            catch (Exception ex)
            {
                if (_consoleOutput)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                    Console.WriteLine(formattedMessage);
                }
            }
        }
    }

    /// <summary>
    ///     Logs a debug message
    /// </summary>
    public void Debug(string message, params object[] args)
    {
        Log(LogLevel.Debug, message, args);
    }

    /// <summary>
    ///     Logs an info message
    /// </summary>
    public void Info(string message, params object[] args)
    {
        Log(LogLevel.Info, message, args);
    }

    /// <summary>
    ///     Logs a warning message
    /// </summary>
    public void Warning(string message, params object[] args)
    {
        Log(LogLevel.Warning, message, args);
    }

    /// <summary>
    ///     Logs an error message
    /// </summary>
    public void Error(string message, params object[] args)
    {
        Log(LogLevel.Error, message, args);
    }

    /// <summary>
    ///     Formats a log message
    /// </summary>
    private string FormatLogMessage(LogLevel level, string message, params object[] args)
    {
        var sb = new StringBuilder();

        // Add timestamp
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        sb.Append(" [");

        switch (level)
        {
            case LogLevel.Debug:
                sb.Append("DEBUG");
                break;
            case LogLevel.Info:
                sb.Append("INFO ");
                break;
            case LogLevel.Warning:
                sb.Append("WARN ");
                break;
            case LogLevel.Error:
                sb.Append("ERROR");
                break;
            case LogLevel.Fatal:
                sb.Append("FATAL");
                break;
        }

        sb.Append("] ");

        try
        {
            sb.Append(args.Length > 0 ? string.Format(message, args) : message);
        }
        catch (FormatException)
        {
            sb.Append(message);
            sb.Append(" [FORMAT ERROR WITH ARGS: ");
            for (var i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(args[i]?.ToString() ?? "null");
            }

            sb.Append("]");
        }

        return sb.ToString();
    }
}