using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class LogRepository : BaseRepository<Log>, ILogRepository
{
    public LogRepository(AppDbContext context) : base(context)
    {
    }
}
