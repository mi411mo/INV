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
        /// <exception cref="System.InvalidOperationException"></exception>
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
        /// <returns>A <see cref="Result"/> instance indicating success result.</returns>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Create a success result with specific value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Create a failure result.
        /// </summary>
        /// <param name="error">The error occurred.</param>
        /// <returns>A <see cref="Result"/> instance indicating failure result.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Create a failure result.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }
}
