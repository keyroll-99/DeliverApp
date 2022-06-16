namespace Models.Logger;

public class LogMessage
{
    public string Message { get; set; }
    public string InnerException { get; set; }
    public string StackTrace { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(Message);
    public bool HasInfomation => 
        !string.IsNullOrEmpty(InnerException)
        || !string.IsNullOrWhiteSpace(StackTrace)
        || !string.IsNullOrWhiteSpace(Message);
}
