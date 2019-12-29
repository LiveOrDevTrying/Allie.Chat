using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.WebAPI;

namespace Allie.Chat.Commands.Core
{
    public abstract class BaseCommandsTokenService : BaseCommandsService
    {
        protected new IParametersToken _parameters;

        public BaseCommandsTokenService(IParametersToken parameters)
            : base(parameters)
        {
            _parameters = parameters;
        }    
    }
}
