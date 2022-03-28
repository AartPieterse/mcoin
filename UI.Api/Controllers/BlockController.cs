using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UI.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class BlockController : Controller
    {
        private readonly IBlockRepository _blockRepository;

        public BlockController(IBlockRepository repository)
        {
            this._blockRepository = repository;
        }

        [Route("/")]
        [HttpGet("{hash}")]
        public IActionResult Details(string hash)
        {
            var block = this._blockRepository.GetOneBlock(hash);

            if(block == null)
            {
                return this.NotFound("Error: This block does not exist");
            }
            else
            {
                return this.Ok(block);
            }
        }
    }
}
