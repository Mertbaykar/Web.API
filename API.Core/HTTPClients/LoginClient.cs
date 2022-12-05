using API.Core.Bases;
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
    public class LoginClient: HttpClientBase
    {
        public LoginClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ConstantHelper.ApiUrl+ "/Login");
        }
        public Task<HttpResponseMessage> Authenticate<TValue>(TValue value) where TValue : class
        {
            return _httpClient.PostAsJsonAsync("", value, jsonSerializerOptions);
        }
    }
}
