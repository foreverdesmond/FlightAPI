using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace FlightAPI.Filters
{
	public class LogFilter : IActionFilter
	{
		private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

		public void OnActionExecuting(ActionExecutingContext context)
		{
			Logger.Info($"Executing action: {context.ActionDescriptor.DisplayName}");
			Logger.Info($"Request data: {context.ActionArguments}");
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Result is ObjectResult result)
			{
				Logger.Info($"Response data: {result.Value}");
			}
			else
			{
				Logger.Info($"Response status: {context.Result?.GetType().Name}");
			}
		}
	}
}

