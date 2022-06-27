using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Account;
using Models.Response._Core;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Password-recovery/init")]
        [AllowAnonymous]
        public async Task<BaseRespons> PasswordRecoveryInit(PasswordRecoveryRequest request)
        {
            await _accountService.RecoveryPassword(request);
            return BaseRespons.Success();
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<BaseRespons> UpdatePassword(ChangePasswordRequest updatePasswordRequest)
        {
            await _accountService.UpdatePassword(updatePasswordRequest);
            return BaseRespons.Success();
        }
    }
}
