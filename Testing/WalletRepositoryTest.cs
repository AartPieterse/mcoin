using Core.Domain;
using Infrastructure;
using System;
using Xunit;

namespace Testing
{
    public class WalletRepositoryTest
    {
        private readonly WalletRepository _walletRepository;

        public WalletRepositoryTest()
        {
            this._walletRepository = new WalletRepository(new WalletFileContext("TestWalletData", "TestData"));

            this._walletRepository.SaveWallet(new Wallet("PUBLICKEY", "PRIVATEKEY", 0));
        }

        [Fact]
        public void SaveWallet()
        {
            // Check if file is not empty
            Wallet oldWallet = this._walletRepository.GetWallet();
            Assert.True(oldWallet.Balance >= 0);

            // Create new wallet
            Wallet newWallet = new Wallet();
            newWallet.PublicKey = "NonSecretPublicKey";
            newWallet.PrivateKey = "VerySecretPrivateKey";
            newWallet.Balance = 50;

            // Save created wallet
            this._walletRepository.SaveWallet(newWallet);

            // Check if wallet is correctly created
            Wallet createdWallet = this._walletRepository.GetWallet();
            Assert.Equal(newWallet.PrivateKey, createdWallet.PrivateKey);
            Assert.Equal(newWallet.PublicKey, createdWallet.PublicKey);
            Assert.Equal(newWallet.Balance, createdWallet.Balance);
        }

        [Fact]
        public void GetWallet()
        {
            // Get the wallet
            Wallet wallet = this._walletRepository.GetWallet();

            // Check if wallet is correct
            Assert.NotNull(wallet.PrivateKey);
            Assert.NotNull(wallet.PublicKey);
            Assert.True(wallet.Balance >= 0);
        }

        [Fact]
        public void HasWallet()
        {
            // Retrieve wallet
            Boolean hasWallet = this._walletRepository.HasWallet();

            // Check if not exists
            Assert.False(hasWallet);
        }
    }
}
