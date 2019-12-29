using Allie.Chat.Commands.Core.Interfaces;

namespace Allie.Chat.Commands.Core.Auth.Interfaces
{
    public interface IParametersAuthPKCE : IParameters
    {
        string ClientId { get; set; }
        string Scopes { get; set; }
    }
}
