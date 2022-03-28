using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Core.Domain;
using Xunit;
using System.IO;

namespace Testing
{
    public class BlockchainRepositoryTest
    {
        private readonly BlockchainRepository _blockchainRepository;
        private String PathString;

        public BlockchainRepositoryTest()
        {
            this._blockchainRepository = new BlockchainRepository(new BlockFileContext("TestBlockchainData", "TestData"), new MempoolFileContext("TestMempoolData", "TestData"));
        }

        [Fact]
        public void GetBlockchain()
        {
            // Get the blockchain
            Blockchain blockchain = this._blockchainRepository.GetBlockchain();

            // Check if correct

        }

        [Fact]
        public void GetAllBlocks()
        {

        }

        [Fact]
        public void GetLastBlock()
        {

        }
    }
}
