using Core.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class UTXOFileContext
    {
        private readonly String pathstring;

        public UTXOFileContext(String pathName, String folderName)
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

        public IEnumerable<SubTx> GetAllUTXO()
        {
            return this.ReadFile();
        }

        public void AddUTXO(SubTx output)
        {
            this.WriteFile(output);
        }

        public void ClearFile()
        {
            this.ResetFile();
        }

        private IEnumerable<SubTx> ReadFile()
        {
            List<SubTx> outputs = new List<SubTx>();

            StreamReader reader;
            using (reader = new StreamReader(this.pathstring, Encoding.ASCII))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    SubTx subtx = new SubTx();

                    subtx.TxHash = this.StringToByteArray(line);
                    subtx.InItemNr = int.Parse(reader.ReadLine());
                    subtx.OutItemNr = int.Parse(reader.ReadLine());
                    subtx.Address = reader.ReadLine();
                    subtx.Amount = long.Parse(reader.ReadLine(), NumberStyles.HexNumber);
                    subtx.Signature = reader.ReadLine();
                    subtx.Spendable = reader.ReadLine() == "1" ? false : true;

                    outputs.Add(subtx);
                    line = reader.ReadLine();
                }
            }

            return outputs;
        }

        private void WriteFile(SubTx subTx)
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathstring, true, Encoding.ASCII))
            {
                writer.WriteLine(subTx.TxHash);
                writer.WriteLine(subTx.InItemNr);
                writer.WriteLine(subTx.OutItemNr);
                writer.WriteLine(subTx.Address);
                writer.WriteLine(subTx.Amount);
                writer.WriteLine(subTx.Signature);
                writer.WriteLine(subTx.Spendable ? "0" : "1");
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
