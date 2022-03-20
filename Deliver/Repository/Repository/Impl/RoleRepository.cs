using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }
}
