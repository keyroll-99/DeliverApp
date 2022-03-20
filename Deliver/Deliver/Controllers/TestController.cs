using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Repository.Repository.Interface;

namespace Deliver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public TestController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<List<User>> Test()
        {
            var x = await _userRepository.GetAll().ToListAsync();
            return x;
        }
    }
}
