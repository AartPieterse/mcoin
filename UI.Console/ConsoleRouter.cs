using Core.Domain;
using System;
using UI.Console.Controllers;

namespace UI.Console
{
    public class ConsoleRouter
    {
        private readonly AuthController _authController;
        private readonly BlockchainController _blockchainController;
        private readonly MempoolController _mempoolController;
        private readonly TransactionController _txController;
        private readonly WalletController _walletController;
        private readonly Miner _miner;

        public ConsoleRouter(AuthController authController, BlockchainController blockchainController, MempoolController mempoolController, WalletController walletController, TransactionController txController, Miner miner)
        {
            this._authController = authController;
            this._blockchainController = blockchainController;
            this._mempoolController = mempoolController;
            this._walletController = walletController;
            this._txController = txController;
            this._miner = miner;
        }

        public void StartProcess()
        {
            Printer.PrintText("Welcome!");

            String command;
            while (true)
            {
                Printer.AskForCommand();
                command = Printer.Listen().ToLower();

                switch (command)
                {
                    case "help":
                        Help();
                        break;
                    case "mine":
                        this.StartMiner();
                        break;
                    case "wallet balance":
                        this.GetWalletBalance();
                        break;
                    case "wallet details":
                        this.GetWalletDetails();
                        break;
                    case "wallet new":
                        this.CreateWallet();
                        break;
                    case "tx show list":
                        this.ShowUTXOs();
                        break;
                    case "tx new":
                        this.NewTransaction();
                        break;
                    case "show blockchain":
                        this.PrintBlockchain();
                        break;
                    case "show mempool":
                        this.PrintMempool();
                        break;
                    case "password new":
                        this.SetPassword();
                        break;
                    case "exit":
                        this.Exit();
                        break;

                    default:
                        Printer.PrintText("Unknown command");
                        break;
                }
            }
        }

        private static void Help()
        {
            Printer.PrintAvailableCommands();
        }

        private void StartMiner()
        {
            this._miner.Mine();
        }

        private void GetWalletBalance()
        {
            Printer.PrintText("{0} Mooncoin", this._walletController.GetWallet().Balance.ToString());
        }

        private void GetWalletDetails()
        {
            Wallet wallet = this._walletController.GetWallet();

            if (wallet == null)
            {
                Printer.PrintText("No wallet found, type 'wallet new' to create one");
            }
            else
            {
                Printer.PrintText(" > Public key: {0}", wallet.PublicKey);
                Printer.PrintText(" > Private key: {0}", wallet.PrivateKey);
                Printer.PrintText(" > Balance: {0} Mooncoin", wallet.Balance.ToString());
            }
        }

        private void CreateWallet()
        {
            this._walletController.SetupWallet();
        }

        private void ShowUTXOs()
        {
            this._txController.PrintUTXOs();
        }

        private void NewTransaction()
        {
            this._txController.NewTransaction();
        }

        private void PrintBlockchain()
        {
            Printer.PrintChain(this._blockchainController.GetBlockchain());
        }

        private void PrintMempool()
        {
            Printer.PrintTransactions(this._mempoolController.GetMempool());
        }

        private void SetPassword()
        {
            this._authController.SetNewPassword();
        }

        private void Exit()
        {
            Printer.PrintText("\nGoodbye! \n\n");

            Environment.Exit(1);
        }
    }
}
