using Core.Domain;
using Core.DomainServices;
using System;

namespace Infrastructure
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletFileContext _context;

        public WalletRepository(WalletFileContext context)
        {
            this._context = context;
        }

        public void SaveWallet(Wallet wallet)
        {
            this._context.AddWallet(wallet);
        }

        public Wallet GetWallet()
        {
            return this._context.GetWallet();
        }

        public Boolean HasWallet()
        {
            return this._context.HasWallet;
        }
    }
}
