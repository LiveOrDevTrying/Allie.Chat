using Allie.Chat.WebAPI.Auth;

namespace Allie.Chat.Commands.Core.Auth
{
    public abstract class BaseCommandsAuthService : BaseCommandsService, ICommandsService
    {
        protected new readonly IWebAPIClientACAuth _webapiClient;

        public BaseCommandsAuthService(string webapiToken, int pollingIntervalMS, IWebAPIClientACAuth webapiAuthClient) 
            : base(webapiToken, pollingIntervalMS, webapiAuthClient)
        {
        }
    }
}
