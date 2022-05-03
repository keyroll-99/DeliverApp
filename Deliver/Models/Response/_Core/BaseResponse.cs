namespace Models.Response._Core
{

    public class BaseRespons
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

        public static BaseRespons Fail(string error)
            => new()
            {
                Error = error,
                IsSuccess = false
            };

        public static BaseRespons Success()
            => new()
            {
                Error = null,
                IsSuccess = true
            };

        public static BaseRespons<T> Success<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class BaseRespons<T> : BaseRespons
    {
        public T Data { get; set; }

        public static new BaseRespons<T> Fail(string error) =>
             new()
             {
                 IsSuccess = false,
                 Error = error
             };

        public static BaseRespons<T> Success(T data) =>
            new()
            {
                IsSuccess = true,
                Data = data
            };

        public static implicit operator T(BaseRespons<T> response) => response.Data;
        public static implicit operator BaseRespons<T>(T response) => Success(response);
    }
}