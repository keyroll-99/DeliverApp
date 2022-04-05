using FluentAssertions;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Request.utils;
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

public class RoleUtilsTest
{
    #region mock

    private readonly IQueryable<Role> _rolesDataMock = new List<Role>
    {
        new Role
        {
            Id = 1,
            Name = "test",
        }
    }.BuildMock();

    #endregion

    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    private readonly IRoleUtils _service;

    public RoleUtilsTest()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _roleRepository.GetAll().Returns(_rolesDataMock);

        _userRoleRepository = Substitute.For<IUserRoleRepository>();

        _service = new RoleUtils(_roleRepository, _userRoleRepository);
    }

    [Fact]
    public void HasPermissionToAddUser_WhenLoggedUserIsNull_ThorwArgumentException()
    {
        // arrange
        var reguest = new HasPermissionToAddUserRequest();

        // act
        Action act = () => _service.HasPermissionToAddUser(reguest);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(SystemRoles.HR, "d8c5ae68-56bc-4ba2-8359-ee5490bf581e", "cddc1c81-1dad-4c0b-a432-b19b25f6ac35")]
    [InlineData(SystemRoles.CompanyAdmin, "d8c5ae68-56bc-4ba2-8359-ee5490bf581e", "cddc1c81-1dad-4c0b-a432-b19b25f6ac35")]
    [InlineData(SystemRoles.Driver, "cddc1c81-1dad-4c0b-a432-b19b25f6ac35", "cddc1c81-1dad-4c0b-a432-b19b25f6ac35")]
    public void HasPermissionToAddUser_WhenUserDoesntHavePerrmisionOrIsNotInThisSameCompany_ReturnFalse(string userRole, string userCompanyHash, string targetCompanyHash)
    {
        // arrange
        var reguest = new HasPermissionToAddUserRequest
        {
            LoggedUser = new LoggedUser { Roles = new List<string> { userRole } },
            TargetCompanyHash = Guid.Parse(targetCompanyHash),
            LoggedUserCompany = new Company
            {
                Hash = Guid.Parse(userCompanyHash)
            }
        };

        // act
        var response = _service.HasPermissionToAddUser(reguest);

        // assert
        response.Should().BeFalse();
    }

    [Theory]
    [InlineData(SystemRoles.Admin)]
    [InlineData(SystemRoles.HR)]
    [InlineData(SystemRoles.CompanyAdmin)]
    public void HasPermissionToAddUser_WhenUserHavePerrmision_ReturnTrue(string userRole)
    {
        // arrange
        var hash = Guid.NewGuid();
        var request = new HasPermissionToAddUserRequest
        {
            LoggedUser = new LoggedUser
            {
                Id = 1,
                Roles = new List<string> { userRole }
            },
            TargetCompanyHash = hash,
            LoggedUserCompany = new Company
            {
                Hash = hash,
            }
        };

        // act
        var response = _service.HasPermissionToAddUser(request);

        // act
        response.Should().BeTrue();
    }

    [Fact]
    public async Task AddRolesToUser_WhenRolesExists_AddRolesToUser()
    {
        // arrange
        var user = new User
        {
            Id = 10,
        };

        var rolesIds = new List<long> { 1 };

        // act
        await _service.AddRolesToUser(user, rolesIds);

        // assert
        await _userRoleRepository
            .Received(1)
            .AddAsync(Arg.Is<UserRole>(x =>
                x.UserId == 10
                && x.RoleId == 1
            ));
    }
}
