using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Request.Authentication;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service;

public class AuthenticationServiceTest
{
    #region mock
    private readonly IQueryable<User> _userDataMock = new List<User>
    {
        new User
        {
            Id = 1,
            Username = "test",
            Password = BCrypt.Net.BCrypt.HashPassword("test"),
            Name = "TestName",
            Surname = "TestSurname",
            Company = new Company
            {
                Hash = Guid.NewGuid()
            }
        },
        new User
        {
            Id = 2,
            Username = "fired",
            Password = BCrypt.Net.BCrypt.HashPassword("test"),
            IsFired = true,
            Company = new Company
            {
                Hash = Guid.NewGuid(),
            }
        }
    }.BuildMock();


    private readonly LoggedUser _loggedUser = new LoggedUser
    {
        Id = 1,
        Roles = new List<string> { "testRole" }
    };

    #endregion

    private readonly IUserRepository _userRepositoryMock;
    private readonly IAuthenticationUtils _jwtUtilsMock;
    private readonly IAuthenticationService _service;
    private readonly IOptions<LoggedUser> _options;

    public AuthenticationServiceTest()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _userRepositoryMock.GetAll().Returns(_userDataMock);

        _jwtUtilsMock = Substitute.For<IAuthenticationUtils>();

        _options = Substitute.For<IOptions<LoggedUser>>();
        _options.Value.Returns(_loggedUser);

        _service = new AuthenticationService(_jwtUtilsMock, _userRepositoryMock, _options);
    }

    [Fact]
    public async Task Login_WhenLoginValid_ReturnAuthReposne()
    {
        // arrnage
        var request = new LoginRequest
        {
            Username = "test",
            Password = "test"
        };
        var jwtToken = Guid.NewGuid().ToString();

        _jwtUtilsMock.GenerateJwtToken(Arg.Any<User>()).Returns(jwtToken);
        _jwtUtilsMock.GenerateRefreshToken(Arg.Any<User>(), Arg.Any<string>()).Returns(new RefreshToken
        {
            Token = jwtToken,
        });

        // act
        var response = await _service.Login(request, "127.0.0.1");

        // assert
        response.Should().NotBeNull();
        response.Name.Should().Be("TestName");
        response.Surname.Should().Be("TestSurname");
        response.Username.Should().Be("test");
    }

    [Theory]
    [InlineData("test", "asdasd")]
    [InlineData("erter", "test")]
    [InlineData("", "test")]
    [InlineData("test", "")]
    public async Task Login_WhenRequestIsInvalid_ThenReturnError(string username, string password)
    {
        // arrange
        var request = new LoginRequest
        {
            Password = password,
            Username = username
        };

        // act
        Func<Task> act = async () => await _service.Login(request, "local");

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidLoginOrPassword);
    }

    [Fact]
    public async Task Login_WhenUserIsFired_ThenThrowError()
    {
        // arrange
        var request = new LoginRequest
        {
            Password = "test",
            Username = "fired"
        };

        // act
        Func<Task> act = async () => await _service.Login(request, "test");

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserIsBlocker);
    }

    [Fact]
    public async Task RefreshToken_WhenTokenDoesntExist_RetrunError()
    {
        // arrange
        var token = "token";
        var ip = "ip";
        _jwtUtilsMock.GetRefreshTokenByToken(Arg.Any<string>()).Returns(null as RefreshToken);

        // act
        Func<Task> act = async () => await _service.RefreshToken(token, ip);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidToken);
    }

    [Fact]
    public async Task RefreshToken_WhenTokenIsUsed_RetrunError()
    {
        // arrange
        var token = "token";
        var ip = "local";
        _jwtUtilsMock.GetRefreshTokenByToken(Arg.Any<string>()).Returns(new RefreshToken
        {
            Id = 1,
            IsUsed = true
        });

        // act
        Func<Task> act = async () => await _service.RefreshToken(token, ip);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.TokenAlreadyTaken);
    }
}
