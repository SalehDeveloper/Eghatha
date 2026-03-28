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

        protected IActionResult ValidationProblem(List<Error> errors)
        {

            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);

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

            return Problem(statusCode: statusCode, detail: error.Description);
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
