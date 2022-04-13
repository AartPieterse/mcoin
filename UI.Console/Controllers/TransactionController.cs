using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UI.Console.Controllers
{
    public class TransactionController
    {
        private readonly IMempoolRepository _mempoolRepo;
        private readonly IUTXORepository _utxoRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly int _version;

        public TransactionController(IMempoolRepository mempoolRepo, IUTXORepository utxoRepo, IWalletRepository walletRepo, int version)
        {
            this._mempoolRepo = mempoolRepo;
            this._utxoRepo = utxoRepo;
            this._walletRepo = walletRepo;
            this._version = version;
        }

        public void PrintUTXOs()
        {
            string address = this._walletRepo.GetWallet().PublicKey;

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
            Printer.PrintText("Enter address: ");

            string address = Printer.Listen();

            char firstchar = address.First();

            Printer.PrintText("Creating new transaction...");
            Thread.Sleep(1000);

            switch (firstchar)
            {
                case '1':
                    Printer.PrintText("You entered a Public Key Hash");
                    this.PayToPublicKeyHash(address);
                    break;
                case '3':
                    Printer.PrintText("You entered a Script Hash");
                    this.PayToScriptHash(address);
                    break;
                case 'C':
                    Printer.PrintText("You entered an x");
                    this.SetupTransactionForm(address);
                    break;

                default:
                    Printer.PrintText("Invalid address");
                    break;
            }
        }

        private void PayToPublicKeyHash(string address)
        {            
            // Decode from base58
            byte[] pkh = null;
            if (!Decode(address, ref pkh))
            {
                Printer.PrintText("Wrong Address");

                return;
            }

            // Validate public key hash
            if (!ValidateChecksum(ref pkh))
            {
                Printer.PrintText("Faulty address");

                return;
            }

            this.SetupTransactionForm("abcdef");
        }

        private void PayToScriptHash(string address)
        {
            Printer.PrintText("P2SH");
        }

        public void SetupTransactionForm(string address)
        {
            // sends a transaction


            string userAddress = this._walletRepo.GetWallet().PublicKey;
            List<SubTx> personalUtxoList = this._utxoRepo.GetAllUTXO(userAddress).ToList();

            if (personalUtxoList.Count() == 0)
            {
                Printer.PrintText("Your wallet is empty. \n Aborting transaction...");
                Thread.Sleep(1000);

                return;
            }

            Transaction newTx = new Transaction();

            // Transaction information: version, vin, totalvin, vout, totalvout, locktime
            Printer.PrintText("Enter some information to complete transaction");

            // Input
            int outputListCounter = 0;
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

            Printer.PrintText("Send to address: {0}", address);
            userOutput.Address = address;

            Printer.PrintText("Send amount: ");
            userOutput.Amount = int.Parse(Printer.Listen());

            userOutput.InItemNr = -1;
            userOutput.OutItemNr = 0;
            userOutput.Signature = "xxxx";
            userOutput.Spendable = true;

            // Ask for fee
            Printer.PrintText("Pay fee: ");
            int fee = int.Parse(Printer.Listen());

            // System created 1 return output
            SubTx returnOutput = new SubTx
            {
                Address = userAddress,
                Amount = newTx.TotalInputValue - userOutput.Amount - fee,
                InItemNr = -1,
                OutItemNr = 1,
                Signature = "xxxx",
                Spendable = true
            };

            // Add user-output, return-output, totaloutput, version and locktime to new transaction
            List<SubTx> outputListForNewTx = new List<SubTx>
            {
                userOutput,
                returnOutput
            };
            newTx.VOut = outputListForNewTx;

            newTx.Version = this._version;

            Printer.PrintText("Locktime (s): ");
            newTx.LockTime = int.Parse(Printer.Listen());

            newTx.TotalOutputValue = userOutput.Amount + returnOutput.Amount;

            // Summary
            List<Transaction> transactions = new List<Transaction>
            {
                newTx
            };

            Printer.PrintTransactions(transactions);

            // Finalize
            Printer.PrintText("Type 'pay' to broadcast transaction");
            if (Printer.Listen().ToLower() == "pay")
            {
                // Calculate hash of this transaction
                newTx.Hash = HashMachine.CalculateTxHash(newTx);

                // Set the Subtx hashes to the transaction hash
                foreach (SubTx subtx in newTx.VOut)
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

        public static bool Decode(string source, ref byte[] destination)
        {
            string Base58characters = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            int i = 0;
            while (i < source.Length)
            {
                if (source[i] == 0 || !char.IsWhiteSpace(source[i]))
                {
                    break;
                }
                i++;
            }

            int zeros = 0;
            while (source[i] == '1')
            {
                zeros++;
                i++;
            }

            byte[] b256 = new byte[(source.Length - i) * 733 / 1000 + 1];
            while (i < source.Length && !char.IsWhiteSpace(source[i]))
            {
                int ch = Base58characters.IndexOf(source[i]);
                if (ch == -1) //null
                {
                    return false;
                }
                int carry = Base58characters.IndexOf(source[i]);
                for (int k = b256.Length - 1; k >= 0; k--)
                {
                    carry += 58 * b256[k];
                    b256[k] = (byte)(carry % 256);
                    carry /= 256;
                }
                i++;
            }
            while (i < source.Length && char.IsWhiteSpace(source[i]))
            {
                i++;
            }
            if (i != source.Length)
            {
                return false;
            }

            int j = 0;
            while (j < b256.Length && b256[j] == 0)
            {
                j++;
            }

            destination = new byte[zeros + (b256.Length - j)];
            for (int kk = 0; kk < destination.Length; kk++)
            {
                if (kk < zeros)
                {
                    destination[kk] = 0x00;
                }
                else
                {
                    destination[kk] = b256[j++];
                }
            }

            return true;
        }

        public static bool ValidateChecksum(ref byte[] source)
        {
            try
            {
                // extract checksum
                byte[] checksum = new byte[4] { source[^4], source[^3], source[^2], source[^1] };

                // remove checksum from array
                byte[] pkhWithoutChecksum = new byte[source.Length - 4];
                for (int i = 0; i < pkhWithoutChecksum.Length; i++)
                {
                    pkhWithoutChecksum[i] = source[i];
                }

                source = pkhWithoutChecksum;

                // double hash public key
                byte[] firsthash  = HashMachine.CalculateHash(BitConverter.ToString(pkhWithoutChecksum).Replace("-", ""));
                byte[] secondhash = HashMachine.CalculateHash(BitConverter.ToString(firsthash).Replace("-", ""));

                byte[] firstbytes = new byte[4] { secondhash[0], secondhash[1], secondhash[2], secondhash[3] };

                // compare byte arrays
                for(int i = 0; i < 4; i++)
                {
                    if (!(checksum[i] == firstbytes[i]))
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}