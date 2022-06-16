using Models.Db;
using Models.Logger;

namespace Services.Interface;

public interface ILogService
{
    Task LogError(LogMessage logMessage);
    Task LogError(string message);
    Task LogInfo(LogMessage logMessage);
    Task LogInfo(string message);
}
