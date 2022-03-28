using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Webservice.Models
{
    public class BlockchainModel
    {
        public List<BlockModel> Chain { get; set; }
        public List<TransactionModel> PendingTransactions { get; set; }
        public int Difficulty { get; set; }
        public int MiningReward { get; set; }
    }
}
