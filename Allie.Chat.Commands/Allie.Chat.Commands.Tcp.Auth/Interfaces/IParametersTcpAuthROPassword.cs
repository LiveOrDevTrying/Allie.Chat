namespace Allie.Chat.Commands.Tcp.Auth.Interfaces
{
    public interface IParametersTcpAuthROPassword : IParametersBaseTcpAuth
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }

        string Scopes { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
