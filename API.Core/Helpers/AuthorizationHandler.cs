using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Helpers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpcontextAccessor = httpContextAccessor;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // Headers'a authorization'ı token ile beraber eklemek gerekiyor
                var jwToken = _httpcontextAccessor.HttpContext.Request.Cookies["Token"];
                if (!string.IsNullOrEmpty(jwToken))
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _httpcontextAccessor.HttpContext.Response.Redirect("/Login/Account");
                throw new Exception(ex.Message);
            }

        }
    }
}
