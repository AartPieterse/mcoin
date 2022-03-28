using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Infrastructure;

namespace Testing
{
    public class UTXORepositoryTest
    {
        private readonly UTXORepository _utxoRepository;

        public UTXORepositoryTest()
        {
            this._utxoRepository = new UTXORepository(new UTXOFileContext("TestUTXOData", "TestData"));
        }


    }
}
