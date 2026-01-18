namespace WalletService;

public class Result
{
    public string? Error { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string error) => new() { IsSuccess = false, Error = error };
}

public class Result<TValue> : Result
{
    public TValue? Value { get; init; }

    private Result() { }

    public static Result<TValue> Success(TValue value) => new()
    {
        IsSuccess = true,
        Value = value
    };

	public static new Result<TValue> Failure(string error) => new() { IsSuccess = false, Error = error };
}
