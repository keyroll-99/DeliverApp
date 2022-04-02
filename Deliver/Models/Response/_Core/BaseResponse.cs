namespace Models.Response._Core;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    public static BaseResponse<T> Fail(String error) =>
         new BaseResponse<T>
         {
             IsSuccess = false,
             Error = error
         };

    public static BaseResponse<T> Success(T data) =>
        new BaseResponse<T>
        {
            IsSuccess = true,
            Data = data
        };

    public static implicit operator T(BaseResponse<T> response) => response.Data;
    public static implicit operator BaseResponse<T>(T response) => BaseResponse<T>.Success(response);
}
