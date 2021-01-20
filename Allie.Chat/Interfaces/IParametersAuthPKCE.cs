using Allie.Chat.Interfaces;

namespace Allie.Chat.Auth.Interfaces
{
    public interface IParametersAuthPKCE : IParameters
    {
        string ClientId { get; set; }
        string Scopes { get; set; }
    }
}
