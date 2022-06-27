using FluentAssertions;
using Integrations.Interface;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
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

}
