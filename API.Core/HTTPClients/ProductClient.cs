using API.Core.Bases;
using API.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    }
}
