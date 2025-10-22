using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Entities.Customers
{
    /// <summary>
    /// Represents a customer within the invoicing system, identified uniquely and linked to a specific merchant.
    /// </summary>
    public class Customer : Entity<CustomerId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> entity.
        /// <summary>
        /// Parameterless constructor used by ORM frameworks for entity materialization.
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
        /// <summary>
        /// Initializes a new Customer with the specified identifier and person association for internal/ORM use.
        /// </summary>
        /// <param name="id">The customer identifier.</param>
        /// <param name="personId">The associated person identifier.</param>
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
        /// <summary>
        /// Creates a new Customer linked to the specified person identifier.
        /// </summary>
        /// <param name="personId">The identifier of the related Person.</param>
        /// <returns>The created Customer instance.</returns>
        public static Customer Create(PersonId personId)
        {
            return new Customer(new CustomerId(0), personId);
        }

        /// <summary>
        /// Sets the merchant-specific customer reference.
        /// </summary>
        /// <summary>
        /// Sets the merchant-facing customer reference for this customer.
        /// </summary>
        /// <param name="reference">The merchant-specific reference string to assign; must not be null, empty, or whitespace.</param>
        /// <returns>`Success` when the reference is assigned; otherwise a failure `Result` with `CustomerIsDeleted` if the customer is already deleted, or `EmptyReference` if the provided reference is null, empty, or whitespace.</returns>
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
        /// <summary>
        /// Marks the customer as deleted (soft delete) if it is not already deleted.
        /// </summary>
        /// <returns>A success Result when deletion is performed; otherwise a failure Result with the CustomerIsDeleted error.</returns>
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