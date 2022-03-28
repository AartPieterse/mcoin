using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Webservice.Api;
using UI.Webservice.Models;

namespace UI.Webservice.Controllers
{
    public class BlockchainController : Controller
    {

        private readonly BlockRequest _request;
        private readonly string path = "Blockchain";

        public BlockchainController()
        {
            this._request = new BlockRequest();
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Overview()
        {
            IEnumerable<BlockModel> blocks = await this._request.GetBlocksAsync(this.path);

            return this.View(blocks.ToList());
        }

        public async Task<IActionResult> AddBlock(BlockModel block)
        {
            await this._request.AddBlockAsync(this.path);

            return this.RedirectToAction("Overview");
        }
    }
}
