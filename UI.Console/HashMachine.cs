using Core.Domain;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UI.Console
{
    internal static class HashMachine
    {
        public static Byte[] CalculateTxHash(Transaction transaction)
        {
            StringBuilder message = new StringBuilder();

            String protocolVersion = transaction.Version.ToString("D8");
            message.Append(protocolVersion);
            Printer.PrintText("Version: {0}", protocolVersion);

            String inputCount = transaction.VIn.Count().ToString("D2");
            message.Append(inputCount);
            Printer.PrintText("Inputcount: {0}", inputCount);

            foreach (SubTx subtx in transaction.VIn)
            {
                String prev_txWithInput = BitConverter.ToString(subtx.TxHash).Replace("-", "");
                message.Append(prev_txWithInput);
                Printer.PrintText("prev_txWithInput: {0}", prev_txWithInput);

                String prev_txVoutNr = subtx.OutItemNr.ToString("X2");
                message.Append(prev_txVoutNr);
                Printer.PrintText("prev_txVoutNr: {0}", prev_txVoutNr);

                String sig_length = subtx.Signature.Length.ToString("X2");
                message.Append(sig_length);
                Printer.PrintText("sig_length: {0}", sig_length);

                String signature = subtx.Signature;
                message.Append(signature);
                Printer.PrintText("Signature: {0}", signature);

                String txVersion = transaction.Version.ToString("D8");
                message.Append(txVersion);
                Printer.PrintText("txVersion: {0}", txVersion);
            }

            String output_count = transaction.VOut.Count().ToString("D2");
            message.Append(output_count);
            Printer.PrintText("output_count: {0}", output_count);

            // Outputs
            foreach (SubTx subtx in transaction.VOut)
            {
                String value = subtx.Amount.ToString("X8");
                message.Append(value);
                Printer.PrintText("value: {0}", value);

                String pk = subtx.Address;
                String pk_length = pk.Length.ToString("X2");

                Printer.PrintText("pk_length: {0}", pk_length);
                Printer.PrintText("pk: {0}", pk);

                message.Append(pk_length);
                message.Append(pk);
            }

            String locktime = transaction.LockTime.ToString("X8");
            message.Append(locktime);
            Printer.PrintText("locktime: {0}", locktime);

            Printer.PrintText(message.ToString());

            Byte[] hash = CalculateHash(message.ToString());
            Printer.PrintText(BitConverter.ToString(hash).Replace("-", ""));

            return hash;
        }

        public static Byte[] CalculateBlockHash(Block block)
        {
            StringBuilder message = new StringBuilder();

            String version = block.Version.ToString("D8");
            message.Append(version);
            //Printer.PrintText("Version: {0}", version);

            String prev_hash = BitConverter.ToString(block.PreviousHash).Replace("-", "");
            message.Append(prev_hash);
            //Printer.PrintText("Prev_hash: {0}", prev_hash);

            String merkleroot = BitConverter.ToString(block.Merkleroot).Replace("-", "");
            message.Append(merkleroot);
            //Printer.PrintText("Merkleroot: {0}", merkleroot);

            String time = BitConverter.ToString(block.Timestamp).Replace("-", "");
            message.Append(merkleroot);
            //Printer.PrintText("Merkleroot: {0}", merkleroot);

            String target = BitConverter.ToString(block.Target).Replace("-", "");
            message.Append(merkleroot);
            //Printer.PrintText("Target: {0}", target);

            String nonce = block.Nonce.ToString("X8");
            message.Append(nonce);
            //Printer.PrintText("Nonce: {0}", nonce);

            //Printer.PrintText("Message: {0}", message.ToString());

            Byte[] hash = CalculateHash(message.ToString());
            //Printer.PrintText(BitConverter.ToString(hash).Replace("-", ""));

            Byte[] hashagain = CalculateHash(BitConverter.ToString(hash).Replace("-", ""));
            //Printer.PrintText(BitConverter.ToString(hashagain).Replace("-", ""));

            return hash;
        }

        public static Byte[] CalculateHash(String input)
        {
            Byte[] bytes = new Byte[input.Length / 2];
            for (Int32 i = 0; i < input.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }

            using (SHA256 sha = SHA256.Create())
            {
                Byte[] hash = sha.ComputeHash(bytes);

                return hash;
            }
        }
    }
}
