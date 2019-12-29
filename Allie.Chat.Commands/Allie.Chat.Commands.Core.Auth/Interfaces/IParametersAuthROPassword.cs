using Allie.Chat.Commands.Core.Interfaces;

namespace Allie.Chat.Commands.Core.Auth.Interfaces
{
    public interface IParametersAuthROPassword : IParameters
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }

        string Scopes { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
