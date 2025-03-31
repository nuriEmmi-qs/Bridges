using Microsoft.AspNetCore.Mvc.Filters;

namespace WhatsAppBridge.Filters;

public class LogExecutionFilter(ILogger<LogExecutionFilter> logger) : IActionFilter {
    
    private readonly ILogger<LogExecutionFilter> _logger = logger;

    // This method is called before the action executes
    public void OnActionExecuting(ActionExecutingContext context) {
        _logger.LogInformation($"Action started: {context.ActionDescriptor.DisplayName}");
    }

    // This method is called after the action executes
    public void OnActionExecuted(ActionExecutedContext context) {
        if (context.Exception == null) {
            _logger.LogInformation($"Action completed successfully: {context.ActionDescriptor.DisplayName}");
        }
        else {
            _logger.LogError($"Action failed: {context.ActionDescriptor.DisplayName}. Error: {context.Exception.Message}");
        }
    }
}
