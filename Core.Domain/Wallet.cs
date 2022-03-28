using System;

namespace Core.Domain
{
    public class Wallet
    {
        public String PublicKey { get; set; }

        public String PrivateKey { get; set; }

        public UInt64 Balance { get; set; }

        public Wallet() { }

        public Wallet(String publickey, String privatekey, UInt64 balance)
        {
            this.PublicKey = publickey;
            this.PrivateKey = privatekey;
            this.Balance = balance;
        }
    }
}
