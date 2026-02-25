using System;

namespace SGA.Domain.Common
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new ArgumentException("Successful result cannot have an error", nameof(error));

            if (!isSuccess && error == Error.None)
                throw new ArgumentException("Failure result must have an error", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);
        public static Result<TValue> Failure<TValue>(Error error) => Result<TValue>.Failure(error);
    }

    public sealed class Result<TValue> : Result
    {
        private Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        public TValue Value { get; } = default!;

        public static Result<TValue> Success(TValue value) =>
            new(value, true, Error.None);

        public static new Result<TValue> Failure(Error error) =>
            new(default, false, error);
    }
}
