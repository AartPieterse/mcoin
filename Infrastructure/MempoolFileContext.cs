using Core.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class MempoolFileContext
    {
        const String IN = "IN";
        const String OUT = "OUT";

        private readonly String pathstring;

        public MempoolFileContext(String pathName, String folderName)
        {
            this.pathstring = Directory.GetCurrentDirectory().Substring(0, 40);

            this.pathstring = Path.Combine(this.pathstring, folderName);

            Directory.CreateDirectory(this.pathstring);

            this.pathstring = Path.Combine(this.pathstring, pathName + ".txt");

            if (!File.Exists(this.pathstring))
            {
                File.Create(this.pathstring);
            }
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return this.ReadFile();
        }

        public void AddTransaction(Transaction transaction)
        {
            this.WriteFile(transaction);
        }

        public void ClearPool()
        {
            this.ResetFile();
        }

        private IEnumerable<Transaction> ReadFile()
        {
            List<Transaction> transactions = new List<Transaction>();

            String line;
            StreamReader reader;
            using (reader = new StreamReader(this.pathstring, Encoding.ASCII))
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    Transaction tx = new Transaction();
                    List<SubTx> vin = new List<SubTx>();
                    List<SubTx> vout = new List<SubTx>();

                    tx.Hash = this.StringToByteArray(line);
                    tx.Version = int.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                    tx.TotalInputValue = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                    tx.TotalOutputValue = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);

                    line = reader.ReadLine();
                    while (line == IN)
                    {
                        SubTx input = new SubTx();

                        input.TxHash = this.StringToByteArray(reader.ReadLine());
                        input.InItemNr = int.Parse(reader.ReadLine());
                        input.OutItemNr = int.Parse(reader.ReadLine());
                        input.Address = reader.ReadLine();
                        input.Amount = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                        input.Signature = reader.ReadLine();
                        input.Spendable = reader.ReadLine() == "1" ? false : true;

                        vin.Add(input);
                        line = reader.ReadLine();
                    }

                    while (line == OUT)
                    {
                        SubTx output = new SubTx();

                        output.TxHash = this.StringToByteArray(reader.ReadLine());
                        output.InItemNr = int.Parse(reader.ReadLine());
                        output.OutItemNr = int.Parse(reader.ReadLine());
                        output.Address = reader.ReadLine();
                        output.Amount = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                        output.Signature = reader.ReadLine();
                        output.Spendable = reader.ReadLine() == "1" ? false : true;

                        vout.Add(output);
                        line = reader.ReadLine();
                    }

                    tx.LockTime = int.Parse(line, NumberStyles.HexNumber);

                    tx.VIn = vin;
                    tx.VOut = vout;

                    transactions.Add(tx);
                    line = reader.ReadLine();
                }
            }

            return transactions;
        }

        private void WriteFile(Transaction transaction)
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathstring, true, Encoding.ASCII))
            {
                writer.WriteLine(BitConverter.ToString(transaction.Hash).Replace("-", ""));
                writer.WriteLine(transaction.Version);
                writer.WriteLine(transaction.TotalInputValue.ToString("X8"));
                writer.WriteLine(transaction.TotalOutputValue.ToString("X8"));

                foreach (SubTx input in transaction.VIn)
                {
                    writer.WriteLine(IN);

                    writer.WriteLine(BitConverter.ToString(input.TxHash).Replace("-", ""));
                    writer.WriteLine(input.InItemNr);
                    writer.WriteLine(input.OutItemNr);
                    writer.WriteLine(input.Address);
                    writer.WriteLine(input.Amount.ToString("X8"));
                    writer.WriteLine(input.Signature);
                    writer.WriteLine(input.Spendable ? "0" : "1");
                }

                foreach (SubTx output in transaction.VOut)
                {
                    writer.WriteLine(OUT);

                    writer.WriteLine(BitConverter.ToString(output.TxHash).Replace("-", ""));
                    writer.WriteLine(output.InItemNr);
                    writer.WriteLine(output.OutItemNr);
                    writer.WriteLine(output.Address);
                    writer.WriteLine(output.Amount.ToString("X8"));
                    writer.WriteLine(output.Signature);
                    writer.WriteLine(output.Spendable ? "0" : "1");
                }

                writer.WriteLine(transaction.LockTime.ToString("X8"));
            }
        }

        private void ResetFile()
        {
            using (File.Create(this.pathstring))
            {

            }
        }

        private Byte[] StringToByteArray(String hex)
        {
            Byte[] bytes = new Byte[hex.Length / 2];
            for (Int32 i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
