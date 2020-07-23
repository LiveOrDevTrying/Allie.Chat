using Allie.Chat.Commands.Service.Interfaces;

namespace Allie.Chat.Commands.Service.Auth.Interfaces
{
    public interface IParametersAuthPKCE : IParameters
    {
        string ClientId { get; set; }
        string Scopes { get; set; }
    }
}
