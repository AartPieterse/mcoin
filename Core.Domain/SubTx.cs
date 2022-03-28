using System;

namespace Core.Domain
{
    public class SubTx
    {
        public Byte[] TxHash { get; set; }

        public Int32 OutItemNr { get; set; }

        public Int32 InItemNr { get; set; }

        public String Address { get; set; }

        public Int64 Amount { get; set; }

        public String Signature { get; set; }

        public Boolean Spendable { get; set; }

        public SubTx() { }

        public SubTx(Byte[] txhash, Int32 innr, Int32 outnr, String address, Int64 amount, String signature, Boolean spendable)
        {
            this.TxHash = txhash;
            this.InItemNr = innr;
            this.OutItemNr = outnr;
            this.Address = address;
            this.Amount = amount;
            this.Signature = signature;
            this.Spendable = spendable;
        }
    }
}
