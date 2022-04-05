using FluentAssertions;
using Integrations.Interface;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Integration;
using Models.Request.User;
using Models.Request.utils;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils
{
    public class UserServiceTest
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
                Surname = "TestSurname"
            }
        }.BuildMock();

        private readonly LoggedUser _loggedUser = new()
        {
            Id = 1,
            Roles = new List<string> { SystemRoles.Admin }
        };

        #endregion

        private readonly IUserRepository _userRepositoryMock;
        private readonly IJwtUtils _jwtUtilsMock;
        private readonly ICompanyUtils _companyUtilsMock;
        private readonly IOptions<LoggedUser> _optionsMock;
        private readonly IRoleUtils _roleUtilsMock;
        private readonly IMailService _mailServiceMock;

        private readonly UserService _service;

        public UserServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _userRepositoryMock.GetAll().Returns(_userDataMock);

            _jwtUtilsMock = Substitute.For<IJwtUtils>();

            _companyUtilsMock = Substitute.For<ICompanyUtils>();

            _optionsMock = Substitute.For<IOptions<LoggedUser>>();
            _optionsMock.Value.Returns(_loggedUser);

            _roleUtilsMock = Substitute.For<IRoleUtils>();

            _mailServiceMock = Substitute.For<IMailService>();

            _service = new UserService(
                _userRepositoryMock,
                _jwtUtilsMock,
                _companyUtilsMock,
                _optionsMock,
                _roleUtilsMock,
                _mailServiceMock
            );
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
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.Name.Should().Be("TestName");
            response.Data.Surname.Should().Be("TestSurname");
            response.Data.Username.Should().Be("test");
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
            var response = await _service.Login(request, "local");

            // assert
            response.IsSuccess.Should().BeFalse();
            response.Data.Should().BeNull();
            response.Error.Should().Be("Invalid username or password");
        }

        [Fact]
        public async Task RefreshToken_WhenTokenDoesntExist_RetrunError()
        {
            // arrange
            var token = "token";
            var ip = "ip";
            _jwtUtilsMock.GetRefreshTokenByToken(Arg.Any<string>()).Returns(null as RefreshToken);

            // act
            var response = await _service.RefreshToken(token, ip);

            // assert
            response.IsSuccess.Should().BeFalse();
            response.Error.Should().Be("Invalid token");
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
            var response = await _service.RefreshToken(token, ip);

            // assert
            response.IsSuccess.Should().BeFalse();
            response.Error.Should().Be("Token already taken");
        }

        [Theory]
        [InlineData("", "ss", "ss")]
        [InlineData("ww", "", "ss")]
        [InlineData("ww", "aa", "")]
        public async Task CreateUser_WhenRequestIsInvalid_ReturnsError(string name, string email, string username)
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = name,
                Email = email,
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = username
            };

            // act
            var response = await _service.CreateUser(request);

            // assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task CreateUser_WhenUserDoesntHavaPermisson_ReturnsError()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _roleUtilsMock.HasPermissionToAddUser(Arg.Any<HasPermissionToAddUserRequest>()).Returns(false);

            // act
            var response = await _service.CreateUser(request);

            // assert

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
        }


        [Fact]
        public async Task CreateUser_WhenCompanyDoesntExists_ReturnsError()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(null as Company);

            _roleUtilsMock.HasPermissionToAddUser(Arg.Any<HasPermissionToAddUserRequest>()).Returns(true);

            // act
            var response = await _service.CreateUser(request);

            // assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task CreateUser_WhenRequestIsValid_CreateUserAndSendEmail()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });

            _roleUtilsMock.HasPermissionToAddUser(Arg.Any<HasPermissionToAddUserRequest>()).Returns(true);

            // act
            var response = await _service.CreateUser(request);

            // assert
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().NotBeNull();

            await _mailServiceMock
                .Received(1)
                .SendWelcomeMessage(Arg.Is<WelcomeMessageModel>(x =>
                   x.Email == request.Email
                   && x.Name == request.Name
                   && x.Surname == request.Surname
                   && x.Username == request.Username
                   && !string.IsNullOrEmpty(x.Password)
                ));

            await _userRepositoryMock.Received(1)
                .AddAsync(Arg.Is<User>(x =>
                    x.Email == request.Email
                   && x.Name == request.Name
                   && x.Surname == request.Surname
                   && x.Username == request.Username
                   && x.PhoneNumber == x.PhoneNumber
                   && !string.IsNullOrEmpty(x.Password)
                ));
        }

        [Fact]
        public async Task AddRoleToUser_WhenUserDoesntExists_ThorwException()
        {
            // arrange
            _userRepositoryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(null as User);

            // act
            Func<Task> act = async () => await _service.AddRoleToUser(Guid.NewGuid(), new List<long> { 1 });

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserDosentExists);
        }

        [Fact]
        public async Task AddRoleToUser_WhenUserExists_ThenCallUtils()
        {
            // arrange
            _userRepositoryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new User { Id = 1 });

            // arrange
            Func<Task> act = async () => await _service.AddRoleToUser(Guid.NewGuid(), new List<long> { 1 });

            // assert
            await act.Should().NotThrowAsync<AppException>();
            await _roleUtilsMock
                .Received(1)
                .AddRolesToUser(Arg.Is<User>(x => x.Id == 1), Arg.Is<List<long>>(x => x.Contains(1)));
        }

        [Fact]
        public async Task AddUserToCompany_WhenCompanyDoesntExists_ThrowException()
        {
            // arramge
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(null as Company);
            _userRepositoryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new User { Id = 1 });

            // act
            Func<Task> act = async () => await _service.AddUserToCompany(Guid.NewGuid(), Guid.NewGuid());

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.CompanyDoesntExists);
        }

        [Fact]
        public async Task AddUserToCompany_WhenUserDoesntExists_ThrowException()
        {
            // arramge
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(new Company { Hash = new Guid() });
            _userRepositoryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(null as User);

            // act
            Func<Task> act = async () => await _service.AddUserToCompany(Guid.NewGuid(), Guid.NewGuid());

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserDosentExists);

        }

        [Fact]
        public async Task AddUserToCompany_WhenRequestIsValid_ThenCallUpdate()
        {
            // arrange
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(new Company { Id = 1 });
            _userRepositoryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new User { Id = 1 });

            // act
            Func<Task> act = async () => await _service.AddUserToCompany(Guid.NewGuid(), Guid.NewGuid());

            // assert
            await act.Should().NotThrowAsync();
            await _userRepositoryMock
                .Received(1)
                .UpdateAsync(Arg.Is<User>(x =>
                    x.Id == 1
                    && x.CompanyId == 1
            ));
        }
    }
}