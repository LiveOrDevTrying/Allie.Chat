namespace Allie.Chat.Commands.Websocket.Auth.Interfaces
{
    public interface IParametersWebsocketAuthROPassword : IParametersCommandsBaseWebsocketAuth
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
