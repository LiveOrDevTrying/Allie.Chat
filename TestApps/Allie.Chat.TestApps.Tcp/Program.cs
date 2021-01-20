using Allie.Chat.Events.Args;
using Allie.Chat.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.TestApps.Tcp
{
    class Program
    {

        static void Main(string[] args)
        {
            var commandsAuthCode = new ACCommands(new ACParametersAuthCode
            {
                BotAccessToken = "cda0f5e754214a15bad4ce13fecf81658c8ce36e2ade42d7be63991194195ba2",
                ClientId = "auth.code",
                ClientSecret = "secret",
                Scopes = "openid profile Allie.Chat.WebAPI",
                StreamCachePollingIntervalMS = 45000,
                ReconnectPollingIntervalMS = 15000
            });
            commandsAuthCode.CommandEvent += OnCommandEvent;

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static Task OnCommandEvent(object sender, CommandEventArgs args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(args));
            return Task.CompletedTask;
        }
    }
}
