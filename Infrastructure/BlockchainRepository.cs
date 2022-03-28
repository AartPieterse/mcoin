//using Core.Domain;
//using Core.DomainServices;
//using System.Collections.Generic;
//using System.Linq;

//namespace Infrastructure
//{
//    public class BlockchainRepository : IBlockchainRepository
//    {
//        private readonly BlockFileContext _blockContext;
//        private readonly MempoolFileContext _transactionContext;

//        public BlockchainRepository(BlockFileContext bContext, MempoolFileContext tContext)
//        {
//            this._blockContext = bContext;
//            this._transactionContext = tContext;
//        }

//        public Blockchain GetBlockchain()
//        {
//            return this._blockContext.GetBlockchain();
//        }

//        public IEnumerable<Block> GetAllBlocks()
//        {
//            return this._blockContext.GetAllBlocks();
//        }

//        public Block GetLastBlock()
//        {
//            return this._blockContext.GetAllBlocks().Last();
//        }

//        public void AddBlock()
//        {

//        }
//    }
//}
