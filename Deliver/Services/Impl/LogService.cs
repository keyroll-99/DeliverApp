using Models.Db;
using Models.Db.ConstValues;
using Models.Logger;
using Repository.Repository.Interface;
using Services.Interface;

namespace Services.Impl;

public class LogService : ILogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }


    public async Task LogError(string message)
    {
        await LogError(new LogMessage { Message = message });
    }

    public async Task LogError(LogMessage logMessage)
    {
        if (await ValidMessage(logMessage))
        {
            await Log(logMessage, LogTypesEnum.Exception);
        }
        else if(logMessage.HasInfomation)
        {
            logMessage.Message = " ";
            await Log(logMessage, LogTypesEnum.Exception);
        }

    }

    public async Task LogInfo(string message)
    {
        await LogInfo(new LogMessage { Message = message });
    }
    
    public async Task LogInfo(LogMessage logMessage)
    {
        if(! await ValidMessage(logMessage))
        {
            return;
        }

        await Log(logMessage, LogTypesEnum.Info);
    }


    private async Task<bool> ValidMessage(LogMessage logMessage)
    {
        if (logMessage is null || !logMessage.IsValid)
        {
            await LogError(new LogMessage
            {
                Message = "missing log message"
            });

            return false;
        }

        return true;
    }

    private async Task Log(LogMessage logMessage, LogTypesEnum logTypes)
    {
        var log = new Log
        {
            InnerException = logMessage.InnerException,
            Message = logMessage.Message,
            LogType = ((int)logTypes),
            StackTrace = logMessage.StackTrace,
        };

        await _logRepository.AddAsync(log);
    }
}
