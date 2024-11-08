using FlightAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FlightAPI.Tests.Filters
{
	public class ValidateFlightFilterAttributeTests
	{
		[Fact]
		public void OnActionExecuting_ReturnsBadRequest_WhenModelStateIsInvalid()
		{
			// Arrange
			var filter = new ValidateFlightFilterAttribute();
			var modelState = new ModelStateDictionary();
			modelState.AddModelError("FlightNumber", "Flight number is required.");

			var actionContext = new ActionContext(
				new DefaultHttpContext(),
				new Microsoft.AspNetCore.Routing.RouteData(),
				new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
				modelState
			);

			var actionExecutingContext = new ActionExecutingContext(
				actionContext,
				new List<IFilterMetadata>(),
				new Dictionary<string, object>(),
				new Mock<Controller>().Object
			);

			// Act
			filter.OnActionExecuting(actionExecutingContext);

			// Assert
			var result = Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
			var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
			Assert.Equal("Bad Request", problemDetails.Title);
			Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
			Assert.Contains("FlightNumber", problemDetails.Extensions.Keys);
		}

		[Fact]
		public void OnActionExecuting_DoesNotReturnBadRequest_WhenModelStateIsValid()
		{
			// Arrange
			var filter = new ValidateFlightFilterAttribute();
			var modelState = new ModelStateDictionary();

			var actionContext = new ActionContext(
				new DefaultHttpContext(),
				new Microsoft.AspNetCore.Routing.RouteData(),
				new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
				modelState
			);

			var actionExecutingContext = new ActionExecutingContext(
				actionContext,
				new List<IFilterMetadata>(),
				new Dictionary<string, object>(),
				new Mock<Controller>().Object
			);

			// Act
			filter.OnActionExecuting(actionExecutingContext);

			// Assert
			Assert.Null(actionExecutingContext.Result);
		}
	}
}

