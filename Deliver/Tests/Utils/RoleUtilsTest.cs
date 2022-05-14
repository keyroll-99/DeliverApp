using FluentAssertions;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.Utils;
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
    private readonly IRolePermissionRepository _rolePermissionRepository;

    private readonly IRoleUtils _service;

    public RoleUtilsTest()
    {
        _roleRepository = Substitute.For<IRoleRepository>();
        _roleRepository.GetAll().Returns(_rolesDataMock);

        _rolePermissionRepository = Substitute.For<IRolePermissionRepository>();

        _userRoleRepository = Substitute.For<IUserRoleRepository>();

        _service = new RoleUtils(_roleRepository, _userRoleRepository, _rolePermissionRepository);
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
