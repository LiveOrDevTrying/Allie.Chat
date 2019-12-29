using Allie.Chat.Commands.Core.Auth.Interfaces;
using Allie.Chat.WebAPI.Auth;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp.Auth
{
    public class CommandsTcpAuthROPassword : BaseCommandsTcpAuth
    {
        protected new readonly IParametersAuthROPassword _parameters;
        protected TokenResponse _tokenResponse;
        protected DateTime _expireTime;

        public CommandsTcpAuthROPassword(IParametersAuthROPassword parameters) 
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
                    Connect();
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
