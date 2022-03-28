using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UI.Webservice.Models;

namespace UI.Webservice.Api
{
    public class BlockRequest
    {
        HttpClient client = null;

        public BlockRequest()
        {
            this.client = new HttpClient();

            this.client.BaseAddress = new Uri("https://localhost:44350/api/");
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<BlockModel>> GetBlocksAsync(string path)
        {
            IEnumerable<BlockModel> blocks = new List<BlockModel>();

            Debug.WriteLine(this.client.BaseAddress + path, "Do Request: ");

            HttpResponseMessage response = await this.client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                blocks = await response.Content.ReadAsAsync<IEnumerable<BlockModel>>();
            }

            Debug.WriteLine("Request Finished", response);

            return blocks;
        }

        public async Task<BlockModel> GetBlockDetailsAsync(string path)
        {
            Debug.WriteLine(this.client.BaseAddress + path);

            HttpResponseMessage response = await this.client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<BlockModel>();
            }

            Debug.WriteLine(response);

            return null;
        }

        public async Task<bool> AddBlockAsync(string path)
        {
            HttpResponseMessage response = await this.client.PostAsJsonAsync(path, "value");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
