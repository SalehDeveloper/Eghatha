using Eghatha.Api.Mappers;
using Eghatha.Application.Features.Authentication.Commands.Login;
using Eghatha.Application.Features.Authentication.Commands.RefreshToken;
using Eghatha.Application.Features.Authentication.Queries.GetLoggedInUser;
using Eghatha.Contract.Identity.Requests;
using Eghatha.Contract.Identity.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class IdentityController : ApiController
    {
        public IdentityController(ISender sender) : base(sender)
        {
     
        }

        [HttpPost(ApiEndpoints.Identity.Login)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Authenticate user and generate access , refresh tokens.")]
        [EndpointDescription("Validates the user's credentials (email and password) and returns an authenticated user response with a valid session if successful.")]
        [EndpointName("Login")]
        public async Task<IActionResult> Login (LoginRequest request , CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);    

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.Ok(v.ToUserResponse()),
                Problem);

        }


        [HttpPost(ApiEndpoints.Identity.RefreshToken)]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Rotates authentication tokens using refresh token.")]
        [EndpointDescription("Validates the refresh token stored in an HTTP-only cookie and issues a new access token and refresh token pair. The previous refresh token is revoked to prevent reuse.")]
        [EndpointName("RefreshToken")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand();

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.NoContent(),
                Problem);
        }

        [Authorize]
        [HttpGet(ApiEndpoints.Identity.Me)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(MeResponse) , StatusCodes.Status200OK)]
        [EndpointSummary("Gets the current authenticated user profile.")]
        [EndpointDescription("Returns the profile information of the currently authenticated user")]
        public async Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            var query = new GetLoggedinUserQuery();

            var result = await _sender.Send(query, cancellationToken);



            return result.Match(
                v=> base.Ok( v.ToMeResponse()),
                 Problem);

        }








    }
}
