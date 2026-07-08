//// 
////                       RevitAPI-Solutions
//// Copyright (c) Duong Tran Quang (DTDucas) (contact@dtducas.com / baymax.contact@gmail.com)
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//// SOFTWARE.
////

//using Autodesk.Revit.DB;
//using RevitMCPSDK.API.Models.JsonRPC;
//using RevitMCPSDK.Exceptions;

//namespace RevitMCPSDK.API.Extensions;

///// <summary>
/////     Extension methods for Revit Transaction and SubTransaction operations
///// </summary>
//public static class TransactionExtensions
//{
//    /// <summary>
//    ///     Executes an action within a transaction
//    /// </summary>
//    /// <param name="doc">The document</param>
//    /// <param name="transactionName">The transaction name</param>
//    /// <param name="action">The action to execute</param>
//    /// <returns>The transaction result</returns>
//    public static TransactionStatus ExecuteTransaction(this Document doc, string transactionName, Action action)
//    {
//        if (doc == null)
//            throw new ArgumentNullException(nameof(doc));
//        if (action == null)
//            throw new ArgumentNullException(nameof(action));

//        using var transaction = new Transaction(doc, transactionName);
//        try
//        {
//            var status = transaction.Start();
//            if (status != TransactionStatus.Started) return status;

//            action();
//            return transaction.Commit();
//        }
//        catch (Exception ex)
//        {
//            if (transaction.HasStarted() && !transaction.HasEnded()) transaction.RollBack();
//            throw new CommandExecutionException($"Transaction failed: {ex.Message}",
//                JsonRPCErrorCodes.TransactionFailed, ex);
//        }
//    }

//    /// <summary>
//    ///     Executes a function within a transaction and returns the result
//    /// </summary>
//    /// <typeparam name="T">The result type</typeparam>
//    /// <param name="doc">The document</param>
//    /// <param name="transactionName">The transaction name</param>
//    /// <param name="function">The function to execute</param>
//    /// <returns>The result of the function</returns>
//    public static T ExecuteTransaction<T>(this Document doc, string transactionName, Func<T> function)
//    {
//        if (doc == null)
//            throw new ArgumentNullException(nameof(doc));
//        if (function == null)
//            throw new ArgumentNullException(nameof(function));

//        using var transaction = new Transaction(doc, transactionName);
//        try
//        {
//            var status = transaction.Start();
//            if (status != TransactionStatus.Started)
//                throw new CommandExecutionException($"Failed to start transaction. Status: {status}",
//                    JsonRPCErrorCodes.TransactionFailed);

//            var result = function();
//            transaction.Commit();
//            return result;
//        }
//        catch (Exception ex)
//        {
//            if (transaction.HasStarted() && !transaction.HasEnded()) transaction.RollBack();
//            throw new CommandExecutionException($"Transaction failed: {ex.Message}",
//                JsonRPCErrorCodes.TransactionFailed, ex);
//        }
//    }

//    /// <summary>
//    ///     Executes an action within a transaction group
//    /// </summary>
//    /// <param name="doc">The document</param>
//    /// <param name="groupName">The transaction group name</param>
//    /// <param name="action">The action to execute</param>
//    /// <returns>The transaction status</returns>
//    public static TransactionStatus ExecuteTransactionGroup(this Document doc, string groupName, Action action)
//    {
//        if (doc == null)
//            throw new ArgumentNullException(nameof(doc));
//        if (action == null)
//            throw new ArgumentNullException(nameof(action));

//        using var group = new TransactionGroup(doc, groupName);
//        try
//        {
//            var status = group.Start();
//            if (status != TransactionStatus.Started) return status;

//            action();
//            return group.Assimilate();
//        }
//        catch (Exception ex)
//        {
//            if (group.HasStarted() && !group.HasEnded()) group.RollBack();
//            throw new CommandExecutionException($"Transaction group failed: {ex.Message}",
//                JsonRPCErrorCodes.TransactionFailed, ex);
//        }
//    }

//    /// <summary>
//    ///     Checks if a transaction has started but not ended
//    /// </summary>
//    /// <param name="transaction">The transaction</param>
//    /// <returns>True if the transaction is active</returns>
//    public static bool IsActive(this Transaction transaction)
//    {
//        return transaction != null && transaction.HasStarted() && !transaction.HasEnded();
//    }
//}