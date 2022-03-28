using Core.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using UI.Api.Models;

namespace UI.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository repository)
        {
            this._transactionRepository = repository;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult<IEnumerable<TransactionDTO>> GetTransactions()
        {
            var x = this._transactionRepository.GetTransactions();

            return this.Ok(x);
        }

        [HttpPost]
        [Route("/")]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction.FromAddress.Substring(0, 2) != "0x")
            {
                return this.UnprocessableEntity("Error: Invalid sending address");
            }
            else if (transaction.ToAddress.Substring(0, 2) != "0x")
            {
                return this.UnprocessableEntity("Error: Invalid recieving address");
            }
            else if (transaction.Amount < 1)
            {
                return this.UnprocessableEntity("Error: Invalid amount");
            }

            if (this._transactionRepository.AddTransaction(transaction))
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Error: Processing transaction went wrong");
            }
        }
    }
}
