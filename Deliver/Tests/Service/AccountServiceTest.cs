using FluentAssertions;
using Integrations.Interface;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Integration;
using Models.Request.Account;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service;

public class AccountServiceTest
{
    #region mock

    private readonly IQueryable<User> _userDataMock = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "test",
                Email = "email@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("test"),
                Name = "TestName",
                Surname = "TestSurname",
                Hash = Guid.NewGuid(),
                UserRole = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = new Role
                        {
                            Name = SystemRoles.Admin
                        }
                    }
                },
                PasswordRecoveries = new List<PasswordRecovery>
                {
                    new PasswordRecovery
                    {
                        CreateTime = DateTime.Now,
                        Hash = Guid.Parse("4a423fc8-4971-4d58-94dc-0045d010ff02"),
                    }
                }
            },
            new User
            {
                Id = 2,
                Username = "test",
                Name = "user_2",
                Surname = "user_2",
                Hash = Guid.Parse("b07b6398-47c7-4e0d-8d5b-27a199ae63a5"),
                Company = new Company
                {
                    Hash = Guid.Parse("63165844-94a4-4769-bcef-bcb0692d9d06")
                },
                UserRole = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = new Role
                        {
                            Name = SystemRoles.HR
                        }
                    }
                }

            }
        }.BuildMock();

    private readonly IQueryable<PasswordRecovery> _passwordRecoveryDataMock = new List<PasswordRecovery>
    {
        new PasswordRecovery
        {
            Id = 1,
            CreateTime = DateTime.Now.AddMinutes(-60),
            Hash = Guid.NewGuid()
        },
        new PasswordRecovery
        {
            Id = 2,
            CreateTime = DateTime.Now.AddMinutes(-10),
            Hash = Guid.NewGuid()
        },
    }.BuildMock();

    #endregion

    private readonly IAccountService _service;
    private readonly IUserRepository _userRepository;
    private readonly IOptions<LoggedUser> _loggedUserOptions;
    private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
    private readonly IMailService _mailService;

    public AccountServiceTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.GetAll().Returns(_userDataMock);

        _passwordRecoveryRepository = Substitute.For<IPasswordRecoveryRepository>();
        _passwordRecoveryRepository.GetAll().Returns(_passwordRecoveryDataMock);

        _loggedUserOptions = Substitute.For<IOptions<LoggedUser>>();
        _loggedUserOptions.Value.Returns(new LoggedUser
        {
            Id = 1,
            Roles = new List<string> { "test" }
        });

        _mailService = Substitute.For<IMailService>();

        _service = new AccountService(_userRepository, _loggedUserOptions, _passwordRecoveryRepository, _mailService);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("l")]
    public async Task UpdatePassword_WhenInvalidPassowrdLenght_ThenThrowException(string newPassword)
    {
        // act
        Func<Task> act = async () => await _service.UpdatePassword(new ChangePasswordRequest { Password = newPassword, OldPassword = "old-password" });

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidNewPassword);
    }

    [Fact]
    public async Task UpdatePassword_WhenUserIsNull_ThorwException()
    {
        // arrange
        _userRepository.GetByIdAsync(Arg.Any<long>()).Returns(null as User);

        // act
        Func<Task> act = async () => await _service.UpdatePassword(new ChangePasswordRequest { Password = "password", OldPassword = "old-password" });

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.CommonMessage);
    }

    [Fact]
    public async Task UpdatePassword_WhenOldPasswordIsIncorrect_ThenThrowException()
    {
        // arrange
        _userRepository.GetByIdAsync(Arg.Any<long>()).Returns(new User { Password = BCrypt.Net.BCrypt.HashPassword("old-password") });

        // act
        Func<Task> act = async () => await _service.UpdatePassword(new ChangePasswordRequest { Password = "password", OldPassword = "super-old-password" });

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidPassword);
    }

    [Fact]
    public async Task UpdatePassword_WhenRequestIsValid_ThenUdpateUser()
    {
        // arrange
        _userRepository.GetByIdAsync(Arg.Any<long>()).Returns(new User { Password = BCrypt.Net.BCrypt.HashPassword("old-password") });

        // act
        await _service.UpdatePassword(new ChangePasswordRequest { Password = "password", OldPassword = "old-password" });

        // assert
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(x => x.Password != null));
    }

    [Theory]
    [InlineData("", "", ErrorMessage.InvalidData)]
    [InlineData("test", "test", ErrorMessage.UserDosentExists)]
    public async Task InitRecoveryPassword_WhenRequestIsInvalid_ThrowException(string email, string username, string expectError)
    {
        // arrange
        var request = new PasswordRecoveryInitRequest
        {
            Email = email,
            Username = username,
        };

        // act
        Func<Task> act = async () => await _service.InitRecoveryPassword(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(expectError);
    }

    [Fact]
    public async Task InitPasswordRecovery_WhenRequestIsValid_ThenAddnRecoveryLinkDoDbAndSendEmail()
    {
        // arrange
        var request = new PasswordRecoveryInitRequest
        {
            Username = "test",
            Email = "email@example.com"
        };

        // act
        await _service.InitRecoveryPassword(request);

        // assert
        await _passwordRecoveryRepository.Received(1).AddAsync(Arg.Is<PasswordRecovery>(x => x.UserId == 1));
        await _mailService.Received(1).SendPasswordRecoveryMessage(Arg.Is<PasswordRecoveryMessageModel>(x => x.Email == request.Email));
    }

    [Theory]
    [InlineData("", "", ErrorMessage.InvalidData)]
    [InlineData("password", "ss", ErrorMessage.TokenExpiredOrInvalidPasswordRecoveryLink)]
    public async Task SetNewPassword_WhenReqestIsInvalid_ThenThrowError(string newPassword, string recoveryKey, string expectError)
    {
        // arrange
        var request = new PasswordRecoverySetNewPasswordRequest
        {
            NewPassword = newPassword,
            RecoveryKey = recoveryKey,
        };

        // act
        Func<Task> act = async () => await _service.SetNewPassword(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(expectError);
    }

    [Fact]
    public async Task SetNewPassword_WhenReqestIsValid_SetNewPassowrd()
    {
        // arrange
        var request = new PasswordRecoverySetNewPasswordRequest
        {
            NewPassword = "password",
            RecoveryKey = "4a423fc8-4971-4d58-94dc-0045d010ff02"
        };

        // act
        await _service.SetNewPassword(request);

        // assert
        await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task IsValidRecoveryKey_WhenRecoveryKeyIsInvalid_ThenRetrunFalse(int passwordRecoveryId)
    {
        // arrange
        var recoveryKey = _passwordRecoveryDataMock.FirstOrDefault(x => x.Id == passwordRecoveryId);

        // act
        var result = await _service.IsValidRecoveryKey(recoveryKey?.ToString() ?? Guid.NewGuid().ToString());

        // assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsValidRecoveryKey_WhenRecoveryKeyIsValid_ThenReturnTrue()
    {
        // arrange
        var recoveryKey = _passwordRecoveryDataMock.Where(x => x.CreateTime > DateTime.Now.AddMinutes(-30)).First();

        // act
        var result = await _service.IsValidRecoveryKey(recoveryKey.Hash.ToString());

        // assert
        result.Should().BeTrue();
    }
}
