using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(AppDbContext context) : base(context)
    {
    }
}
