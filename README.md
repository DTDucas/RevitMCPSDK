# RevitMCPSDK

## Overview

RevitMCPSDK is a comprehensive software development kit for Autodesk Revit that implements the Model Context Protocol (MCP). This SDK simplifies the development of Revit plugins by providing a robust framework for communication between Revit and external applications through a JSON-RPC 2.0 interface.

## Key Features

- **Multi-Version Support**: Compatible with Revit 2020-2026
- **JSON-RPC 2.0 Implementation**: Standardized communication protocol
- **MVVM Architecture**: Clean separation of Model-View-ViewModel for WPF applications
- **SOLID Principles**: Follows best practices in software design
- **Command Pattern**: Simplified command execution with error handling
- **External Event Framework**: Thread-safe execution of Revit API operations
- **Versioning Support**: Compatibility management between different Revit versions
- **Comprehensive Error Handling**: Standardized error codes and reporting

## Installation

### NuGet Package

The RevitMCPSDK is available as a NuGet package for each supported Revit version:

```
Install-Package RevitMCPSDK -Version 2020.0.0.1 // For Revit 2020
Install-Package RevitMCPSDK -Version 2021.0.0.1 // For Revit 2021
Install-Package RevitMCPSDK -Version 2022.0.0.1 // For Revit 2022
Install-Package RevitMCPSDK -Version 2023.0.0.1 // For Revit 2023
Install-Package RevitMCPSDK -Version 2024.0.0.1 // For Revit 2024
Install-Package RevitMCPSDK -Version 2025.0.0.1 // For Revit 2025
Install-Package RevitMCPSDK -Version 2026.0.0.1 // For Revit 2026
```
Or, if you prefer using PackageReference in your .csproj file:

```xml
<PackageReference Include="RevitMCPSDK" Version="$(RevitVersion).*" />
```

### Manual Installation

Alternatively, you can download the appropriate version of the SDK from the [GitHub releases page](https://github.com/DTDucas/RevitMCPSDK/releases) and reference it in your project.

## Architecture

RevitMCPSDK is built on the following architectural foundations:

### MVVM Pattern

The SDK follows the Model-View-ViewModel pattern to facilitate:

- Clean separation of concerns
- Improved testability
- Better code maintainability
- UI/UX flexibility

### SOLID Principles

- **S**ingle Responsibility: Each class has one responsibility
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Derived classes can be substituted for base classes
- **I**nterface Segregation: Focused interfaces for specific purposes
- **D**ependency Inversion: Abstractions over implementations

### JSON-RPC 2.0

The SDK implements the JSON-RPC 2.0 specification for communication between Revit and external applications:

- Standardized request/response format
- Support for notifications
- Comprehensive error handling
- Versioned protocol

## Core Components

### Command System

The command system allows execution of Revit API operations:

```csharp
// Register a command
commandRegistry.RegisterCommand(new YourCustomCommand());

// Execute a command
JObject parameters = JObject.FromObject(new { /* your parameters */ });
object result = revitCommand.Execute(parameters, "requestId");
```

### External Events

Thread-safe execution of Revit API operations:

```csharp
public class YourExternalEvent : ExternalEventCommandBase
{
    public YourExternalEvent(IWaitableExternalEventHandler handler, UIApplication uiApp)
        : base(handler, uiApp)
    {
    }

    public override string CommandName => "YourCommandName";

    public override object Execute(JObject parameters, string requestId)
    {
        // Extract parameters
        if (!parameters.TryGetValue("parameter1", out JToken param1Value))
            throw new CommandExecutionException("Missing required parameter: parameter1");

        // Raise the external event and wait for completion
        if (!RaiseAndWaitForCompletion())
            throw CreateTimeoutException(CommandName);

        // Return success result
        return CommandResult.CreateSuccess(new { /* result data */ });
    }
}
```

### Versioning Support

The SDK includes utilities for managing compatibility across different Revit versions:

```csharp
// Check if the current Revit version is supported
RevitVersionAdapter adapter = new RevitVersionAdapter(application);
bool isSupported = adapter.IsVersionSupported(new[] { "2020", "2021", "2022", "2023", "2024", "2025", "2026" });

// Compare versions
int result = VersionHelper.CompareVersions("2020", "2025"); // Returns -1 (2020 < 2025)
```

## Getting Started

### Creating a Basic Revit Plugin

1. Create a new Class Library project in Visual Studio
2. Install the RevitMCPSDK NuGet package for your target Revit version
3. Implement a custom command:

```csharp
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using RevitMCPSDK.API.Base;
using RevitMCPSDK.API.Interfaces;
using RevitMCPSDK.API.Models;

namespace YourNamespace
{
    public class GetElementCommand : ExternalEventCommandBase
    {
        public GetElementCommand(IWaitableExternalEventHandler handler, UIApplication uiApp)
            : base(handler, uiApp)
        {
        }

        public override string CommandName => "GetElement";

        public override object Execute(JObject parameters, string requestId)
        {
            // Extract element ID from parameters
            if (!parameters.TryGetValue("elementId", out JToken elementIdToken))
                return CommandResult.CreateError("Missing elementId parameter");

            int elementId = elementIdToken.Value<int>();

            // Get the document
            var doc = UiApp.ActiveUIDocument.Document;
            
            // Find the element
            var element = doc.GetElement(new Autodesk.Revit.DB.ElementId(elementId));
            
            if (element == null)
                return CommandResult.CreateError($"Element with ID {elementId} not found", 
                    new { ErrorCode = JsonRPCErrorCodes.ElementNotFound });

            // Return element information
            return CommandResult.CreateSuccess(new
            {
                Id = element.Id.IntegerValue,
                Name = element.Name,
                Category = element.Category?.Name
            });
        }
    }
}
```

4. Register your command in your Revit external application:

```csharp
using Autodesk.Revit.UI;
using RevitMCPSDK.API.Interfaces;
using System;

namespace YourNamespace
{
    public class YourRevitApp : IExternalApplication
    {
        private ICommandRegistry _commandRegistry;

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Initialize the command registry
                _commandRegistry = new CommandRegistry();
                
                // Create and register your commands
                var handler = new YourExternalEventHandler();
                var uiApp = new UIApplication(application.ControlledApplication);
                
                var getElementCommand = new GetElementCommand(handler, uiApp);
                _commandRegistry.RegisterCommand(getElementCommand);
                
                // Initialize communication channel
                // ...
                
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Log the error
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // Cleanup resources
            return Result.Succeeded;
        }
    }
}
```

## Error Handling

The SDK provides standardized error codes and handling:

```csharp
try
{
    // Execute some Revit API operation
}
catch (Exception ex)
{
    return CommandResult.CreateError(
        $"Failed to execute operation: {ex.Message}",
        new { ErrorCode = JsonRPCErrorCodes.RevitApiError }
    );
}
```

## Best Practices

- **Keep Commands Focused**: Each command should do one thing well
- **Validate Parameters**: Always validate input parameters before execution
- **Handle Errors Gracefully**: Use the standardized error system
- **Consider Version Compatibility**: Use the versioning system to manage differences between Revit versions
- **Follow UI/UX Guidelines**: When building WPF interfaces, follow Revit UI/UX guidelines
- **Optimize Performance**: Keep commands lightweight and efficient
- **Use Transactions Properly**: Begin transactions only when necessary and commit them as soon as possible

## License

RevitMCPSDK is licensed under the MIT License. See the LICENSE file for details.

## Support

For support, please open an issue on the [GitHub repository](https://github.com/DTDucas/RevitMCPSDK/issues).

## Author

Duong Tran Quang - DTDucas

Copyright © 2025 Duong Tran Quang - DTDucas