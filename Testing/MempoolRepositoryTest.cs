using Core.Domain;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Testing
{
    public class MempoolRepositoryTest
    {
        private readonly MempoolRepository _mempoolRepository;

        public MempoolRepositoryTest()
        {
            this._mempoolRepository = new MempoolRepository(new MempoolFileContext("TestMempoolData", "TestData"));

            // Clear pool before each test
            this._mempoolRepository.ClearMempool();
        }

        [Fact]
        public void GetAllTransactionsWhenNonexisting()
        {
            // Get all transactions
            IEnumerable<Transaction> transactions = this._mempoolRepository.GetTransactions();

            // Check if list is empty
            Assert.Empty(transactions);
        }

        [Fact]
        public void GetAllTransactionsWhenExisting()
        {
            // Add transactions
            List<Transaction> transactions = this.AddTransactionForTestingPurposes();

            // Get all transactions
            IEnumerable<Transaction> savedTransactions = this._mempoolRepository.GetTransactions();

            // Check if list is not empty && correct
            Assert.NotEmpty(savedTransactions);
            Assert.Equal(transactions[0].Hash, savedTransactions.First().Hash);
        }

        [Fact]
        public void ClearMempool()
        {
            // Add transaction
            this.AddTransactionForTestingPurposes();

            // Check if exists
            IEnumerable<Transaction> addedTransactions = this._mempoolRepository.GetTransactions();
            Assert.NotEmpty(addedTransactions);
            Assert.True(addedTransactions.Count() == 3);

            // Delete
            this._mempoolRepository.ClearMempool();

            // Check if not exist
            IEnumerable<Transaction> transactions2 = this._mempoolRepository.GetTransactions();
            Assert.Empty(transactions2);
        }

        private List<Transaction> AddTransactionForTestingPurposes()
        {
            Byte[] hash1 = new Byte[] { 0x21, 0xF4, 0xAC, 0x2F, 0xE7, 0xB1, 0xE4, 0xAA, 0xAD, 0xFC, 0xFF, 0x66, 0x55, 0x2C, 0xD9, 0x33, 0x94, 0xBA, 0xA0, 0x44, 0x24, 0x36, 0x96, 0x3E, 0xE0, 0xD6, 0x08, 0xBA, 0x3B, 0x50, 0x13, 0xB1 };
            Byte[] hash2 = new Byte[] { 0x22, 0xF4, 0xAC, 0x01, 0xE7, 0xB1, 0xE4, 0x22, 0xDD, 0xFC, 0x11, 0x66, 0x94, 0x44, 0xD9, 0x9F, 0x94, 0xA2, 0xA5, 0x5C, 0xBB, 0x36, 0x96, 0x3E, 0xE0, 0xDD, 0x08, 0x10, 0x3B, 0x50, 0x13, 0xB1 };
            Byte[] hash3 = new Byte[] { 0x23, 0xF4, 0xAC, 0x0C, 0xE7, 0xB1, 0x11, 0xAA, 0xAD, 0xEE, 0x70, 0x33, 0x94, 0x2C, 0x99, 0x11, 0x45, 0xAA, 0x33, 0x5C, 0x23, 0x36, 0x96, 0x3E, 0xE0, 0x34, 0x08, 0xBA, 0x3B, 0x50, 0x13, 0xB1 };
            SubTx subInTx = new SubTx(hash1, 0, 0, "ABCD1", 100, "xxxsignone", true);
            SubTx subOutTx = new SubTx(hash1, 0, 0, "ABCD2", 50, "xxxsigntwo", true);

            Transaction tx1 = new Transaction(hash1, 1, 50, new List<SubTx>() { subInTx }, 46, new List<SubTx>() { subOutTx });
            Transaction tx2 = new Transaction(hash2, 1, 50, new List<SubTx>() { subInTx }, 46, new List<SubTx>() { subOutTx });
            Transaction tx3 = new Transaction(hash3, 1, 50, new List<SubTx>() { subInTx }, 46, new List<SubTx>() { subOutTx });

            this._mempoolRepository.AddTransaction(tx1);
            this._mempoolRepository.AddTransaction(tx2);
            this._mempoolRepository.AddTransaction(tx3);

            return new List<Transaction>() { tx1, tx2, tx3 };
        }
    }
}
