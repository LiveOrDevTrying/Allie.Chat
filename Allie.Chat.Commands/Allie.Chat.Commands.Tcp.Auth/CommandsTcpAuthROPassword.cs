using Allie.Chat.Commands.Tcp.Auth.Interfaces;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp.Auth
{
    public class CommandsTcpAuthROPassword : BaseCommandsTcpAuth
    {
        protected new readonly IParametersTcpAuthROPassword _parameters;
        protected TokenResponse _tokenResponse;
        protected DateTime _expireTime;

        public CommandsTcpAuthROPassword(IParametersTcpAuthROPassword parameters) 
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
                    Connect();
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
