using Models.Integration;

namespace Integrations.Interface;

public interface IMailService
{
    Task<bool> SendWelcomeMessage(WelcomeMessageModel welcomeMessageModel);
}
