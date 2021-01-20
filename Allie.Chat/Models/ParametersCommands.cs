using Allie.Chat.Interfaces;

namespace Allie.Chat.Models
{
    public struct ParametersCommands : IParameters
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
    }
}
