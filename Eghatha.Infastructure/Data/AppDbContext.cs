using Eghatha.Domain.Abstractions;
using Eghatha.Infastructure.Outbox;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;



namespace Eghatha.Infastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {

        private static readonly JsonSerializerSettings jsonSerializerSettings = new()
        {
           TypeNameHandling = TypeNameHandling.All
        };

        private readonly IMediator _mediator;
        private readonly TimeProvider _timeProvider;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IMediator mediator,
            TimeProvider timeProvider)
            : base(options)
        {
            _mediator = mediator;
            _timeProvider = timeProvider;
        }



        public async Task CompleteAsync(CancellationToken cancellationToken)
        {

            AddDomainEventsAsOutboxMessages();
            
            await base.SaveChangesAsync(cancellationToken);
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

     

        private void AddDomainEventsAsOutboxMessages()
        {
            var domainEntities = ChangeTracker
              .Entries<Entity>()
              .Where(x => x.Entity.DomainEvents.Any())
              .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            foreach (var entity in domainEntities)
            {
                entity.Entity.ClearDomainEvents();
            }

            var outboxMessages = domainEvents
              .Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(),
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent , jsonSerializerSettings ),
                    _timeProvider.GetUtcNow().UtcDateTime))
                    .ToList();
           
            
            Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}
