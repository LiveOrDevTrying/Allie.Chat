using Allie.Chat.Interfaces;

namespace Allie.Chat.Auth.Interfaces
{
    public interface IParametersAuthCode : IParameters
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
    }
}
