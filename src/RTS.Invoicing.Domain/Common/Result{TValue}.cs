using System;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Represents a successful or failed result of an operation with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    public class Result<TValue> : Result
    {
        /// <summary>
        /// The value.
        /// </summary>
        private readonly TValue? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
        /// <param name="error">The error.</param>
        /// <exception cref="InvalidOperationException">A success result cannot be created with null value.</exception>
        public Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            if (isSuccess && value is null)
            {
                throw new InvalidOperationException("A success result cannot be created with null value.");
            }

            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="InvalidOperationException">The value of failure cannot be accessed</exception>
        public TValue? Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("The value of failure cannot be accessed");

        /// <summary>
        /// Performs an implicit conversion from <see cref="Nullable{TValue}"/> to <see cref="Result{TValue}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
