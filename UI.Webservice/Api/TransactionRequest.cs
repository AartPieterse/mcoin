using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UI.Webservice.Models;

namespace UI.Webservice.Api
{
    public class TransactionRequest
    {
        HttpClient client = null;

        public TransactionRequest()
        {
            this.client = new HttpClient();

            this.client.BaseAddress = new Uri("https://localhost:44350/api/");
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsAsync(string pathExtension)
        {
            IEnumerable<TransactionModel> transactions = new List<TransactionModel>();

            HttpResponseMessage response = await this.client.GetAsync(pathExtension);
            if (response.IsSuccessStatusCode)
            {
                transactions = await response.Content.ReadAsAsync<IEnumerable<TransactionModel>>();
            }

            return transactions;
        }

        public async Task<bool> AddTransaction(string pathExtension, TransactionModel transaction)
        {
            HttpResponseMessage response = await this.client.PostAsJsonAsync(pathExtension, transaction);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
