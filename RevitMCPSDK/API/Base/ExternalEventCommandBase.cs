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

using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using RevitMCPSDK.API.Interfaces;
using RevitMCPSDK.API.Models.JsonRPC;
using RevitMCPSDK.Exceptions;

namespace RevitMCPSDK.API.Base;

public abstract class ExternalEventCommandBase(IWaitableExternalEventHandler handler, UIApplication uiApp)
    : IRevitCommand
{
    protected ExternalEvent Event { get; } = ExternalEvent.Create(handler);

    protected IWaitableExternalEventHandler Handler { get; } =
        handler ?? throw new ArgumentNullException(nameof(handler));

    protected UIApplication UiApp { get; private set; } = uiApp ?? throw new ArgumentNullException(nameof(uiApp));

    public abstract string CommandName { get; }

    public abstract object Execute(JObject parameters, string requestId);

    protected bool RaiseAndWaitForCompletion(int timeoutMs = 10000)
    {
        Event.Raise();
        return Handler.WaitForCompletion(timeoutMs);
    }

    protected CommandExecutionException CreateTimeoutException(string commandName)
    {
        return new CommandExecutionException(
            $"Command {commandName} execution timed out",
            JsonRPCErrorCodes.CommandExecutionTimeout);
    }
}