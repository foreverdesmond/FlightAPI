using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlightAPI.Filters
{
	public class ValidateFlightFilterAttribute : ActionFilterAttribute
	{
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Bad Request",
                    Status = (int)StatusCodes.Status400BadRequest,
                    Detail = "Invalid request parameters.",
                    Instance = context.HttpContext.Request.Path
                };
                
                foreach (var key in context.ModelState.Keys)
                {
                    var errors = context.ModelState[key].Errors.Select(e => e.ErrorMessage).ToArray();
                    if (errors.Length > 0)
                    {
                        problemDetails.Extensions["invalidParams:" + key] = errors;
                    }
                }
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}

