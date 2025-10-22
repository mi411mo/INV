using System.Diagnostics.CodeAnalysis;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Base class for domain entities.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public abstract class Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Gets the entity's identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public TId Id { get; protected set; } = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TId}"/> class with a specific identifier.
        /// </summary>
        /// <param name="id">The entity's identifier.</param>
        protected Entity(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current entity.
        /// </summary>
        /// <remarks>
        /// Equality is determined by comparing the <see cref="Id"/> properties of the entities.
        /// </remarks>
        /// <param name="obj">The object to compare with the current entity.</param>
        /// <returns><c>true</c> if the specified object is equal to the current entity; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Id.Equals(default) || other.Id.Equals(default))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// Gets the hash code for the specified entity.
        /// </summary>
        /// <param name="obj">The entity for which to get the hash code.</param>
        /// <returns>The hash code for the entity, based on its <see cref="Id"/>.</returns>
        public int GetHashCode([DisallowNull] Entity<TId> obj)
        {
            return obj.Id.GetHashCode() * 27;
        }

        /// <summary>
        /// Gets the hash code for the current entity.
        /// </summary>
        /// <returns>The hash code for the entity, based on its <see cref="Id"/>.</returns>
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}
