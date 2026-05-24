using Eghatha.Domain.Abstractions;
using Eghatha.Infastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Outbox
{
    public sealed class OutboxProcessor : BackgroundService
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeProvider _timeProvider;
        private readonly OutboxOptions _outboxOptions;
        private readonly ILogger<OutboxProcessor> _logger;


        public OutboxProcessor(IServiceScopeFactory scopeFactory, TimeProvider timeProvider, IOptions<OutboxOptions> outboxOptions, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _timeProvider = timeProvider;
            _outboxOptions = outboxOptions.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_outboxOptions.IntervalInSeconds));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();


                var dbContext = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var mediator = scope.ServiceProvider
                    .GetRequiredService<IMediator>();

                var messages = await dbContext.Set<OutboxMessage>()
                    .Where(x => x.ProcessedOnUtc == null)
                    .OrderBy(x => x.OccurredOnUtc)
                    .Take(_outboxOptions.BatchSize)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        

                        var domainEvent = JsonConvert.DeserializeObject<DomainEvent>(
                            message.Content,
                            jsonSerializerSettings);

                      
                            await mediator.Publish(
                                domainEvent,
                                stoppingToken);
                        

                        message.ProcessedOnUtc =
                            _timeProvider.GetUtcNow().UtcDateTime;
                    }
                    catch (Exception ex)
                    {
                        message.Error = ex.ToString();
                        _logger.LogError(ex, $"Exception while processing outbox message {message.Id}", message.Id);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);

            }
           

          
        }
    }
}
