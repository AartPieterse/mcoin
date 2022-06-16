using System;
using System.IO;
using System.Text;

namespace Infrastructure
{
    public class AuthFileContext : FileContext
    {
        private readonly String pathString;

        public AuthFileContext(String pathName, String folderName)
        {
            this.pathString = this.SetupFile(pathName, folderName);
        }

        public String GetPassword()
        {
            return this.ReadFile();
        }

        public void AddPassword(String password)
        {
            this.WriteFile(password);
        }

        public void ClearFile()
        {
            this.ResetFile();
        }

        private String ReadFile()
        {
            String password;

            StreamReader reader;
            using (reader=  new StreamReader(this.pathString, Encoding.ASCII))
            {
                password = reader.ReadLine();
            }

            return password;
        }

        private void WriteFile(String password)
        {
            StreamWriter writer;
            using (writer = new StreamWriter(this.pathString, false, Encoding.ASCII))
            {
                writer.WriteLine(password);
            }
        }

        private void ResetFile()
        {
            using (File.Create(this.pathString))
            {

            }
        }
    }
}
