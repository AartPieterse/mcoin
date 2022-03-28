using Core.Domain;
using System.Collections.Generic;

namespace Core.DomainServices
{
    public interface IMempoolRepository
    {
        IEnumerable<Transaction> GetTransactions();

        void AddTransaction(Transaction transaction);

        void ClearMempool();
    }
}
