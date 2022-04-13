using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Console
{
    internal static class Printer
    {
        public static void PrintText(String text, String replacement = null)
        {
            System.Console.WriteLine(text, replacement);
        }

        public static void PrintText(String text, Int32? replacement)
        {
            System.Console.WriteLine(text, replacement);
        }

        public static void AskForCommand()
        {
            System.Console.WriteLine("\nType a request. Type 'help' for available commands");
        }

        public static void PrintAvailableCommands()
        {
            System.Console.WriteLine(
                //" > help : gives all available commands\n" +
                " > mine : starts the miner\n" +
                " > wallet balance : gives the current balance in Mooncoin\n" +
                " > wallet details : gives the details of the wallet\n" +
                " > wallet new : deletes the current wallet and creates a new one\n" +
                " > tx show list : gives all unspent transactions\n" +
                " > tx new : creates a new transaction to another Mooncoin address\n" +
                " > show blockchain : gives every block on the chain\n" +
                " > show mempool : gives all pending transactions\n" +
                " > password new : set a new password\n" +
                " > exit : closes the application\n"
            );
        }

        public static String Listen()
        {
            return System.Console.ReadLine();
        }

        public static String GenerateRandomString(int length)
        {
            const String chars = "0123456789ABCDEF";

            Random random = new Random();
            String randomString = new String(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }

        public static void PrintChain(Blockchain blockchain)
        {
            Int32 i = 0;

            System.Console.WriteLine("Blockchain 1: ");
            System.Console.WriteLine("Difficulty: {0}", BitConverter.ToString(blockchain.Difficulty).Replace("-", ""));
            System.Console.WriteLine("Mining Reward: {0}", blockchain.MiningReward);

            foreach (Block block in blockchain.Blocks)
            {
                System.Console.WriteLine(" ");
                System.Console.WriteLine("Block #{0}", i);
                System.Console.WriteLine("> Hash: {0}", BitConverter.ToString(block.Hash).Replace("-", ""));
                System.Console.WriteLine("> Version: {0}", block.Version);
                System.Console.WriteLine("> Previous Hash: {0}", BitConverter.ToString(block.PreviousHash).Replace("-", ""));
                System.Console.WriteLine("> Merkleroot: {0}", BitConverter.ToString(block.Merkleroot).Replace("-", ""));
                System.Console.WriteLine("> Timestamp: {0}", BitConverter.ToString(block.Timestamp).Replace("-", ""));
                System.Console.WriteLine("> Target: {0}", BitConverter.ToString(block.Target).Replace("-", ""));
                System.Console.WriteLine("> Nonce: {0}", block.Nonce);
                System.Console.WriteLine("> Tx count: {0}", block.TxCounter);

                i++;
            }
        }

        public static void PrintTransactions(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                // System.Console.WriteLine(" > Tx Hash: {0}", BitConverter.ToString(transaction.Hash).Replace("-", ""));
                Printer.PrintText(" > Protocol version: {0}", transaction.Version);
                Printer.PrintText(" > Total input: {0} Mooncoin", transaction.TotalInputValue.ToString());
                Printer.PrintText(" > Spent output: {0}", transaction.TotalOutputValue.ToString());
                Printer.PrintText(" > Spent fees: {0}", (transaction.TotalInputValue - transaction.TotalOutputValue).ToString());

                Printer.PrintText("===== Used unspent outputs");
                foreach (SubTx inSub in transaction.VIn)
                {
                    Printer.PrintText(" > TxID: {0}", BitConverter.ToString(inSub.TxHash).Replace("-", ""));
                    Printer.PrintText(" > OutItemNr: {0}", inSub.OutItemNr.ToString());
                    Printer.PrintText(" > InItemNr: {0}", inSub.InItemNr.ToString());
                    Printer.PrintText(" > Address: {0}", inSub.Address);
                    Printer.PrintText(" > Amount: {0} Mooncoin", inSub.Amount.ToString());
                    Printer.PrintText(" > Signature: {0}", inSub.Signature);
                }

                Printer.PrintText("===== Generated Outputs: ");
                foreach (SubTx outSub in transaction.VOut)
                {
                    //Printer.PrintText(" > TxID: {0}", BitConverter.ToString(outSub.TxHash).Replace("-", ""));
                    Printer.PrintText(" > OutItemNr: {0}", outSub.OutItemNr.ToString());
                    Printer.PrintText(" > InItemNr: {0}", outSub.InItemNr.ToString());
                    Printer.PrintText(" > Address: {0}", outSub.Address);
                    Printer.PrintText(" > Amount: {0} Mooncoin", outSub.Amount.ToString());
                    Printer.PrintText(" > Signature: {0}", outSub.Signature);
                    Printer.PrintText(" == ");
                }

                Printer.PrintText(" > Locktime: {0}", transaction.LockTime.ToString());
            }
        }
    }
}
