using Core.Domain;
using Core.DomainServices;
using System.Collections.Generic;

namespace UI.Console.Controllers
{
    public class MempoolController
    {
        private readonly IMempoolRepository _mempoolRepo;

        public MempoolController(IMempoolRepository mempoolRepository)
        {
            this._mempoolRepo = mempoolRepository;
        }

        public IEnumerable<Transaction> GetMempool()
        {
            return this._mempoolRepo.GetTransactions();
        }
    }
}
