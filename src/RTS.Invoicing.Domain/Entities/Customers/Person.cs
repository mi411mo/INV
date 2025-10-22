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
        /// <summary>
        /// Parameterless constructor reserved for ORM and serializer use.
        /// </summary>
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
        /// <summary>
        /// Initializes a new instance of <see cref="Person"/> with the specified identifier and field values and sets CreatedAt to UTC now.
        /// </summary>
        /// <param name="id">The person's identifier.</param>
        /// <param name="arabicName">The person's Arabic name.</param>
        /// <param name="englishName">The person's English name.</param>
        /// <param name="email">The person's email value object.</param>
        /// <param name="phone">The person's phone number.</param>
        /// <param name="address">The person's address.</param>
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
        /// <summary>
        /// Creates a new Person after validating that both Arabic and English names are provided.
        /// </summary>
        /// <returns>
        /// A successful Result containing the created Person when both names are valid; otherwise a failed Result with PersonErrors.InvalidName.
        /// </returns>
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