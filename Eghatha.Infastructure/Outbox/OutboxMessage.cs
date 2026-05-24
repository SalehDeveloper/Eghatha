using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Outbox
{
    public sealed class OutboxMessage
    {
        public OutboxMessage(Guid id, string type, string content, DateTime occurredOnUtc)
        {
            Id = id;
            Type = type;
            Content = content;
            OccurredOnUtc = occurredOnUtc;
        }

        public Guid Id { get; init; }

        public string Type { get; init; }

        public string Content { get; init; }

        public DateTime OccurredOnUtc { get; init; }

        public DateTime? ProcessedOnUtc { get; set; }

        public string? Error { get; set; }
    }
}
