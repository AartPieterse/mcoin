using System.Collections.Generic;

namespace UI.Webservice.Models
{
    public class BlockModel
    {
        public long Timestamp { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        public List<TransactionModel> Transactions { get; set; }
        public int Nonce { get; set; }
    }
}
