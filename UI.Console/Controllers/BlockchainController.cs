using Core.Domain;
using Core.DomainServices;
using System.Collections.Generic;

namespace UI.Console.Controllers
{
    public class BlockchainController
    {
        private readonly IBlockRepository _blockRepository;

        public BlockchainController(IBlockRepository repo)
        {
            this._blockRepository = repo;
        }

        public Blockchain GetBlockchain()
        {
            return this._blockRepository.GetBlockchain();
        }

        public IEnumerable<Block> GetAllBlocks()
        {
            return this._blockRepository.GetAllBlocks();
        }
    }
}
