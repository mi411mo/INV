using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;

namespace RTS.Invoicing.Domain.Aggregates.Merchants
{
    /// <summary>
    /// Manages the numbering sequence configuration for invoices issued by a specific merchant.
    /// This entity tracks the prefix and the last used number to generate the next unique invoice number.
    /// </summary>
    public class InvoiceSequence : Entity<InvoiceSequenceId>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="InvoiceSequence"/> class from being created.
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
        /// <param name="prefix">The prefix.</param>
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
        /// Creates a new <see cref="InvoiceSequence"/> instance for the specified merchant with the given prefix.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public int GetNextSequence()
        {
            return ++LastValue;
        }
    }
}
