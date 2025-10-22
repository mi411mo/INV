using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Entities.Merchants
{
    /// <summary>
    /// Manages the numbering sequence configuration for invoices issued by a specific merchant.
    /// This entity tracks the prefix and the last used number to generate the next unique invoice number.
    /// </summary>
    public class InvoiceSequence : Entity<InvoiceSequenceId>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="InvoiceSequence"/> class from being created.
        /// <summary>
        /// Parameterless constructor reserved for ORM usage only; prevents external default instantiation.
        /// </summary>
        private InvoiceSequence()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceSequence"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <summary>
        /// Initializes a new InvoiceSequence with the specified identifier, merchant, and prefix, and sets the sequence's LastValue to 0.
        /// </summary>
        /// <param name="id">The unique identifier for the invoice sequence.</param>
        /// <param name="merchantId">The identifier of the merchant that owns this sequence.</param>
        /// <param name="prefix">The static text prefix used when composing invoice numbers (for example, "INV-").</param>
        private InvoiceSequence(
            InvoiceSequenceId id,
            MerchantId merchantId,
            string prefix)
            : base(id)
        {
            MerchantId = merchantId;
            Prefix = prefix;
            LastValue = 0;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the merchant who issued the invoice.
        /// </summary>
        public MerchantId MerchantId { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the static text prefix used for the invoice number (e.g., "INV-", "ABC-").
        /// </summary>
        public string Prefix { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the last numeric value used in the sequence.
        /// The next invoice number will typically be generated using: Prefix + (LastValue + 1).
        /// </summary>
        public int LastValue { private set; get; } = 0;

        /// <summary>
        /// Gets or sets the concurrency token (RowVersion) used for optimistic locking.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public byte[] ConcurrencyToken { private set; get; } = null!;

        /// <summary>
        /// Creates invoice sequence with the 
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="prefix">The prefix.</param>
        /// <summary>
        /// Creates a new InvoiceSequence for the given merchant using the provided prefix after validating the prefix.
        /// </summary>
        /// <param name="merchantId">Identifier of the merchant that will own the invoice sequence.</param>
        /// <param name="prefix">Static text prefix to prepend to generated invoice numbers.</param>
        /// <returns>A Result containing the created InvoiceSequence with LastValue initialized to 0 on success; a failure Result with <see cref="InvoiceSequenceErrors.PrefixRequired"/> if <paramref name="prefix"/> is null, empty, or whitespace, or with <see cref="InvoiceSequenceErrors.PrefixTooLong"/> if <paramref name="prefix"/> exceeds <c>Constants.MAX_PREFIX_CHARACTERS</c>.</returns>
        public static Result<InvoiceSequence> Create(
            MerchantId merchantId,
            string prefix
            )
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return Result.Failure<InvoiceSequence>(InvoiceSequenceErrors.PrefixRequired);
            }

            if (prefix.Length > Constants.MAX_PREFIX_CHARACTERS)
            {
                return Result.Failure<InvoiceSequence>(InvoiceSequenceErrors.PrefixTooLong);
            }

            var invoiceSequence = new InvoiceSequence(new InvoiceSequenceId(0), merchantId, prefix);
            return Result.Success(invoiceSequence);
        }

        /// <summary>
        /// Gets the next sequence.
        /// </summary>
        /// <summary>
        /// Advances LastValue by one and returns the next numeric sequence for invoices.
        /// </summary>
        /// <returns>The next sequence value after incrementing LastValue.</returns>
        public int GetNextSequence()
        {
            return ++LastValue;
        }
    }
}