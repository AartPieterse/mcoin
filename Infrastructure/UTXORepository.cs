using Core.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class UTXORepository : IUTXORepository
    {
        private readonly UTXOFileContext _context;

        public UTXORepository(UTXOFileContext context)
        {
            this._context = context;
        }

        public IEnumerable<SubTx> GetAllUTXO(String address = null)
        {
            IEnumerable<SubTx> list = this._context.GetAllUTXO();
            List<SubTx> strippedList = new List<SubTx>();

            if (address == null)
                return list;
            else
            {
                foreach (SubTx sub in list)
                {
                    if (sub.Address == address)
                    {
                        strippedList.Add(sub);
                    }
                }

                return strippedList;
            }
        }

        public void AddUTXO(SubTx output)
        {
            this._context.AddUTXO(output);
        }
    }
}
