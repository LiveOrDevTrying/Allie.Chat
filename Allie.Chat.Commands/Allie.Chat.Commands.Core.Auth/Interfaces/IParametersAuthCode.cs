using Allie.Chat.Commands.Core.Interfaces;

namespace Allie.Chat.Commands.Core.Auth.Interfaces
{
    public interface IParametersAuthCode : IParameters
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
    }
}
