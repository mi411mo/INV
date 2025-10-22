using System;

namespace RTS.Invoicing.Domain.Common
{
    public class AuditableEntity<TId> : Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Parameterless constructor intended for use by object-relational mappers (ORMs).
        /// </summary>
        /// <remarks>
        /// Exists to satisfy ORM materialization requirements; not intended for general construction.
        /// </remarks>
        protected AuditableEntity()
             : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new auditable entity with the specified identifier and sets its creation timestamp.
        /// </summary>
        /// <param name="id">The identifier for the entity.</param>
        protected AuditableEntity(TId id)
            : base(id)
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the date and time when this entity was first created.
        /// </summary>
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when this entity was last modified.
        /// </summary>
        public DateTime? ModifiedAt { get; private set; } = null;

        /// <summary>
        /// Marks the entity as modified by updating <see cref="ModifiedAt"/> to the current UTC time.
        /// </summary>
        public void EntityModified()
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}