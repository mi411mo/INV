using System.Diagnostics.CodeAnalysis;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Base class for domain entities.
    /// </summary>
    public abstract class Entity<TId> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;

        /// <summary>
        /// Initializes a new instance of the Entity&lt;TId&gt; class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the entity.</param>
        protected Entity(TId id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
        /// </summary>
        /// <remarks>
        /// Provided for use by ORMs, serializers, and derived classes that require a parameterless constructor.
        /// </remarks>
        protected Entity()
        {
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current entity.
        /// </summary>
        /// <param name="obj">The object to compare with the current entity.</param>
        /// <returns>`true` if <paramref name="obj"/> is the same instance or an Entity with the same Id that is not the default value; `false` otherwise.</returns>
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

        public static bool operator ==(Entity<TId>? first, Entity<TId>? second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null || second is null)
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Computes a hash code for the specified entity based on its Id.
        /// </summary>
        /// <param name="obj">Entity whose Id is used to compute the hash code.</param>
        /// <returns>An integer hash code derived from the entity's Id (Id.GetHashCode() multiplied by 27).</returns>
        public int GetHashCode([DisallowNull] Entity<TId> obj)
        {
            return obj.Id.GetHashCode() * 27;
        }

        /// <summary>
        /// Computes the hash code for this entity based on its Id.
        /// </summary>
        /// <returns>An integer hash code computed from the entity's Id.</returns>
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}