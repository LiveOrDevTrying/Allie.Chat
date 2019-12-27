namespace Allie.Chat.Commands.Tcp.Auth.Interfaces
{
    public interface IParametersTcpAuthCode : IParametersBaseTcpAuth
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
    }
}
