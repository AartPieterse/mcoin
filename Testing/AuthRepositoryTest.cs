using Infrastructure;
using System;
using System.IO;
using Xunit;

namespace Testing
{
    public class AuthRepositoryTest
    {
        private readonly AuthRepository _authRepository;
        private String PathString;

        private const String TEST_PASSWORD = "thisismypassword";

        public AuthRepositoryTest()
        {
            this._authRepository = new AuthRepository(new AuthFileContext("TestAuthData", "TestData"));

            this.CreateFileIfNotExists();

            this.AddDataToFileForTestingPurposes();
        }

        [Fact]
        public void SavePassword()
        {
            String password = "thisisanewpassword";

            // Save a new password
            this._authRepository.SavePassword(password);

            // Check if new password is saved
            String line;

            StreamReader reader;
            using (reader = new StreamReader(this.PathString))
            {
                line = reader.ReadLine();
            }

            // Check if saved password equals read password
            Assert.Equal(password, line);
        }

        [Fact]
        public void GetPassword()
        {
            // Get password
            String password = this._authRepository.GetPassword();

            // Check if exists && is correct
            Assert.NotNull(password);
            Assert.Equal(TEST_PASSWORD, password);
        }

        private void AddDataToFileForTestingPurposes()
        {
            using (StreamWriter writer = new StreamWriter(this.PathString))
            {
                writer.WriteLine(TEST_PASSWORD);
            }
        }

        private void CreateFileIfNotExists()
        {
            this.PathString = Directory.GetCurrentDirectory().Substring(0, 40);

            this.PathString = Path.Combine(this.PathString, "TestData");

            Directory.CreateDirectory(this.PathString);

            this.PathString = Path.Combine(this.PathString, "TestAuthData.txt");

            if (!File.Exists(this.PathString))
            {
                File.Create(this.PathString);
            }
        }
    }
}
