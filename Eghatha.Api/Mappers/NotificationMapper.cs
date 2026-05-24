using Eghatha.Application.Features.Notifications.Dtos;
using Eghatha.Contract.Notifications.Responses;

namespace Eghatha.Api.Mappers
{
    public  static class NotificationMapper
    {
        public static NotificationResponse ToResponse(this NotificationDto dto)
        {
            return new NotificationResponse(dto.Id, dto.Title, dto.Message, dto.Url, dto.Type.ToString(), dto.IsRead, dto.CreatedAt);
        }
    }
}
