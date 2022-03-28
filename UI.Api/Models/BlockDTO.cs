using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Api.Models
{
    public class BlockDTO
    {
        public long Timestamp { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
        public int Nonce { get; set; }

    }
}
