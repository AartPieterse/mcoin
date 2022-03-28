using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Blockchain
    {
        public List<Block> Blocks { get; set; }

        public Byte[] Difficulty { get; set; }

        public Int32 MiningReward { get; set; }
    }
}
