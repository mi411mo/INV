using System;

namespace RTS.Invoicing.Domain.Contracts.Events
{
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets or sets the domain event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid Id { set; get; }

        /// <summary>
        /// Gets when the event occurred on.
        /// </summary>
        /// <value>When the event occurred.</value>
        public DateTime OccurredOn { get; init; }
    }
}
