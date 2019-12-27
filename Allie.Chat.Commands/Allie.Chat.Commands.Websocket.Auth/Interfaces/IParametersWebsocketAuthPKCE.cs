namespace Allie.Chat.Commands.Websocket.Auth.Interfaces
{
    public interface IParametersWebsocketAuthPKCE : IParametersCommandsBaseWebsocketAuth
    {
        string ClientId { get; set; }
        string Scopes { get; set; }
    }
}
