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
        /// <param name="value">The validated email string.</param>
        private Email(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Factory method used to safely instantiate an <see cref="Email"/> object, performing necessary validation.
        /// </summary>
        /// <param name="email">The email string to validate and create.</param>
        /// <returns>A <see cref="Result{T}"/> containing the new <see cref="Email"/> object on success, or an <see cref="Error"/> on failure.</returns>
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
