using Eghatha.Application.Features.AiAssistant.Queries.AskSystemQuestion;
using Eghatha.Contract.AiAssistant.Requests;
using Eghatha.Contract.AiAssistant.Responses;
using Eghatha.Contract.Shared;
using Eghatha.Contract.Teams.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Eghatha.Api.Controllers
{
    public class AiAssistantController : ApiController
    {
        public AiAssistantController(ISender sender) : base(sender)
        {
        }

        // [Authorize(Roles = ApplicationRole.Admin)]

        [HttpPost(ApiEndpoints.AiAssistant.Ask)]
        [ProducesResponseType(typeof(AskSystemQuestionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Ask([FromBody] AskSystemQuestionRequest request, CancellationToken ct)
        {
            var command = new AskSystemQuestionQuery(request.Question);
            var result = await _sender.Send(command, ct);
            return Ok(new AskSystemQuestionResponse(result.Answer));
        }
    }
}
