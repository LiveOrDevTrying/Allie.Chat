using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.WebAPI.Auth;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Core.Auth
{
    public abstract class BaseCommandsAuthService : BaseCommandsService, ICommandsService
    {
        protected new IWebAPIClientACAuth _webapiClient;

        public BaseCommandsAuthService(IParameters parameters) 
            : base(parameters)
        {
            _webapiClient = new WebAPIClientACAuth();
        }

        protected abstract Task ValidateTokenAsync();
        public override void UpdateWebAPIToken(string webAPIToken)
        {
            base.UpdateWebAPIToken(webAPIToken);
            _webapiClient.SetAccessToken(webAPIToken);
        }
    }
}
