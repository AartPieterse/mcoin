using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Api.Models
{
    public class BlockchainDTO
    {
        public List<BlockDTO> Chain { get; set; }
        public List<TransactionDTO> PendingTransactions { get; set; }
        public int Difficulty { get; set; }
        public int MiningReward { get; set; }

    }
}
