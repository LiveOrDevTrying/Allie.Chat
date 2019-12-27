namespace Allie.Chat.Commands.Tcp.Auth.Interfaces
{
    public interface IParametersTcpAuthPKCE : IParametersBaseTcpAuth
    {
        string ClientId { get; set; }
        string Scopes { get; set; }
    }
}
