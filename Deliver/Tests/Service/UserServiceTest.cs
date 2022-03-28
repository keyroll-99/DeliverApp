using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using Models.Request.User;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service
{
    public class UserServiceTest
    {
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

        private readonly IUserRepository _userRepositoryMock;
        private readonly IJwtUtils _jwtUtilsMock;

        private readonly UserService _service;

        public UserServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _userRepositoryMock.GetAll().Returns(_userDataMock);

            _jwtUtilsMock = Substitute.For<IJwtUtils>();

            _service = new UserService(_userRepositoryMock, _jwtUtilsMock);
        }

        [Fact]
        public async Task GetById_WhenUserExists_ReturnsUser()
        {
            // arrange
            var request = 1;
            _userRepositoryMock.GetByIdAsync(Arg.Any<long>()).Returns(new User { Id = request });

            // act
            var response = await _service.GetById(request);

            // assert
            response.Should().BeOfType<User>();
            response.Id.Should().Be(1);
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
            var refreshToken = Guid.NewGuid().ToString();

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
            _jwtUtilsMock.GetRefreshTokenByToken(Arg.Any<string>()).Returns((RefreshToken)null);

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
    }
}