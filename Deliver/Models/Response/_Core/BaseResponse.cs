namespace Models.Response._Core;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    public static BaseResponse<T> Fail(String error)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Error = error
        };
    }
}
