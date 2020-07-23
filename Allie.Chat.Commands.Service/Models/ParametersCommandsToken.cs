using Allie.Chat.Commands.Service.Interfaces;

namespace Allie.Chat.Commands.Service.Models
{
    public struct ParametersCommandsToken : IParametersToken
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
    }
}
