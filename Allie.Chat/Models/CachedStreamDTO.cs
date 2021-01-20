using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Models
{
    public struct CachedStreamDTO
    {
        public StreamDTO Stream { get; set; }
        public CachedCommandSetDTO[] CommandSets { get; set; }
    }
}
