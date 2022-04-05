using Core.Domain;
using Core.DomainServices;
using Infrastructure;
using System;
using System.Globalization;
using UI.Console.Controllers;
using UI.Console.HTTP;

namespace UI.Console
{
    internal class Program
    {       
        // Hardcoded for now, has to come from BlockchainData.txt
        private static readonly Byte[] s_target = new Byte[4] { 0x1A, 0xFF, 0xFF, 0xFF };
        private static readonly Int32 s_miningReward = 20;

        private static readonly Int32 s_version = 1;
        private static readonly Int32 s_txVersion = 1;

        private const String FOLDERNAME = "Data";

        private static readonly NodeSender s_sender = new NodeSender();

        private static readonly IBlockRepository s_blockRepo = new BlockRepository(new BlockFileContext("BlockchainData", FOLDERNAME, s_target, s_miningReward));
        private static readonly IMempoolRepository s_mempoolRepo = new MempoolRepository(new MempoolFileContext("MempoolData", FOLDERNAME));
        private static readonly IUTXORepository s_utxoRepo = new UTXORepository(new UTXOFileContext("UTXOData", FOLDERNAME));
        private static readonly IWalletRepository s_walletRepo = new WalletRepository(new WalletFileContext("WalletData", FOLDERNAME));
        private static readonly IAuthRepository s_authRepo = new AuthRepository(new AuthFileContext("Auth", FOLDERNAME));

        private static void Main(String[] args)
        {
            AuthController authController = new AuthController(s_authRepo);
            BlockchainController blockController = new BlockchainController(s_blockRepo);
            MempoolController mempoolController = new MempoolController(s_mempoolRepo);
            TransactionController transactionController = new TransactionController(s_mempoolRepo, s_utxoRepo, s_walletRepo, s_txVersion);
            WalletController walletController = new WalletController(s_walletRepo, s_utxoRepo);

            Miner miner = new Miner(s_sender, null, s_mempoolRepo, s_utxoRepo, s_version, s_target, walletController.GetWallet(), s_miningReward);

            ConsoleRouter router = new ConsoleRouter(authController, blockController, mempoolController, walletController, transactionController, miner);

            authController.Authenticate();

            router.StartProcess();
        }

        private static void comments()
        {
            // Convert: long > string > byte[] > string > long
            Int64 time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

            String hex = time.ToString("X");

            Byte[] bytes = new Byte[hex.Length / 2];
            for (Int32 i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            String stringagain = BitConverter.ToString(bytes).Replace("-", "");

            Int64 longagain = long.Parse(stringagain, NumberStyles.HexNumber);


            foreach (Block b in s_blockRepo.GetAllBlocks())
            {
                System.Console.WriteLine(BitConverter.ToString(b.Hash).Replace("-", ""));
                System.Console.WriteLine(s_version.ToString("D8"));
                System.Console.WriteLine(BitConverter.ToString(b.PreviousHash).Replace("-", ""));
                System.Console.WriteLine(BitConverter.ToString(b.Merkleroot).Replace("-", ""));
                System.Console.WriteLine(b.Timestamp);
                System.Console.WriteLine(b.Target);
                System.Console.WriteLine(b.Nonce);
            }
        }
    }
}
