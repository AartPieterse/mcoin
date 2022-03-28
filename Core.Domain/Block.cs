using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Block
    {
        // Header
        public Byte[] Hash { get; set; }

        public Int32 Version { get; set; }

        public Byte[] PreviousHash { get; set; }

        public Byte[] Merkleroot { get; set; }

        public Byte[] Timestamp { get; set; }

        public Byte[] Target { get; set; }

        public Int32 Nonce { get; set; } = 0;

        // Data
        public Int32 TxCounter { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }

        public Block() { }

        public Block(Byte[] hash, Int32 version, Byte[] previousHash, Byte[] merkleRoot, Byte[] timestamp, Byte[] target, Int32 nonce, IEnumerable<Transaction> transactions)
        {
            this.Hash = hash;
            this.Version = version;
            this.PreviousHash = previousHash;
            this.Merkleroot = merkleRoot;
            this.Timestamp = timestamp;
            this.Target = target;
            this.Nonce = nonce;
            this.Transactions = transactions;
        }
    }
}
