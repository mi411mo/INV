using RTS.Invoicing.Domain.Common;
using RTS.Invoicing.Domain.Errors;
using RTS.Invoicing.Domain.ValueObjects;
using System;

namespace RTS.Invoicing.Domain.Entities.Customers
{
    /// <summary>
    /// Represent the person entity.
    /// </summary>
    public class Person : AuditableEntity<PersonId>
    {
        private Person()
            : base()
        {
            // For ORM Only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="arabicName">Person Arabic name.</param>
        /// <param name="englishName">Person English name.</param>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="address">The address.</param>
        private Person(
            PersonId id,
            string arabicName,
            string englishName,
            Email email,
            string phone,
            string address)
            : base(id)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
            Email = email;
            Phone = phone;
            Address = address;

            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the person's name in Arabic.
        /// </summary>
        public string ArabicName { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the person's name in English.
        /// </summary>
        public string EnglishName { private set; get; } = string.Empty;

        /// <summary>
        /// Gets or sets the person's email address.
        /// </summary>
        public Email Email { private set; get; } = null!;

        /// <summary>
        /// Gets or sets the person's phone number.
        /// </summary>
        public string Phone { private set; get; } = string.Empty; // TODO: Make ValueObject for the Phone field to ensure proper formatting and validation.

        /// <summary>
        /// Gets or sets the person's physical address details.
        /// </summary>
        public string Address { private set; get; } = string.Empty; // TODO: Make ValueObject for the Address field to encapsulate address components (e.g., street, city, country).

        /// <summary>
        /// Creates the specified person.
        /// </summary>
        /// <param name="arabicName">Person Arabic name.</param>
        /// <param name="englishName">Person English name.</param>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static Result<Person> Create(
            string arabicName,
            string englishName,
            Email email,
            string phone,
            string address)
        {
            if (string.IsNullOrWhiteSpace(arabicName) || string.IsNullOrWhiteSpace(englishName))
            {
                return Result.Failure<Person>(PersonErrors.InvalidName);
            }

            var person = new Person(new PersonId(0), arabicName, englishName, email, phone, address);
            return Result.Success(person);
        }
    }
}
