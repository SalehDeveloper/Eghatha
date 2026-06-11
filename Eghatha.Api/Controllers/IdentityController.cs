using Eghatha.Api.Mappers;
using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Features.Authentication.Commands.ConfirmEmail;
using Eghatha.Application.Features.Authentication.Commands.Login;
using Eghatha.Application.Features.Authentication.Commands.Logout;
using Eghatha.Application.Features.Authentication.Commands.RefreshToken;
using Eghatha.Application.Features.Authentication.Commands.RequestPasswordReset;
using Eghatha.Application.Features.Authentication.Commands.ResendEmailConfirmationCode;
using Eghatha.Application.Features.Authentication.Commands.ResetPassword;
using Eghatha.Application.Features.Authentication.Dtos;
using Eghatha.Application.Features.Authentication.Queries.GetLoggedInUser;
using Eghatha.Contract.Identity.Requests;
using Eghatha.Contract.Identity.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eghatha.Api.Controllers
{
    public class IdentityController : ApiController
    {
        private readonly IUser user;

        public IdentityController(ISender sender, IUser user) : base(sender)
        {
            this.user = user;
        }

        [HttpPost(ApiEndpoints.Identity.Login)]
        [ProducesResponseType(typeof(Contract.Identity.Responses.TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Authenticate user and generate access , refresh tokens.")]
        [EndpointDescription("Validates the user's credentials (email and password) and returns an authenticated user response with a valid tokens if successful.")]
        [EndpointName("Login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.Ok(v.ToResponse()),
                Problem);

        }


        [HttpPost(ApiEndpoints.Identity.RefreshToken)]
        [ProducesResponseType( typeof(Contract.Identity.Responses.TokenResponse)  , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Refreshes access token using a valid refresh token.")]
        [EndpointDescription("Exchanges an expired access token and a valid refresh token for a new token pair.")]
        [EndpointName("RefreshToken")]
        public async Task<IActionResult> RefreshToken( [FromBody] RefreshTokenRequest request , CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(request.RefreshToken , request.ExpiredAccessToken);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                v => base.Ok(v.ToResponse()),
                Problem);
        }

        [Authorize]
        [HttpGet(ApiEndpoints.Identity.Me)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(MeResponse), StatusCodes.Status200OK)]
        [EndpointSummary("Gets the current authenticated user profile.")]
        [EndpointDescription("Returns the profile information of the currently authenticated user")]
        [EndpointName("Me")]
        public async Task<IActionResult> Me(CancellationToken cancellationToken)
        {
            var userId = user.Id;

          


            var query = new GetLoggedinUserQuery(userId.Value);

            var result = await _sender.Send(query, cancellationToken);



            return result.Match(
                v => base.Ok(v.ToUserResponse()),
                 Problem);

        }

        [HttpPost(ApiEndpoints.Identity.RequestPasswordReset)]
        [ProducesResponseType(typeof(RequestPasswordResetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Initiates a password reset request.")]
        [EndpointDescription("Generates and sends a password reset code to the user's email to allow resetting their password.")]
        [EndpointName("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswrodReset([FromBody] RequestPasswordResetRequest request, CancellationToken cancellationToken)
        {
            var command = new RequestPasswordResetCommand(request.Email);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                 v => base.Ok(new RequestPasswordResetResponse(result.Value)),
                 Problem);
        }



        [HttpPost(ApiEndpoints.Identity.ResetPassword)]
        [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Resets the user's password.")]
        [EndpointDescription("Validates the provided reset code and updates the user's password to the new value.")]
        [EndpointName("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var command = new ResetPasswordCommand(request.Email, request.Code, request.NewPassword);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                 v => base.Ok(new ResetPasswordResponse(result.Value)),
                 Problem);
        }

        //[Authorize]
        [HttpPost(ApiEndpoints.Identity.Logout)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Logs out the current user.")]
        [EndpointDescription("Invalidates the current user's authentication token, effectively logging them out of the system.")]
        [EndpointName("Logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var command = new LogoutCommand();
            var result = await _sender.Send(command, cancellationToken);
            return result.Match(
                 v => base.NoContent(),
                 Problem);








        }


        [HttpPost(ApiEndpoints.Identity.ConfirmEmail)]
        [ProducesResponseType(typeof(ConfirmEmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Confirms user email using OTP code.")]
        [EndpointDescription("Validates the provided OTP code sent to the user's email and confirm the user account if the code is correct.")]
        [EndpointName("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var command = new ConfirmEmailCommand(request.Email, request.Otp);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                 v => base.Ok(new ConfirmEmailResponse(result.Value)),
                 Problem);
        }


        [HttpPost(ApiEndpoints.Identity.ResendEmailCode)]
        [ProducesResponseType(typeof(ResendEmailCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Resends email confirmation OTP code.")]
        [EndpointDescription("Generates and sends a new OTP code to the user's email to allow email verification or re-verification.")]
        [EndpointName("ResendEmailCode")]
        public async Task<IActionResult> ResendEmailCode([FromBody] ResendEmailCodeRequest request, CancellationToken cancellationToken)
        {
            var command = new ResendEmailConfirmationCodeCommand(request.Email);

            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                   v => base.Ok(new ResendEmailCodeResponse(result.Value)),
                   Problem);
        }

    }
}