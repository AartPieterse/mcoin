using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UI.Api.Models;

namespace UI.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class BlockchainController : Controller
    {
        private readonly IBlockchainRepository _blockchainRepository;

        public BlockchainController(IBlockchainRepository blockchainRepo)
        {
            this._blockchainRepository = blockchainRepo;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult<BlockchainDTO> GetBlockchain()
        {
            IEnumerable<Core.Domain.Block> result = this._blockchainRepository.GetChain();

            return this.Ok(result);
        }

        [HttpPost]
        [Route("/")]
        public IActionResult AddBlock()
        {
            this._blockchainRepository.AddBlock();

            return this.Ok();
        }
    }
}
