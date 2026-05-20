using Eghatha.Domain.Disasters;
using MediatR;

namespace Eghatha.Application.Features.Disasters.Queries.GetDisasterTypes
{
    public class GetDisasterTypesQueryHandler : IRequestHandler<GetDisasterTypesQuery, IReadOnlyList<DisasterTypeResponse>>
    {
        public Task<IReadOnlyList<DisasterTypeResponse>> Handle(GetDisasterTypesQuery request, CancellationToken cancellationToken)
        {

            var result = DisasterType.List.Select(x => new DisasterTypeResponse(x.Value , x.Name)).ToList();
             

            return Task.FromResult<IReadOnlyList<DisasterTypeResponse>>(result);
        }
    }


}
