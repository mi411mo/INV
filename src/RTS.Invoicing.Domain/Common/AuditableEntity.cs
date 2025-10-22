using System;

namespace RTS.Invoicing.Domain.Common
{
    public class AuditableEntity<TId> : Entity<TId> where TId : notnull
    {
        protected AuditableEntity()
             : base()
        {
            // For ORM Only.
        }

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

        public void EntityModified()
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
