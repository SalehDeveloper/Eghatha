using Eghatha.Application.Common.Interfaces;
using Eghatha.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Data.Interceptors
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly IUser _user;
        private readonly TimeProvider _dateTime;

        public AuditableEntityInterceptor(IUser user, TimeProvider dateTime)
        {
            _user = user;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var userId = _user.Id?.ToString() ?? "system";
            var utcNow = _dateTime.GetUtcNow();

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified
                    || entry.HasChangedOwnedEntities())
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedAt = utcNow;
                    }

                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedUtc = utcNow;

                    // Handle owned entities
                    foreach (var ownedEntry in entry.References)
                    {
                        if (ownedEntry.TargetEntry is { Entity: AuditableEntity ownedEntity } target &&
                            target.State is EntityState.Added or EntityState.Modified)
                        {
                            if (target.State == EntityState.Added)
                            {
                                ownedEntity.CreatedBy = userId;
                                ownedEntity.CreatedAt = utcNow;
                            }

                            ownedEntity.LastModifiedBy = userId;
                            ownedEntity.LastModifiedUtc = utcNow;
                        }
                    }
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry?.Metadata.IsOwned() == true &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
