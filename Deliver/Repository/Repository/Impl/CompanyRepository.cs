using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class CompanyRepository : BaseHashRepository<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {
    }
}
