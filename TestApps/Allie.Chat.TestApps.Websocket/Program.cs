using Allie.Chat.Events.Args;
using Allie.Chat.Models;
using Allie.Chat.WebAPI;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.TestApps.Websocket
{
    class Program
    {
        private static ACCommands _acCommands;

        static void Main(string[] args)
        {
            var accessToken = "c88cbdcbce25497d8ba9edb71a97f06cd3e1fda419bc4adfb42cfc283f0bb29d";
            _acCommands = new ACCommands(new ACParametersAuthCode
            {
                BotAccessToken = accessToken,
                ClientId = "auth.code",
                ClientSecret = "secret",
                Scopes = "openid profile Allie.Chat.WebAPI",
                StreamCachePollingIntervalMS = 45000,
                ReconnectPollingIntervalMS = 15000,
                ClientType = Enums.ClientType.Websocket
            });
            _acCommands.CommandEvent += OnCommandEvent;

            _acCommands.StartAsync().Wait();

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void OnCommandEvent(object sender, CommandEventArgs args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(args));
        }
    }
}
