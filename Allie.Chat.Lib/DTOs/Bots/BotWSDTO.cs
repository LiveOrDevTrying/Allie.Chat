using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.DTOs.Bots
{
    /// <summary>
    /// A Websocket Bot data-transfer object
    /// </summary>
    public class BotWSDTO : BotDTO
    {
        /// <summary>
        /// The avatar url of the Bot User
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
