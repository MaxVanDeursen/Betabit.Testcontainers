namespace Betatalks.Testcontainers.Core.Results;

public class OperationResult
{
    protected OperationResult()
    {
    }

    public Exception? Exception { get; private init; }
    public string? ErrorMessage { get; private init; }
    public bool Successful => ErrorMessage == null;
    public bool Failed => !Successful;

    public static OperationResult Success()
    {
        return new OperationResult();
    }

    public static OperationResult<T> Success<T>(T value)
    {
        return new OperationResult<T>() { Value = value };
    }

    public static OperationResult Failure(Exception exception)
    {
        return new OperationResult { Exception = exception, ErrorMessage = exception?.Message };
    }

    public static OperationResult Failure(Exception exception, string errorMessage)
    {
        return new OperationResult { Exception = exception, ErrorMessage = errorMessage };
    }

    public static OperationResult Failure(string errorMessage)
    {
        return new OperationResult { ErrorMessage = errorMessage };
    }

    public static OperationResult<T> Failure<T>(Exception exception)
    {
        return new OperationResult<T> { Exception = exception, ErrorMessage = exception?.Message };
    }

    public static OperationResult<T> Failure<T>(Exception exception, string errorMessage)
    {
        return new OperationResult<T> { Exception = exception, ErrorMessage = errorMessage };
    }
    public static OperationResult<T> Failure<T>(string errorMessage)
    {
        return new OperationResult<T> { ErrorMessage = errorMessage };
    }
}

public class OperationResult<T> : OperationResult
{
    internal OperationResult() : base()
    {
    }

    public T? Value { get; internal init; }
}
