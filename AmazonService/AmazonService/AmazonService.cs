using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataHakHomeProject.Service.AmazonService
{

    public class AmazonService : IAmazonService
    {
        private readonly HttpClient _httpClient;
        public AmazonService(HttpClient client) 
        {
            _httpClient = client;
        }

        public async Task<string> GetPageAsync(string asin, int page = 1)
        {
            var response = await _httpClient.GetAsync(asin + $"/ref=cm_cr_arp_d_viewopt_srt?sortBy=recent&pageNumber={page}");
            var pageContent = await response.Content.ReadAsStringAsync();

            return pageContent;
        }
    }
}
