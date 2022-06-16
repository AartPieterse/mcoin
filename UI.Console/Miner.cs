using System.Diagnostics;

namespace UI.Console
{
    static class Miner
    {
        public static void StartMinerProgram()
        {
            Process process = new Process();

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process.Start(@"C:\Users\aartp\Documents\Coins\Miner.exe");
        }
    }
}
