using Integrations.Interface;
using Microsoft.Extensions.Options;
using Models;
using Models.Integration;
using System.Net;
using System.Net.Mail;

namespace Integrations.Impl;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly AppSettings _appSettings;
    private readonly SmtpClient _smtpClient;


    public MailService(IOptions<MailSettings> mailSettings, IOptions<AppSettings> appSettgins,SmtpClient smtpClient)
    {
        _mailSettings = mailSettings.Value;
        _smtpClient = smtpClient;
        _appSettings = appSettgins.Value;
    }

    public async Task SendPasswordRecoveryMessage(PasswordRecoveryMessageModel passwordRecoveryMessageModel)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_mailSettings.Login),
            Subject = "Deliver app password recovery",
            Body = $@"Hi, <br/> 
                    It is your password recovery link: <a href='{_appSettings.FrontAppUrl}/password-recovery/{passwordRecoveryMessageModel.RecoveryLink}'>{_appSettings.FrontAppUrl}/password-recovery/{passwordRecoveryMessageModel.RecoveryLink}</a> <br/>
                    If you didn't fill up password recovery form, please contact with us <a href='mailto:{_mailSettings.Login}'>{_mailSettings.Login}</a>",
            IsBodyHtml = true
        };
        message.To.Add(passwordRecoveryMessageModel.Email);

        await SendMail(message);
    }

    public async Task<bool> SendWelcomeMessage(WelcomeMessageModel welcomeMessageModel)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_mailSettings.Login),
            Subject = "Wlecome in deliver app",
            Body = $@"<h1> Welcome {welcomeMessageModel.Name} {welcomeMessageModel.Surname} </h1> <br /> 
            Your loing {welcomeMessageModel.Username} <br />
            Your password {welcomeMessageModel.Password} <br />
            <h3>Remeber you should change password after first login</h3>",
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(welcomeMessageModel.Email));

        return await SendMail(message);
    }

    private async Task<bool> SendMail(MailMessage message)
    {
        _smtpClient.Port = 587;
        _smtpClient.Host = "smtp-mail.outlook.com";

        _smtpClient.UseDefaultCredentials = false;
        _smtpClient.Credentials = new NetworkCredential
        {
            UserName = _mailSettings.Login,
            Password = _mailSettings.Password,
        };
        _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        _smtpClient.EnableSsl = true;
        try
        {
            await _smtpClient.SendMailAsync(message);

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
}
