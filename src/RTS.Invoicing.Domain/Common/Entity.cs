using System.Diagnostics.CodeAnalysis;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Base class for domain entities.
    /// </summary>
    public abstract class Entity<TId> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;

        protected Entity(TId id)
        {
            Id = id;
        }

        protected Entity()
        {
        }

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

        public int GetHashCode([DisallowNull] Entity<TId> obj)
        {
            return obj.Id.GetHashCode() * 27;
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}
