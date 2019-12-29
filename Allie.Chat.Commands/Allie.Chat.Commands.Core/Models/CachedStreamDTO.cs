using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Commands.Core.Models
{
    public struct CachedStreamDTO
    {
        public StreamDTO Stream { get; set; }
        public CachedCommandSetDTO[] CommandSets { get; set; }
    }
}
