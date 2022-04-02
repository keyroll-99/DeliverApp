using Integrations.Interface;
using Microsoft.Extensions.Options;
using Models.Intefrations;
using System.Net;
using System.Net.Mail;

namespace Integrations.Impl;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly SmtpClient _smtpClient;


    public MailService(IOptions<MailSettings> mailSettings, SmtpClient smtpClient)
    {
        _mailSettings = mailSettings.Value;
        _smtpClient = smtpClient;
    }

    public async Task<bool> SendWelcomeMessage(WelcomeMessageModel welcomeMessageModel)
    {
        var message = new MailMessage();
        message.From = new MailAddress(_mailSettings.Login);
        message.To.Add(new MailAddress(welcomeMessageModel.Email));
        message.Subject = "Wlecome in deliver app";
        message.Body =
            $@"<h1> Welcome {welcomeMessageModel.Name} {welcomeMessageModel.Surname} </h1> <br /> 
            Your loing {welcomeMessageModel.Username} <br />
            Your password {welcomeMessageModel.Password} <br />
            <h3>Remeber you should change password after first login</h3>";
        message.IsBodyHtml = true;
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
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
}
