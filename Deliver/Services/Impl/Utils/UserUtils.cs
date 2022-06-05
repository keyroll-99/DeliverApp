using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Exceptions;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class UserUtils : IUserUtils
{
    private readonly IUserRepository _userRepository;

    public UserUtils(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ChangeUserCompany(User user, Company company)
    {
        user.CompanyId = company.Id;
        user.Company = company;
        await _userRepository.UpdateAsync(user);
    }

    public async Task<User> GetByHash(Guid hash)
    {
        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == hash);
      
        if(user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        return user!;
    }

    public async Task<User?> GetById(long id) =>
        await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(y => y.Role)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<User>> GetUsersFromCompany(Company company) =>
        await _userRepository
            .GetAll()
            .Where(x => x.CompanyId == company.Id)
            .ToListAsync();
}
