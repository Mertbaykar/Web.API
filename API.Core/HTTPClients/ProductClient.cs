using API.Core.Bases;
using API.Core.DTOs.Product;
using API.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.HTTPClients
{
    public class ProductClient : HttpClientBase
    {
        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ConstantHelper.ApiUrl+ "/Product");
        }
        public async Task<List<GetProductDTO>> GetProducts()
        {
            var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadFromJsonAsync<List<GetProductDTO>>();
            return result;
        }

        public async Task<GetProductDTO> GetProduct(Guid id)
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress!.ToString() + "/" + id.ToString());
            var result = await response.Content.ReadFromJsonAsync<GetProductDTO>();
            return result;
        }
    }
}
