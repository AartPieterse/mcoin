using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UI.Console.Controllers
{
    public class WalletController
    {
        private Wallet _userWallet;
        private readonly IWalletRepository _walletRepository;
        private readonly IUTXORepository _utxoRepository;

        public WalletController(IWalletRepository walletrepo, IUTXORepository utxorepo)
        {
            this._walletRepository = walletrepo;
            this._utxoRepository = utxorepo;

            this._userWallet = this._walletRepository.GetWallet();
        }

        private Boolean GiveWarning()
        {
            Printer.PrintText("With creating a new wallet, the old wallet will be overwritten and your funds will be lost.");
            Printer.PrintText("Are you sure you will create a new wallet? Type 'new' to create one, or hit enter to cancel.");

            if (Printer.Listen() != "new")
                return false;

            return true;
        }

        public void SetupWallet()
        {
            if (this.GiveWarning() != true)
                return;

            Printer.PrintText("Type your preferred walletaddress: ");
            String publickey = Printer.Listen();

            if (publickey != null)
            {
                publickey = Printer.GenerateRandomString(32);
            }

            Printer.PrintText("Generating private key...");
            String privatekey = Printer.GenerateRandomString(64);
            Thread.Sleep(2000);


            Printer.PrintText("Saving new wallet...");
            this._userWallet = new Wallet(publickey, privatekey, 0);
            this._walletRepository.SaveWallet(this._userWallet);
        }

        public Wallet GetWallet()
        {
            //this.RecalculateBalance();

            return this._userWallet;
        }

        private void RecalculateBalance()
        {
            Wallet wallet = this._walletRepository.GetWallet();
            UInt64 realBalance = 0;

            List<SubTx> utxoList = this._utxoRepository.GetAllUTXO(wallet.PublicKey).ToList();

            foreach (SubTx subTx in utxoList)
            {
                if (subTx.Address == wallet.PublicKey)
                    realBalance += (UInt64)subTx.Amount;
            }

            if (wallet.Balance == realBalance)
                return;

            wallet.Balance = realBalance;
            this._walletRepository.SaveWallet(wallet);
        }
    }
}
