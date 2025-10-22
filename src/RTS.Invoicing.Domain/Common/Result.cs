using System;

namespace RTS.Invoicing.Domain.Common
{
    /// <summary>
    /// Represents the result of an operation, which can either be successful or failed.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">if set to <c>true</c> [is success].</param>
        /// <param name="error">The error.</param>
        /// <summary>
        /// Initializes a Result indicating success or failure and associates the corresponding error state.
        /// </summary>
        /// <param name="isSuccess">true to create a successful result; false to create a failed result.</param>
        /// <param name="error">The error associated with a failed result. Must be <see cref="Error.None"/> when <paramref name="isSuccess"/> is true; must not be <see cref="Error.None"/> when <paramref name="isSuccess"/> is false.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when <paramref name="isSuccess"/> and <paramref name="error"/> are inconsistent.</exception>
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Gets a value represent whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value represent whether the operation failed.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error associated with a failed operation.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Create a success result.
        /// </summary>
        /// <summary>
/// Creates a successful Result indicating the operation succeeded.
/// </summary>
/// <returns>A <see cref="Result"/> representing success with <see cref="Error.None"/>.</returns>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Create a success result with specific value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <summary>
/// Creates a successful Result containing the provided value.
/// </summary>
/// <param name="value">The value to include in the successful result.</param>
/// <returns>A Result&lt;TValue&gt; representing success that contains the specified value and Error.None.</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Create a failure result.
        /// </summary>
        /// <param name="error">The error occurred.</param>
        /// <summary>
/// Create a failed <see cref="Result"/> using the provided error.
/// </summary>
/// <param name="error">The error representing the failure; must not be <see cref="Error.None"/>.</param>
/// <returns>A <see cref="Result"/> representing failure with the specified error.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Create a failure result.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="error">The error.</param>
        /// <summary>
/// Creates a failed Result&lt;TValue&gt; initialized with the specified error.
/// </summary>
/// <param name="error">The error that caused the failure.</param>
/// <returns>A failed Result&lt;TValue&gt; containing the specified error.</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }
}