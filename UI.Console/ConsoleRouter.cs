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

        public ConsoleRouter(AuthController authController, BlockchainController blockchainController, MempoolController mempoolController, WalletController walletController, TransactionController txController)
        {
            this._authController = authController;
            this._blockchainController = blockchainController;
            this._mempoolController = mempoolController;
            this._walletController = walletController;
            this._txController = txController;
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
                        StartMiner();
                        break;
                    case "wallet balance":
                        GetWalletBalance();
                        break;
                    case "wallet details":
                        GetWalletDetails();
                        break;
                    case "wallet new":
                        CreateWallet();
                        break;
                    case "tx show list":
                        ShowUTXOs();
                        break;
                    case "tx new":
                        NewTransaction();
                        break;
                    case "show blockchain":
                        PrintBlockchain();
                        break;
                    case "show mempool":
                        PrintMempool();
                        break;
                    case "password new":
                        SetPassword();
                        break;
                    case "exit":
                        Exit();
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

        private static void StartMiner()
        {
            Miner.StartMinerProgram();
        }

        private void GetWalletBalance()
        {
            Printer.PrintText("{0} Mooncoin", _walletController.GetWallet().Balance.ToString());
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
            _walletController.SetupWallet();
        }

        private void ShowUTXOs()
        {
            _txController.PrintUTXOs();
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

        private static void Exit()
        {
            Printer.PrintText("\nGoodbye! \n\n");

            Environment.Exit(1);
        }
    }
}
