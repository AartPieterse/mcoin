using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UI.Console.HTTP;

namespace UI.Console
{
    public class Miner
    {
        private readonly NodeSender _sender;

        private readonly IBlockRepository _blockRepository;
        private readonly IMempoolRepository _mempoolRepository;
        private readonly IUTXORepository _utxoRepository;

        private readonly Int32 _version;
        private readonly Byte[] _target;
        private readonly Wallet _userWallet;
        private readonly Int32 _miningReward;

        public Miner(
            NodeSender sender,
            IBlockRepository bRepo,
            IMempoolRepository mpRepo,
            IUTXORepository utxoRepo,
            Int32 version,
            Byte[] target,
            Wallet wallet,
            Int32 miningReward
        )
        {
            this._sender = sender;
            this._blockRepository = bRepo;
            this._mempoolRepository = mpRepo;
            this._utxoRepository = utxoRepo;
            this._version = version;
            this._target = target;
            this._userWallet = wallet;
            this._miningReward = miningReward;
        }

        public void Mine()
        {
            Block block = this.GetBlockInfo();

            Byte[] calculatedHash = HashMachine.CalculateBlockHash(block);

            Byte[] realTarget = this.DecompressTarget(block.Target);

            Boolean hashIsIncorrect = true;
            while (hashIsIncorrect)
            {
                Printer.PrintText("Try: {0}", block.Nonce);
                Printer.PrintText(BitConverter.ToString(calculatedHash));
                Printer.PrintText(BitConverter.ToString(this._target));
                Printer.PrintText(BitConverter.ToString(realTarget));

                if (this.HashIsLessThenTarget(calculatedHash, realTarget) == 1)
                {
                    // Good
                    hashIsIncorrect = false;

                    block.Hash = calculatedHash;

                    Boolean approved = this._sender.ShareMinedBlock(block);

                    if (approved)
                    {
                        this._mempoolRepository.ClearMempool();
                    }

                }
                else
                {
                    // Hash too high, increase nonce and recalculate
                    block.Nonce++;

                    calculatedHash = HashMachine.CalculateBlockHash(block);
                }
            }
        }

        private Block GetBlockInfo()
        {
            Block b = new Block();
            IEnumerable<Transaction> transactions = this._mempoolRepository.GetTransactions();

            Int32 version = 1;
            Byte[] previousHash = this._blockRepository.GetLastBlock().Hash;
            Byte[] merkleroot = this.CalculateMerkleroot(transactions); // TBD
            Byte[] timestamp = this.CalculateTimestamp();
            Byte[] target = this._target;
            Int32 nonce = 0;

            return new Block(new Byte[32], version, previousHash, merkleroot, timestamp, target, nonce, transactions);
        }

        private Byte[] CalculateHash(Byte[] header)
        {
            using (SHA256 sha = SHA256.Create())
            {
                Byte[] bytes = sha.ComputeHash(header);

                return bytes;
            }
        }

        private Int16 HashIsLessThenTarget(Byte[] hash, Byte[] target)
        {
            Int32 counter = 0;
            while (true)
            {
                Int32 h = int.Parse(hash[counter].ToString(), NumberStyles.HexNumber);
                Int32 t = int.Parse(target[counter].ToString(), NumberStyles.HexNumber);

                if (h < t)
                    return 1;

                if (h > t)
                    return -1;

                counter++;
            }
        }

        private Byte[] CalculateMerkleroot(IEnumerable<Transaction> transactions)
        {
            return new Byte[3] { 0x21, 0x55, 0x3A };
        }

        private Byte[] CalculateTimestamp()
        {
            String hex = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString("X");

            Byte[] arr = new Byte[4];
            for (Int32 i = 0; i < arr.Length; i += 2)
                arr[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return arr;
        }

        private Byte[] DecompressTarget(Byte[] compressedtarget)
        {
            Byte[] realTarget = new Byte[32];
            for (Int32 i = 0; i < realTarget.Length; i++)
            {
                realTarget[i] = 0x00;
            }

            Int32 x = compressedtarget[0];
            realTarget[realTarget.Length - x + 0] = compressedtarget[1]; 
            realTarget[realTarget.Length - x + 1] = compressedtarget[2]; 
            realTarget[realTarget.Length - x + 2] = compressedtarget[3]; 

            return realTarget;
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
