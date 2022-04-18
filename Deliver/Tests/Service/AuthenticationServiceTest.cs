using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using Models.Exceptions;
using Models.Request.Authentication;
using Models.Request.User;
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

namespace Tests.Service
{
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
                Surname = "TestSurname"
            }
        }.BuildMock();
        #endregion

        private readonly IUserRepository _userRepositoryMock;
        private readonly IJwtUtils _jwtUtilsMock;
        private readonly IAuthenticationService _service;

        public AuthenticationServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _userRepositoryMock.GetAll().Returns(_userDataMock);

            _jwtUtilsMock = Substitute.For<IJwtUtils>();

            _service = new AuthenticationService(_jwtUtilsMock, _userRepositoryMock);
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
}
