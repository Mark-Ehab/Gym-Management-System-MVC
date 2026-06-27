using System;
using System.Collections.Generic;
using System.Text;
namespace GymManagementSystem.BusinessLogic.Common;

public class Result
{
    // Properties
    public bool IsSuccessful { get; }
    public bool IsFailure
        => !IsSuccessful;
    public Error? Error { get; }

    // Constructors
    protected Result(bool isSuccessful, Error? error)
    {
        // Check if passed parameters will create valid
        // state Result instance
        if ((isSuccessful && error is not null) ||
           (!isSuccessful && error is null))
            throw new ArgumentException("Inavlid Result Instance Arguments !");
        IsSuccessful = isSuccessful;
        Error = error;
    }

    // Methods
    public static Result Success()
        => new(true, null);
    public static Result Failure(Error error)
        => new(false, error);
}
public sealed class Result<T> : Result
{
    // Properties
    public T Value { get; }

    // Constructors
    private Result(bool isSuccessful, Error? error, T value) : base(isSuccessful, error)
    {
        this.Value = value;
    }

    // Methods
    public static Result<T> Success(T value)
        => new(true, null, value);
    public new static Result<T> Failure(Error error)
        => new(false, error, default!);
}