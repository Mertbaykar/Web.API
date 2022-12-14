using API.Core.HTTPClients;
using API.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Core.Bases
{
    public class HttpClientBase
    {
        protected HttpClient _httpClient;
        protected JsonSerializerOptions jsonSerializerOptions => new JsonSerializerOptions { WriteIndented = true, ReferenceHandler = ReferenceHandler.IgnoreCycles, PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        public Task<HttpResponseMessage> PostAsJsonAsync<TValue>(TValue value, string requestUri = "") where TValue : class
        {
            if (string.IsNullOrEmpty(requestUri))
                return _httpClient.PostAsJsonAsync("", value, jsonSerializerOptions);
            return _httpClient.PostAsJsonAsync(requestUri, value, jsonSerializerOptions);
        }
        public async Task<List<TDTO>> GetAll<TDTO>() where TDTO : class
        {
            //return _httpClient.GetFromJsonAsync<List<TDTO>>("", jsonSerializerOptions);
            var response = await _httpClient.GetAsync("");
            var result = await response.Content.ReadFromJsonAsync<List<TDTO>>();
            return result;
        }
        public async Task<HttpResponseMessage> GetAsyncWithQueryString(
            Dictionary<string, string> queryStringParams, string uri = "")
        {
            if (string.IsNullOrEmpty(uri))
                return await _httpClient.GetWithQueryStringAsync(_httpClient.BaseAddress!.ToString(), queryStringParams);
            return await _httpClient.GetWithQueryStringAsync(uri, queryStringParams);
        }
    }
}
