using System;
using System.IO;

namespace Infrastructure
{
    public class FileContext
    {
        public string SetupFile(string fileName, string folderName)
        {
            string pathString;

            pathString = Directory.GetCurrentDirectory();

            pathString = Directory.GetParent(pathString).ToString();
            pathString = Directory.GetParent(pathString).ToString();
            pathString = Directory.GetParent(pathString).ToString();
            pathString = Directory.GetParent(pathString).ToString();
            pathString = Directory.GetParent(pathString).ToString();
            pathString = Directory.GetParent(pathString).ToString();

            pathString = Path.Combine(pathString, folderName);

            Directory.CreateDirectory(pathString);

            pathString = Path.Combine(pathString, fileName + ".txt");

            Console.WriteLine(pathString);

            if (!File.Exists(fileName)) File.Create(pathString);

            return pathString;
        }
    }
}
