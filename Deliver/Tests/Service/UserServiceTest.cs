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
using Models.Request.Utils;
using Models.Request.Utils.Role;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service
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

        private readonly LoggedUser _loggedUser = new()
        {
            Id = 1,
            Roles = new List<string> { SystemRoles.Admin }
        };

        #endregion

        private readonly IUserRepository _userRepositoryMock;
        private readonly ICompanyUtils _companyUtilsMock;
        private readonly IOptions<LoggedUser> _optionsMock;
        private readonly IRoleUtils _roleUtilsMock;
        private readonly IMailService _mailServiceMock;

        private readonly UserService _service;

        public UserServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _userRepositoryMock.GetAll().Returns(_userDataMock);

            _companyUtilsMock = Substitute.For<ICompanyUtils>();
            _companyUtilsMock.IsUserCompany(Arg.Any<Guid>(), Arg.Any<long>()).Returns(true);

            _optionsMock = Substitute.For<IOptions<LoggedUser>>();
            _optionsMock.Value.Returns(_loggedUser);

            _roleUtilsMock = Substitute.For<IRoleUtils>();

            _mailServiceMock = Substitute.For<IMailService>();

            _service = new UserService(
                _userRepositoryMock,
                _companyUtilsMock,
                _optionsMock,
                _roleUtilsMock,
                _mailServiceMock
            );
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
            Func<Task> act = async () => await _service.CreateUser(request);

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
        }

        [Fact]
        public async Task CreateUser_WhenUserDoesntHavaPermisson_ReturnsError()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test2",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test2"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

            // act
            Func<Task> act = async () => await _service.CreateUser(request);

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
        }


        [Fact]
        public async Task CreateUser_WhenCompanyDoesntExists_ReturnsError()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test2",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test2"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(null as Company);

            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

            // act
            Func<Task> act = async () => await _service.CreateUser(request);

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.CompanyDoesntExists);
        }

        [Fact]
        public async Task CreateUser_WhenRequestIsValid_CreateUserAndSendEmail()
        {
            // arrange
            var request = new CreateUserRequest
            {
                CompanyHash = Guid.NewGuid(),
                Name = "test2",
                Email = "test@test.com",
                PhoneNumber = "11-11-11",
                RoleIds = new List<long> { 1 },
                Surname = "test",
                Username = "test2"
            };

            _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });
            _companyUtilsMock.GetCompanyByHash(Arg.Any<Guid>()).Returns(new Company
            {
                Hash = Guid.NewGuid(),
            });

            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

            // act
            var response = await _service.CreateUser(request);

            // assert
            response.Should().NotBeNull();

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

        [Fact]
        public async Task GetUser_WhenUserDoesntExist_ThenThrowError()
        {
            // act
            Func<Task> act = async () => await _service.GetUser(Guid.NewGuid());

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserDosentExists);
        }

        [Fact]
        public async Task GetUser_WhenUserHasInvalidRole_ThenThrowError()
        {
            // arrange
            var userHash = Guid.Parse("b07b6398-47c7-4e0d-8d5b-27a199ae63a5");
            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

            // act
            Func<Task> act = async () => await _service.GetUser(userHash);

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
        }

        [Fact]
        public async Task FireUser_WhenUserDoenstExists_ThenThrowError()
        {
            // act
            Func<Task> act = async () => await _service.FireUser(Guid.Parse("63165844-94a4-4769-bcef-bcb0692d9d26"));

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserDosentExists);
        }

        [Fact]
        public async Task FireUser_WhenUserIsAdmin_ThenThrowError()
        {
            // act
            Func<Task> act = async () => await _service.FireUser(_userDataMock.First().Hash);

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.CannotModifyAdmin);
        }

        [Fact]
        public async Task FireUser_WhenUserDoesntHavePermission_ThenThrowError()
        {
            // arranre
            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

            // act
            Func<Task> act = async () => await _service.FireUser(Guid.Parse("b07b6398-47c7-4e0d-8d5b-27a199ae63a5"));

            // assert
            await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
        }

        [Fact]
        public async Task FireUser_WhenRequestIsValid_ThenUpdateFireFlagOnUser()
        {
            // arranre
            _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

            // act
            await _service.FireUser(Guid.Parse("b07b6398-47c7-4e0d-8d5b-27a199ae63a5"));

            // assert
            await _userRepositoryMock.Received(1).UpdateAsync(Arg.Is<User>(x => x.IsFired == true));
        }
    }
}