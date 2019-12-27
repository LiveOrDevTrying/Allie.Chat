using Allie.Chat.Lib.DTOs.Streams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Allie.Chat.Commands.Core.Models
{
    public class CachedStreamDTO
    {
        public StreamDTO Stream { get; set; }
        public CachedCommandSetDTO[] CommandSets { get; set; }
    }
}
