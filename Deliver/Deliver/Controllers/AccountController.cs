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

        [HttpPost("Password-recovery/Init")]
        [AllowAnonymous]
        public async Task<bool> PasswordRecoveryInit(PasswordRecoveryInitRequest request)
        {
            await _accountService.InitRecoveryPassword(request);
            return true;
        }

        [HttpPost("Password-recovery/Change")]
        public async Task<bool> PasswordRecoveryChange(PasswordRecoverySetNewPasswordRequest request)
        {
            await _accountService.SetNewPassword(request);
            return true;
        }

        [HttpGet("Password-recovery/Valid/{recoveryKey}")]
        [AllowAnonymous]
        public async Task<bool> IsValidRecoveryKey(string recoveryKey)
        {
            return await _accountService.IsValidRecoveryKey(recoveryKey);
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
