using Core.Domain;
using System;

namespace Core.DomainServices
{
    public interface IWalletRepository
    {
        public void SaveWallet(Wallet wallet);

        public Wallet GetWallet();

        public Boolean HasWallet();
    }
}
