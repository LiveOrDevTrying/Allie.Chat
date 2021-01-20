using Allie.Chat.Interfaces;

namespace Allie.Chat.Models
{
    public struct ParametersCommandsToken : IParametersToken
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
    }
}
