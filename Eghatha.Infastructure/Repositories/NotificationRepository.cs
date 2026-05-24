using Eghatha.Application.Common.Interfaces;
using Eghatha.Application.Common.Models;
using Eghatha.Application.Features.Notifications.Dtos;
using Eghatha.Domain.Notifications;
using Eghatha.Infastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eghatha.Infastructure.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<NotificationDto>> GetNotificationsByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
        {


            var query =
              from n in _context.Set<Notification>()
              join r in _context.Set<NotificationRecipient>()
                  on n.Id equals r.NotificationId
              where r.UserId == userId
              orderby n.CreatedAt descending
              select new NotificationDto
              {
                  Id = n.Id,
                  Title = n.Title,
                  Message = n.Message,
                  Url = n.Url,
                  Type = n.Type,
                  IsRead = r.IsRead,
                  CreatedAt = n.CreatedAt
              };

            return new PaginatedList<NotificationDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(cancellationToken),
                TotalPages = (int)Math.Ceiling(await query.CountAsync(cancellationToken) / (double)pageSize),
                Items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken)

            };
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(Guid notificationId , Guid userId , CancellationToken cancellationToken )
        {
            var notification = await
           (from n in _context.Set<Notification>()
            join r in _context.Set<NotificationRecipient>()
                on n.Id equals r.NotificationId
            where n.Id == notificationId
                  && r.UserId == userId
            select new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Url = n.Url,
                Type = n.Type,
                IsRead = r.IsRead,
                CreatedAt = n.CreatedAt
            })
           .FirstOrDefaultAsync(cancellationToken);

            return notification;
        }
        public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken)
        {


            return await _context.Set<NotificationRecipient>()
                .CountAsync(r => r.UserId == userId && !r.IsRead, cancellationToken);


        }

        public async Task<NotificationRecipient> GetNotificationRecipientAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<NotificationRecipient>()
                .FirstOrDefaultAsync(r => r.NotificationId == notificationId && r.UserId == userId, cancellationToken);
        }

        public async Task<List<NotificationRecipient>> GetNotificationRecipientsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<NotificationRecipient>()
                .Where(r => r.UserId == userId && !r.IsRead)
                .ToListAsync(cancellationToken);
        }
    }
}