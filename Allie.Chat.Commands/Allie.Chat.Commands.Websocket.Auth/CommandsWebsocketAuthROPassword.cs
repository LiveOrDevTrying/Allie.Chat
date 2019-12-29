using Allie.Chat.Commands.Core.Auth.Interfaces;
using Allie.Chat.WebAPI.Auth;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket.Auth
{
    public class CommandsWebsocketAuthROPassword : BaseCommandsWebsocketAuth
    {
        protected readonly new IParametersAuthROPassword _parameters;
        protected TokenResponse _tokenResponse;
        protected DateTime _expireTime;

        public CommandsWebsocketAuthROPassword(IParametersAuthROPassword parameters) 
            : base(parameters)
        {
            _parameters = parameters;
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
                    await UpdateEventsAsync(0);
                    await ConnectAsync();
                }
            }
        }
        protected override async Task ValidateTokenAsync()
        {
            if (_expireTime <= DateTime.UtcNow)
            {
                await GetAccessTokenAsync();
            }
        }
    }
}
