using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Exceptions;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class CompanyUtils : ICompanyUtils
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyUtils(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Company?> GetCompanyByHash(Guid hash) =>
            await _companyRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == hash);

    public async Task<Company> GetUserCompany(long userId)
        => (await _companyRepository
            .GetAll()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Users.Any(u => u.Id == userId))) ?? throw new AppException(ErrorMessage.UserDosentHaveCompany);

    public async Task<bool> IsUserCompany(Guid companyHash, long userId)
    {
        return await _companyRepository.GetAll()
            .Include(x => x.Users)
            .AnyAsync(x => x.Hash == companyHash && x.Users.Any(u => u.Id == userId));
    }
}
