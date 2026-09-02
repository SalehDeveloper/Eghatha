using Eghatha.Api.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Eghatha.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected ISender _sender;

        public ApiController(ISender sender)
        {
            _sender = sender;
        }
        private IDomainErrorLocalizer? _errorLocalizer;
        protected IDomainErrorLocalizer ErrorLocalizer =>
            _errorLocalizer ??= HttpContext.RequestServices.GetRequiredService<IDomainErrorLocalizer>();
        protected IActionResult ValidationProblem(List<Error> errors)
        {

            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                      ErrorLocalizer.Localize(error));

            }

            return ValidationProblem(modelStateDictionary);

        }

        protected IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, detail: ErrorLocalizer.Localize(error));
        }

        protected IActionResult Problem(List<Error> errors)
        {
            if (errors is null || errors.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if (errors.All(e => e.Type == ErrorType.Validation))
                return ValidationProblem(errors);

            return Problem(errors[0]);


        }
    }
}
