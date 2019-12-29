using Allie.Chat.Commands.Core.Auth.Interfaces;
using Allie.Chat.WebAPI.Auth;
using IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket.Auth
{
    public class CommandsWebsocketAuthCode : BaseCommandsWebsocketAuth
    {
        protected new readonly IParametersAuthCode _parameters;
        protected LoginResult _loginResult;
        protected DateTime _expireTime;

        public CommandsWebsocketAuthCode(IParametersAuthCode parameters) 
            : base(parameters)
        { 
            _parameters = parameters;
        }

        protected override async Task GetAccessTokenAsync()
        {
            // R8D: Do this with a refresh token
            _loginResult = await _webapiClient.GetAccessTokenAuthCodeAsync(
                _parameters.ClientId, _parameters.ClientSecret, _parameters.Scopes);

            if (_loginResult != null &&
                !string.IsNullOrWhiteSpace(_loginResult.AccessToken))
            {
                UpdateWebAPIToken(_loginResult.AccessToken);
                await UpdateEventsAsync(0);
                await ConnectAsync();
            }
        }
        protected override async Task ValidateTokenAsync()
        {
            if (_loginResult.AccessTokenExpiration <= DateTime.Now)
            {
                await GetAccessTokenAsync();
            }
        }
    }
}
