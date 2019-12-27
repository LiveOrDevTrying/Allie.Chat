using Allie.Chat.Commands.Websocket.Auth.Interfaces;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket.Auth
{
    public class CommandsWebsocketAuthROPassword : BaseCommandsWebsocketAuth
    {
        protected readonly new IParametersWebsocketAuthROPassword _parameters;
        protected TokenResponse _tokenResponse;
        protected DateTime _expireTime;

        public CommandsWebsocketAuthROPassword(IParametersWebsocketAuthROPassword parameters) 
            : base(parameters)
        {
        }

        protected override async Task GetAccessTokenAsync()
        {
            _tokenResponse = await _webapiClient.GetAccessTokenResourceOwnerPasswordAsync(
                _parameters.ClientId, _parameters.ClientSecret, _parameters.Scopes, _parameters.Username, _parameters.Password);

            if (_tokenResponse != null)
            {
                if (!string.IsNullOrWhiteSpace(_tokenResponse.AccessToken))
                {
                    _expireTime = DateTime.UtcNow + TimeSpan.FromSeconds(_tokenResponse.ExpiresIn * 0.8f);
                    UpdateWebAPIToken(_tokenResponse.AccessToken);
                    await ConnectAsync();
                }
            }
        }
        protected override async Task<bool> IsTokenValidAsync()
        {
            if (_expireTime <= DateTime.UtcNow)
            {
                await GetAccessTokenAsync();
            }

            return await base.IsTokenValidAsync();
        }
    }
}
