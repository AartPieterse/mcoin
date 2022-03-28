using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UI.Webservice.Api;
using UI.Webservice.Models;

namespace UI.Webservice.Controllers
{
    public class BlockController : Controller
    {
        private readonly BlockRequest _request;
        private readonly string path = "Block/";

        public BlockController()
        {
            this._request = new BlockRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            BlockModel block = await this._request.GetBlockDetailsAsync(this.path + id);

            Debug.WriteLine("block: ", block);

            if (block != null)
            {
                return View(block);
            }

            return View();
        }
    }
}
