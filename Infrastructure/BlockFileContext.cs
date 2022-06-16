using Core.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class BlockFileContext : FileContext
    {
        private const String IN = "IN";
        private const String OUT = "OUT";

        private readonly String pathString;

        private readonly byte[] target;
        private readonly Int32 miningreward;

        public BlockFileContext(String pathName, String folderName, byte[] target, Int32 miningreward)
        {
            this.pathString = this.SetupFile(pathName, folderName);

            this.target = target;
            this.miningreward = miningreward;
        }

        public Blockchain GetBlockchain()
        {
            return this.ReadFile();
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            return this.ReadFile().Blocks;
        }

        public Block GetOneBlock(Byte[] hash)
        {
            foreach (Block block in this.ReadFile().Blocks)
            {
                if (block.Hash == hash)
                {
                    return block;
                }
            }

            return null;
        }

        public void SetupFile()
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathString, true, Encoding.ASCII))
            {
                writer.WriteLine(BitConverter.ToString(target).Replace("-", ""));
                writer.WriteLine(miningreward);
            }
        }

        public void AddBlock(Block block)
        {
            this.WriteFile(block);
        }

        private Blockchain ReadFile()
        {
            Blockchain blockchain = new Blockchain();
            List<Block> blockList = new List<Block>();

            StreamReader reader;
            using (reader = new StreamReader(this.pathString, Encoding.ASCII))
            {

                string line = reader.ReadLine();

                // if file is empty, write necessary information
                if (line == null)
                {
                    reader.Close();

                    this.SetupFile();

                    reader = new StreamReader(this.pathString, Encoding.ASCII);

                    line = reader.ReadLine();
                }

                blockchain.Difficulty = this.StringToByteArray(line);
                blockchain.MiningReward = int.Parse(reader.ReadLine());

                line = reader.ReadLine();
                while (line != null)
                {
                    Block block = new Block();

                    block.Hash = this.StringToByteArray(line);
                    block.Version = int.Parse(reader.ReadLine());
                    block.PreviousHash = this.StringToByteArray(reader.ReadLine());
                    block.Merkleroot = this.StringToByteArray(reader.ReadLine());
                    block.Timestamp = this.StringToByteArray(reader.ReadLine());
                    block.Target = this.StringToByteArray(reader.ReadLine());
                    block.Nonce = int.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                    block.TxCounter = int.Parse(reader.ReadLine());

                    List<Transaction> blockTransactions = new List<Transaction>();
                    for (Int32 i = 0; i < block.TxCounter; i++)
                    {
                        Transaction readingTx = new Transaction();
                        List<SubTx> vin = new List<SubTx>();
                        List<SubTx> vout = new List<SubTx>();

                        readingTx.Hash = this.StringToByteArray(reader.ReadLine());
                        readingTx.Version = int.Parse(reader.ReadLine());
                        readingTx.TotalInputValue = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                        readingTx.TotalOutputValue = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);

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

                        readingTx.LockTime = int.Parse(line, NumberStyles.HexNumber);

                        readingTx.VIn = vin;
                        readingTx.VOut = vout;
                        blockTransactions.Add(readingTx);

                    }

                    block.Transactions = blockTransactions;

                    blockList.Add(block);

                    line = reader.ReadLine();
                }
            }

            blockchain.Blocks = blockList;

            return blockchain;
        }

        private void WriteFile(Block block)
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathString, true, Encoding.ASCII))
            {
                writer.WriteLine(BitConverter.ToString(block.Hash).Replace("-", ""));
                writer.WriteLine(block.Version);
                writer.WriteLine(BitConverter.ToString(block.PreviousHash).Replace("-", ""));
                writer.WriteLine(BitConverter.ToString(block.Merkleroot).Replace("-", ""));
                writer.WriteLine(BitConverter.ToString(block.Timestamp).Replace("-", ""));
                writer.WriteLine(BitConverter.ToString(block.Target).Replace("-", ""));
                writer.WriteLine(block.Nonce.ToString("X8"));
                writer.WriteLine(block.TxCounter.ToString("D2"));

                foreach (Transaction tx in block.Transactions)
                {
                    writer.WriteLine(tx.Hash);
                    writer.WriteLine(tx.Version);
                    writer.WriteLine(tx.TotalInputValue.ToString("X8"));
                    writer.WriteLine(tx.TotalOutputValue.ToString("X8"));

                    foreach (SubTx inSub in tx.VIn)
                    {
                        writer.WriteLine(IN);

                        writer.WriteLine(BitConverter.ToString(inSub.TxHash).Replace("-", ""));
                        writer.WriteLine(inSub.InItemNr);
                        writer.WriteLine(inSub.OutItemNr);
                        writer.WriteLine(inSub.Address);
                        writer.WriteLine(inSub.Amount.ToString("X8"));
                        writer.WriteLine(inSub.Signature);
                        writer.WriteLine(inSub.Spendable ? "0" : "1");
                    }

                    foreach (SubTx outSub in tx.VOut)
                    {
                        writer.WriteLine(OUT);

                        writer.WriteLine(BitConverter.ToString(outSub.TxHash).Replace("-", ""));
                        writer.WriteLine(outSub.InItemNr);
                        writer.WriteLine(outSub.OutItemNr);
                        writer.WriteLine(outSub.Address);
                        writer.WriteLine(outSub.Amount.ToString("X8"));
                        writer.WriteLine(outSub.Signature);
                        writer.WriteLine(outSub.Spendable ? "0" : "1");
                    }

                    writer.WriteLine(tx.LockTime.ToString("X8"));
                }
            }
        }

        private void ResetFile()
        {
            using (File.Create(this.pathString))
            {

            }
        }

        private Byte[] StringToByteArray(String hex)
        {
            if (hex == null)
                return null;

            Byte[] bytes = new Byte[hex.Length / 2];
            for (Int32 i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
