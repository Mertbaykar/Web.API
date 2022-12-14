using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient client, string uri,
            Dictionary<string, string> queryStringParams)
        {
            var url = QueryHelpers.AddQueryString(uri, queryStringParams);

            return await client.GetAsync(url);
        }
    }
}
