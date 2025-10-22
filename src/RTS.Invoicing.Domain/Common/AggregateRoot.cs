using RTS.Invoicing.Domain.Contracts.Events;
using System.Collections.Generic;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Base class for aggregate roots. An aggregate root is a specific type of entity that acts as the entry point to an aggregate.
    /// </summary>
    public abstract class AggregateRoot<TId> : AuditableEntity<TId> where TId : notnull
    {
        /// <summary>
        /// The domain events holder.
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        protected AggregateRoot(TId id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
        /// </summary>
        protected AggregateRoot()
            : base()
        {
        }

        /// <summary>
        /// Gets the domain events collection.
        /// </summary>
        /// <value>The domain events collection.</value>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adds the domain event into the domain event collection.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        /// <summary>
        /// Clears the domain events.
        /// </summary>
        protected void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
