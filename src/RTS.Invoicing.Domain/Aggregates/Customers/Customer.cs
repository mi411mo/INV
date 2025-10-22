using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Aggregates.Customers
{
    /// <summary>
    /// Represents a customer within the invoicing system, identified uniquely and linked to a specific merchant.
    /// </summary>
    public class Customer : Entity<CustomerId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> entity.
        /// </summary>
        private Customer()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="personId">The person identifier.</param>
        private Customer(
            CustomerId id,
            PersonId personId)
            : base(id)
        {
            PersonId = personId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the person who owns this customer record.
        /// This establishes the relationship to the parent Merchant entity (or Aggregate Root).
        /// </summary>
        public PersonId PersonId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the customer record is soft-deleted.
        /// </summary>
        public bool IsDeleted { private set; get; } = false;

        /// <summary>
        /// Gets or sets a merchant-specific, unique reference code or number for the customer.
        /// This provides a business-friendly, external identifier.
        /// </summary>
        public string? CustomerReference { private set; get; } = null;

        // Navigation Properties        

        /// <summary>
        /// Gets or sets the customer identity.
        /// </summary>
        /// <value>The person identity.</value>
        public Person Person { private set; get; } = null!;

        /// <summary>
        /// Creates the specified customer with person identifier.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <returns></returns>
        public static Customer Create(PersonId personId)
        {
            return new Customer(new CustomerId(0), personId);
        }

        /// <summary>
        /// Sets the merchant-specific customer reference.
        /// </summary>
        /// <param name="reference">The customer reference.</param>
        public Result SetReference(string reference)
        {
            if (IsDeleted)
            {
                return Result.Failure(CustomerErrors.CustomerIsDeleted);
            }

            if (string.IsNullOrWhiteSpace(reference))
            {
                return Result.Failure(CustomerErrors.EmptyReference);
            }

            CustomerReference = reference;
            return Result.Success();
        }

        /// <summary>
        /// Marks the customer as deleted. This is a soft delete.
        /// </summary>
        public Result Delete()
        {
            if (IsDeleted)
            {
                return Result.Failure(CustomerErrors.CustomerIsDeleted);
            }

            IsDeleted = true;
            return Result.Success();
        }
    }
}
