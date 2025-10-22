using System;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Represents an entity that includes audit information, such as creation and modification timestamps.
    /// </summary>
    /// <remarks>The <see cref="AuditableEntity{TId}"/> class extends the functionality of the base <see
    /// cref="Entity{TId}"/> class by adding properties to track when the entity was created and last modified. The <see
    /// cref="CreatedAt"/> property is automatically set to the current UTC date and time when the entity is
    /// initialized. The <see cref="ModifiedAt"/> property is updated to the current UTC date and time whenever the <see
    /// cref="EntityModified"/> method is called.</remarks>
    /// <typeparam name="TId">The type of the unique identifier for the entity. This type must be non-nullable.</typeparam>
    public class AuditableEntity<TId> : Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableEntity{TId}"/> class.
        /// </summary>
        protected AuditableEntity()
             : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditableEntity{TId}"/> class with the specified identifier.
        /// </summary>
        /// <remarks>The <see cref="CreatedAt"/> property is automatically set to the current UTC date and
        /// time upon initialization.</remarks>
        /// <param name="id">The unique identifier for the entity.</param>
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
        /// Updates the modification timestamp of the entity to the current UTC time.
        /// </summary>
        /// <remarks>This method sets the <c>ModifiedAt</c> property to the current UTC time, indicating
        /// the last modification time of the entity.</remarks>
        public void EntityModified()
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
