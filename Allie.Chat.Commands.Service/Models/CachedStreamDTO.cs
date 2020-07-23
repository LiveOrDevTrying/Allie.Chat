using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Commands.Service.Models
{
    public struct CachedStreamDTO
    {
        public StreamDTO Stream { get; set; }
        public CachedCommandSetDTO[] CommandSets { get; set; }
    }
}
