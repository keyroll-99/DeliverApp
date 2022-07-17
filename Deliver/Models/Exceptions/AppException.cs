namespace Models.Exceptions
{
    public class AppException : Exception
    {
        public int? StatusCode { get; set; }

        public AppException()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
