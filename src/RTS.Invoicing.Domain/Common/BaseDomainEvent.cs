using RTS.Invoicing.Domain.Contracts.Events;
using System;

namespace RTS.Invoicing.Domain.Common
{
    public abstract record BaseDomainEvent : IDomainEvent
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDomainEvent"/> class.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of <see cref="BaseDomainEvent"/> with the specified identifier and sets <see cref="OccurredOn"/> to the current UTC time.
        /// </summary>
        /// <param name="id">The unique identifier for the domain event.</param>
        protected BaseDomainEvent(Guid id)
        {
            Id = id;
            OccurredOn = DateTime.UtcNow;
        }
    }
}