using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;
using System.Text.RegularExpressions;

namespace RTS.Invoicing.Domain.ValueObjects
{
    /// <summary>
    /// Represents an Email address as a Value Object, ensuring immutability and enforcing validation rules upon creation.
    /// </summary>
    public record Email
    {
        /// <summary>
        /// Gets the validated, immutable string value of the email address.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new, valid instance of the <see cref="Email"/> Value Object. 
        /// It is private to force consumers to use the static <see cref="Create"/> method.
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the Email value object with the validated email string.
        /// </summary>
        /// <param name="value">The validated email string.</param>
        private Email(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Factory method used to safely instantiate an <see cref="Email"/> object, performing necessary validation.
        /// </summary>
        /// <param name="email">The email string to validate and create.</param>
        /// <summary>
        /// Creates an <see cref="Email"/> value object from the provided string after validating that it is not empty and matches a basic email format.
        /// </summary>
        /// <param name="email">The input email address to validate; may be null or whitespace.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Email"/> on success, or an <see cref="Error"/> describing the validation failure.</returns>
        public static Result<Email> Create(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>(new Error("Email.Empty", "Email is empty."));
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                return Result.Failure<Email>(PersonErrors.InvalidEmailFormat);
            }

            return Result.Success(new Email(email));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Email"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="email">The email Value Object.</param>
        /// <returns>The string value of the email.</returns>
        public static implicit operator string(Email email) => email.Value;
    }
}