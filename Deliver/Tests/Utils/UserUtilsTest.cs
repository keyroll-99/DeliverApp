using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using Models.Exceptions;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl.Utils;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils;

public class UserUtilsTest
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
            Hash = Guid.NewGuid()
        }
    }.BuildMock();

    #endregion

    private readonly IUserRepository _userRepository;
    private readonly IUserUtils _service;

    public UserUtilsTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.GetAll().Returns(_userDataMock);

        _service = new UserUtils(_userRepository);
    }

    [Fact]
    public async Task GetById_WhenUserExists_ReturnUser()
    {
        // act
        var result = await _service.GetById(1);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ChangeUserCompany_WhenValidData_ThenCallUpdate()
    {
        // act
        await _service.ChangeUserCompany(new User { Id = 1 }, new Company { Id = 2 });

        // assert
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(x => x.Id == 1 && x.CompanyId == 2));
    }

    [Fact]
    public async Task GetByHash_WhenUserDoesntExist_ThenThrowError()
    {
        // act
        Func<Task> act = async () => await _service.GetByHash(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.UserDosentExists);
    }

    [Fact]
    public async Task GetByHash_WhenUserExists_ThenReturnUser()
    {
        // arrange
        var user = _userDataMock.First();

        // act
        var result = await _service.GetByHash(user.Hash);

        // assert
        result.Should().BeSameAs(user);
    }
}
