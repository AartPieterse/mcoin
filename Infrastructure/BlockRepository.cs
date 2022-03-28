using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class BlockRepository : IBlockRepository
    {
        private readonly BlockFileContext _context;

        public BlockRepository(BlockFileContext context)
        {
            this._context = context;
        }

        public Block GetOneBlock(Byte[] hash)
        {
            return this._context.GetOneBlock(hash);
        }

        public Blockchain GetBlockchain()
        {
            return this._context.GetBlockchain();
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            return this._context.GetAllBlocks();
        }

        public Block GetLastBlock()
        {
            return this._context.GetAllBlocks().Last();
        }

        public void AddBlock()
        {

        }
    }
}
