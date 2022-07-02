using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class PasswordRecoveryRepository : BaseRepository<PasswordRecovery>, IPasswordRecoveryRepository
{
    public PasswordRecoveryRepository(AppDbContext context) : base(context)
    {
    }
}
