using Core.Domain;
using System;
using System.Collections.Generic;

namespace Core.DomainServices
{
    public interface IUTXORepository
    {
        public IEnumerable<SubTx> GetAllUTXO(String publicKey);

        public void AddUTXO(SubTx output);
    }
}
