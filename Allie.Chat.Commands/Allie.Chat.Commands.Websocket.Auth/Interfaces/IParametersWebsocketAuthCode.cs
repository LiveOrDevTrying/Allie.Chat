using System;
using System.Collections.Generic;
using System.Text;

namespace Allie.Chat.Commands.Websocket.Auth.Interfaces
{
    public interface IParametersWebsocketAuthCode : IParametersCommandsBaseWebsocketAuth
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string Scopes { get; set; }
    }
}
