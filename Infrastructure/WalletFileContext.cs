using Core.Domain;
using System;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class WalletFileContext
    {
        public Boolean HasWallet = false;
        private readonly String pathstring;

        public WalletFileContext(String pathName, String folderName)
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

        public Wallet GetWallet()
        {
            return this.ReadFile();
        }

        public void AddWallet(Wallet wallet)
        {
            this.WriteFile(wallet);
        }

        private Wallet ReadFile()
        {
            Wallet wallet = new Wallet();

            StreamReader reader;
            using (reader = new StreamReader(this.pathstring, Encoding.ASCII))
            {
                String line = reader.ReadLine();
                if (line == null)
                {
                    this.HasWallet = false;
                }
                else
                {
                    wallet.PublicKey = line;
                    wallet.PrivateKey = reader.ReadLine();
                    wallet.Balance = ulong.Parse(reader.ReadLine());

                    this.HasWallet = true;
                }
            }

            return wallet;
        }

        private void WriteFile(Wallet wallet)
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathstring, false, Encoding.ASCII))
            {
                writer.WriteLine(wallet.PublicKey);
                writer.WriteLine(wallet.PrivateKey);
                writer.WriteLine(wallet.Balance);
            }
        }

        private void ResetFile()
        {
            using (File.Create(this.pathstring))
            {

            }
        }
    }
}
