using Allie.Chat.Lib.DTOs.Commands;

namespace Allie.Chat.Commands.Core.Models
{
    public struct CachedCommandSetDTO
    {
        public CommandSetDTO CommandSet { get; set; }
        public CommandDTO[] Commands { get; set; }
    }
}
