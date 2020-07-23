using Allie.Chat.Commands.Service.Interfaces;

namespace Allie.Chat.Commands.Service.Auth.Interfaces
{
    public interface IParametersAuthCode : IParameters
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
    }
}
