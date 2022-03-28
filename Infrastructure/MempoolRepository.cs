using Core.Domain;
using Core.DomainServices;
using System.Collections.Generic;

namespace Infrastructure
{
    public class MempoolRepository : IMempoolRepository
    {
        private readonly MempoolFileContext _context;

        public MempoolRepository(MempoolFileContext context)
        {
            this._context = context;
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return this._context.GetAllTransactions();
        }

        public void AddTransaction(Transaction transaction)
        {
            this._context.AddTransaction(transaction);
        }

        public void ClearMempool()
        {
            this._context.ClearPool();
        }
    }
}
