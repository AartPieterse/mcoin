using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Transaction
    {
        public Byte[] Hash { get; set; }

        public Int32 Version { get; set; }

        public Int64 TotalInputValue { get; set; }

        public IEnumerable<SubTx> VIn { get; set; }

        public Int64 TotalOutputValue { get; set; }

        public IEnumerable<SubTx> VOut { get; set; }

        public Int32 LockTime { get; set; }

        public Transaction() { }

        public Transaction(Byte[] hash, Int32 version, Int64 totalinput, IEnumerable<SubTx> vin, Int64 totaloutput, IEnumerable<SubTx> vout, Int32 locktime = 0)
        {
            this.Hash = hash;
            this.Version = version;
            this.TotalInputValue = totalinput;
            this.VIn = vin;
            this.TotalOutputValue = totaloutput;
            this.VOut = vout;
            this.LockTime = locktime;
        }
    }
}
