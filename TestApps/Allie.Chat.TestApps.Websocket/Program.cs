using Allie.Chat.Events.Args;
using Allie.Chat.Models;
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
            _acCommands = new ACCommands(new ACParametersAuthCode
            {
                BotAccessToken = "c88cbdcbce25497d8ba9edb71a97f06cd3e1fda419bc4adfb42cfc283f0bb29d",
                ClientId = "auth.code",
                ClientSecret = "secret",
                Scopes = "openid profile Allie.Chat.WebAPI",
                StreamCachePollingIntervalMS = 45000,
                ReconnectPollingIntervalMS = 15000
            });
            _acCommands.CommandEvent += OnCommandEvent;

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
