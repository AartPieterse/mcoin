using Core.Domain;
using System;
using System.Collections.Generic;

namespace Core.DomainServices
{
    public interface IBlockRepository
    {
        Block GetOneBlock(Byte[] hash);

        Blockchain GetBlockchain();

        IEnumerable<Block> GetAllBlocks();

        void AddBlock();

        Block GetLastBlock();
    }
}
