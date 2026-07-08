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

using Autodesk.Revit.DB;
using RevitMCPSDK.API.Interfaces;

namespace RevitMCPSDK.API.Helpers;

/// <summary>
///     Handles Revit failures during transactions
/// </summary>
public class FailureHandler : IFailuresPreprocessor
{
    private readonly HashSet<FailureDefinitionId> _ignoredErrors;
    private readonly HashSet<FailureDefinitionId> _ignoredWarnings;
    private readonly ILogger _logger;
    private readonly bool _resolveErrors;
    private readonly bool _suppressWarnings;

    /// <summary>
    ///     Creates a new failure handler
    /// </summary>
    /// <param name="suppressWarnings">Whether to suppress all warnings</param>
    /// <param name="resolveErrors">Whether to resolve all errors</param>
    /// <param name="logger">Optional logger</param>
    public FailureHandler(bool suppressWarnings = true, bool resolveErrors = false, ILogger logger = null)
    {
        _suppressWarnings = suppressWarnings;
        _resolveErrors = resolveErrors;
        _logger = logger;
        _ignoredWarnings = new HashSet<FailureDefinitionId>();
        _ignoredErrors = new HashSet<FailureDefinitionId>();
        FailureMessages = new List<string>();
    }

    /// <summary>
    ///     Gets the number of errors encountered
    /// </summary>
    public int ErrorCount { get; private set; }

    /// <summary>
    ///     Gets the number of warnings encountered
    /// </summary>
    public int WarningCount { get; private set; }

    /// <summary>
    ///     Gets the list of failure messages encountered
    /// </summary>
    public List<string> FailureMessages { get; }

    /// <summary>
    ///     Implementation of IFailuresPreprocessor
    /// </summary>
    public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
    {
        if (failuresAccessor == null)
            return FailureProcessingResult.Continue;

        var failureMessages = failuresAccessor.GetFailureMessages();
        if (failureMessages.Count == 0)
            return FailureProcessingResult.Continue;

        var hasErrors = false;
        foreach (var failure in failureMessages)
        {
            var severity = failure.GetSeverity();
            var failureDefinitionId = failure.GetFailureDefinitionId();
            var failureMessage = failure.GetDescriptionText();

            _logger?.Debug($"Revit failure: {severity} - {failureMessage} (ID: {failureDefinitionId})");

            FailureMessages.Add($"{severity}: {failureMessage}");

            if (severity == FailureSeverity.Warning)
            {
                WarningCount++;

                if (_suppressWarnings || _ignoredWarnings.Contains(failureDefinitionId))
                    failuresAccessor.DeleteWarning(failure);
            }
            else if (severity == FailureSeverity.Error)
            {
                ErrorCount++;
                hasErrors = true;

                if (_resolveErrors || _ignoredErrors.Contains(failureDefinitionId))
                    failuresAccessor.ResolveFailure(failure);
            }
        }

        if (hasErrors && !_resolveErrors &&
            !failureMessages.All(f => _ignoredErrors.Contains(f.GetFailureDefinitionId())))
            return FailureProcessingResult.ProceedWithRollBack;

        return FailureProcessingResult.ProceedWithCommit;
    }

    /// <summary>
    ///     Add a specific warning to ignore
    /// </summary>
    /// <param name="failureDefinitionId">The failure definition ID</param>
    public void AddIgnoredWarning(FailureDefinitionId failureDefinitionId)
    {
        if (failureDefinitionId != null) _ignoredWarnings.Add(failureDefinitionId);
    }

    /// <summary>
    ///     Add a specific error to ignore and resolve automatically
    /// </summary>
    /// <param name="failureDefinitionId">The failure definition ID</param>
    public void AddIgnoredError(FailureDefinitionId failureDefinitionId)
    {
        if (failureDefinitionId != null) _ignoredErrors.Add(failureDefinitionId);
    }

    /// <summary>
    ///     Reset error and warning counts
    /// </summary>
    public void Reset()
    {
        ErrorCount = 0;
        WarningCount = 0;
        FailureMessages.Clear();
    }

    /// <summary>
    ///     Sets failure handling options for a transaction
    /// </summary>
    /// <param name="transaction">The transaction to configure</param>
    public void ConfigureTransaction(Transaction transaction)
    {
        if (transaction == null)
            return;

        var options = transaction.GetFailureHandlingOptions();
        options.SetFailuresPreprocessor(this);
        options.SetClearAfterRollback(true);
        transaction.SetFailureHandlingOptions(options);
    }

    /// <summary>
    ///     Executes an action within a transaction with this failure handler
    /// </summary>
    /// <param name="doc">The document</param>
    /// <param name="transactionName">The transaction name</param>
    /// <param name="action">The action to execute</param>
    /// <param name="errorCount"></param>
    /// <param name="warningCount"></param>
    /// <param name="messages"></param>
    /// <returns>Transaction status and failure information</returns>
    public TransactionStatus ExecuteTransaction(
        Document doc, string transactionName, Action action,
        out int errorCount, out int warningCount, out List<string> messages)
    {
        if (doc == null)
            throw new ArgumentNullException(nameof(doc));
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        // Reset counters
        Reset();
        var status = TransactionStatus.RolledBack;

        using (var transaction = new Transaction(doc, transactionName))
        {
            ConfigureTransaction(transaction);

            try
            {
                status = transaction.Start();
                if (status != TransactionStatus.Started)
                {
                    errorCount = ErrorCount;
                    warningCount = WarningCount;
                    messages = new List<string>(FailureMessages);
                    return status;
                }

                action();
                status = transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger?.Error($"Transaction failed: {ex.Message}");

                if (transaction.HasStarted() && !transaction.HasEnded()) transaction.RollBack();

                FailureMessages.Add($"Exception: {ex.Message}");
                status = TransactionStatus.RolledBack;
            }
        }

        errorCount = ErrorCount;
        warningCount = WarningCount;
        messages = new List<string>(FailureMessages);
        return status;
    }

    /// <summary>
    ///     Executes an action within a transaction with this failure handler
    /// </summary>
    /// <param name="doc">The document</param>
    /// <param name="transactionName">The transaction name</param>
    /// <param name="action">The action to execute</param>
    /// <returns>Transaction status</returns>
    public TransactionStatus ExecuteTransaction(Document doc, string transactionName, Action action)
    {
        return ExecuteTransaction(doc, transactionName, action, out _, out _, out _);
    }
}