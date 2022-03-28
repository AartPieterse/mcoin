using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Webservice.Api;
using UI.Webservice.Models;

namespace UI.Webservice.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionRequest _request;
        private readonly string path = "Transaction";

        public TransactionController()
        {
            this._request = new TransactionRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Overview()
        {
            IEnumerable<TransactionModel> transactions = await this._request.GetTransactionsAsync(this.path);

            this.ViewBag.Transactions = transactions;

            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddTransaction()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddTransaction(TransactionModel transaction)
        {
            bool to = true;
            bool fr = true;
            bool am = true;

            if (transaction.FromAddress == null)
            {
                ModelState.AddModelError("FromAddress", "* Where must the coins be send from?");
                fr = false;
            }
            else if (transaction.FromAddress.Substring(0, 2) != "0x")
            {
                ModelState.AddModelError("FromAddress", "* Invalid wallet address");
                fr = false;
            }

            if (transaction.ToAddress == null)
            {
                ModelState.AddModelError("ToAddress", "* On which address must the coins be deposited");
                to = false;
            }
            else if (transaction.ToAddress.Substring(0, 2) != "0x")
            {
                ModelState.AddModelError("ToAddress", "* Invalid wallet address");
                to = false;
            }

            if (transaction.Amount == null)
            {
                ModelState.AddModelError("Amount", "* We cannot send this amount of coins");
                am = false;
            }
            else if (transaction.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "* Must be greater than 0");
                am = false;
            }

            if (to && fr && am)
            {            
                await this._request.AddTransaction(this.path, transaction);

                return this.RedirectToAction("Overview");
            }

            return this.View();            
        }
    }
}
