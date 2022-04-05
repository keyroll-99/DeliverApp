using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl.Utils;
using Services.Interface.Utils;
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
            Surname = "TestSurname"
        }
    }.BuildMock();

    #endregion

    private readonly IUserRepository _userRepository;
    private IUserUtils _service;

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
}
