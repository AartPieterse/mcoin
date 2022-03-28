using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Console.Controllers
{
    public class TransactionController
    {
        private readonly IMempoolRepository _mempoolRepo;
        private readonly IUTXORepository _utxoRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly Int32 _version;

        public TransactionController(IMempoolRepository mempoolRepo, IUTXORepository utxoRepo, IWalletRepository walletRepo, Int32 version)
        {
            this._mempoolRepo = mempoolRepo;
            this._utxoRepo = utxoRepo;
            this._walletRepo = walletRepo;
            this._version = version;
        }

        public void PrintUTXOs()
        {
            String address = this._walletRepo.GetWallet().PublicKey;

            List<SubTx> utxoList = this._utxoRepo.GetAllUTXO(address).ToList();

            foreach (SubTx utxo in utxoList)
            {
                Printer.PrintText(" > TxID: {0}", BitConverter.ToString(utxo.TxHash).Replace("-", ""));
                Printer.PrintText(" > InItemNr: {0}", utxo.InItemNr.ToString());
                Printer.PrintText(" > OutItemNr: {0}", utxo.OutItemNr.ToString());
                Printer.PrintText(" > Address: {0}", utxo.Address);
                Printer.PrintText(" > Amount: {0}", utxo.Amount.ToString());
                Printer.PrintText(" > Signature: {0}\n", utxo.Signature);
            }
        }

        public void NewTransaction()
        {
            String userAddress = this._walletRepo.GetWallet().PublicKey;
            List<SubTx> personalUtxoList = this._utxoRepo.GetAllUTXO(userAddress).ToList();

            Transaction newTx = new Transaction();

            // Transaction information: version, vin, totalvin, vout, totalvout, locktime
            Printer.PrintText("Enter some information to complete transaction");

            // Input
            Int32 outputListCounter = 0;
            foreach (SubTx pUtxo in personalUtxoList)
            {
                newTx.TotalInputValue += pUtxo.Amount;

                pUtxo.OutItemNr = outputListCounter;
                outputListCounter++;

                pUtxo.Spendable = false;
            }

            newTx.VIn = personalUtxoList;

            // User created 1 output
            SubTx userOutput = new SubTx();

            Printer.PrintText("Send to address: ");
            userOutput.Address = Printer.Listen();

            Printer.PrintText("Send amount: ");
            userOutput.Amount = int.Parse(Printer.Listen());

            userOutput.InItemNr = -1;
            userOutput.OutItemNr = 0;
            userOutput.Signature = "xxxx";
            userOutput.Spendable = true;

            // Ask for fee
            Printer.PrintText("Pay fee: ");
            Int32 fee = int.Parse(Printer.Listen());

            // System created 1 return output
            SubTx returnOutput = new SubTx();
            returnOutput.Address = userAddress;
            returnOutput.Amount = newTx.TotalInputValue - userOutput.Amount - fee;
            returnOutput.InItemNr = -1;
            returnOutput.OutItemNr = 1;
            returnOutput.Signature = "xxxx";
            returnOutput.Spendable = true;

            // Add user-output, return-output, totaloutput, version and locktime to new transaction
            List<SubTx> outputListForNewTx = new List<SubTx>();

            outputListForNewTx.Add(userOutput);
            outputListForNewTx.Add(returnOutput);
            newTx.VOut = outputListForNewTx;

            newTx.Version = this._version;

            Printer.PrintText("Locktime (s): ");
            newTx.LockTime = int.Parse(Printer.Listen());

            newTx.TotalOutputValue = userOutput.Amount + returnOutput.Amount;

            // Summary
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(newTx);
            Printer.PrintTransactions(transactions);


            //Printer.PrintText("\nOverview: ");
            //Printer.PrintText(" > Protocol version: {0}", newTx.Version);
            //Printer.PrintText(" > Total input: {0} Mooncoin", newTx.TotalInputValue.ToString());
            //Printer.PrintText(" > Spent output: {0}", newTx.TotalOutputValue.ToString());
            //Printer.PrintText(" > Spent fees: {0}", fee);

            //Printer.PrintText("===== Used unspent outputs");
            //foreach (SubTx inSub in newTx.VIn)
            //{
            //    Printer.PrintText(" > TxID: {0}", BitConverter.ToString(inSub.TxHash).Replace("-", ""));
            //    Printer.PrintText(" > OutItemNr: {0}", inSub.OutItemNr.ToString());
            //    Printer.PrintText(" > InItemNr: {0}", inSub.InItemNr.ToString());
            //    Printer.PrintText(" > Address: {0}", inSub.Address);
            //    Printer.PrintText(" > Amount: {0} Mooncoin", inSub.Amount.ToString());
            //    Printer.PrintText(" > Signature: {0}", inSub.Signature);
            //}

            //Printer.PrintText("===== Generated Outputs: ");
            //foreach (SubTx outSub in newTx.VOut)
            //{
            //    Printer.PrintText(" > OutItemNr: {0}", outSub.OutItemNr.ToString());
            //    Printer.PrintText(" > InItemNr: {0}", outSub.InItemNr.ToString());
            //    Printer.PrintText(" > Address: {0}", outSub.Address);
            //    Printer.PrintText(" > Amount: {0} Mooncoin", outSub.Amount.ToString());
            //    Printer.PrintText(" > Signature: {0}", outSub.Signature);
            //    Printer.PrintText(" == ");
            //}

            //Printer.PrintText(" > Locktime: {0}", newTx.LockTime.ToString());

            // Finalize
            Printer.PrintText("Type 'pay' to broadcast transaction");
            if (Printer.Listen().ToLower() == "pay")
            {
                // Calculate hash of this transaction
                newTx.Hash = HashMachine.CalculateTxHash(newTx);

                // Set the Subtx hashes to the transaction hash
                foreach(SubTx subtx in newTx.VOut)
                {
                    subtx.TxHash = newTx.Hash;
                }

                // Add the transaction to the memorypool
                this._mempoolRepo.AddTransaction(newTx);
            }
            else
            {
                Printer.PrintText("Transaction cancelled");
            }
        }
    }
}