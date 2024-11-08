using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace FlightAPI.Filters
{
	public class ExceptionFilter : IExceptionFilter
	{
		private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

		public void OnException(ExceptionContext context)
		{
			Logger.Error(context.Exception, "An exception occurred.");
			context.Result = new ObjectResult(new
			{
				Message = "An error occurred while processing your request.",
				Detail = context.Exception.Message
			})
			{
				StatusCode = 500
			};
		}
	}
}

