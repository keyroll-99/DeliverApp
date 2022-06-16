using Models.Db;
using Models.Db.ConstValues;
using Models.Logger;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service;

public class LogServiceTest
{
    private readonly ILogRepository _logRepositoryMock;

    private ILogService _service;

    public LogServiceTest()
    {
        _logRepositoryMock = Substitute.For<ILogRepository>();

        _service = new LogService(_logRepositoryMock);
    }

    [Fact]
    public async Task LogError_WhenValidError_ShouldLogError()
    {
        // arrange
        var logMessage = new LogMessage
        {
            Message = "message"
        };

        // act
        await _service.LogError(logMessage);

        // assert
        await _logRepositoryMock
            .Received(1)
            .AddAsync(Arg.Is<Log>(x =>
                x.Message == logMessage.Message
                && x.LogType == ((int)LogTypesEnum.Exception))
        );
    }

    [Fact]
    public async Task LogError_WhenInvalidErrorLogMessage_ShouldLogInvalidLog()
    {
        // arrang
        var logMessage = new LogMessage
        {
            Message = string.Empty,
        };

        // act
        await _service.LogError(logMessage);

        // assert
        await _logRepositoryMock
            .Received(1)
            .AddAsync(Arg.Is<Log>(x => 
                x.Message == "missing log message"
                && x.LogType == ((int)LogTypesEnum.Exception))
        );
    }

    [Fact]
    public async Task LogError_WhenMessageIsNullButLogMessageHaveInfomation_ShouldLogInvalidLogAndGivenLog()
    {
        // arrange
        var logMessage = new LogMessage
        {
            Message = string.Empty,
            InnerException = "inner"
        };

        // act
        await _service.LogError(logMessage);

        // assert
        await _logRepositoryMock
            .Received(2)
            .AddAsync(Arg.Is<Log>(x => x.LogType == ((int)LogTypesEnum.Exception)));
    }

    [Fact]
    public async Task LogInfo_WhenInvalidMessage_LogError()
    {
        // act
        await _service.LogInfo(string.Empty);

        // assert
        await _logRepositoryMock.Received(1).AddAsync(Arg.Is<Log>(x => x.LogType == ((int)LogTypesEnum.Exception)));
    }

    [Fact]
    public async Task LogInfo_WhenValidMessage_LogInfo()
    {
        // act
        await _service.LogInfo("info");

        // assert
        await _logRepositoryMock
            .Received(1)
            .AddAsync(Arg.Is<Log>(x =>
                x.LogType == (int)LogTypesEnum.Info
                && x.Message == "info")
        );
    }
}
