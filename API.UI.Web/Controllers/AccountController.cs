using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using API.Core.Models;
using System.Security.Claims;
using API.Core.Helpers;
using System.IdentityModel.Tokens.Jwt;
using API.Core.HTTPClients;

namespace API.UI.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly LoginClient _loginClient;
        public AccountController(LoginClient loginClient)
        {
            _loginClient = loginClient;
        }

        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            var prevUrl = TempData.FirstOrDefault(x => x.Key == "ReturnUrl");
            if (!prevUrl.Equals(default(KeyValuePair<string, object?>)))
                TempData.Remove(prevUrl);

            if (!string.IsNullOrEmpty(ReturnUrl))
                TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {

            if (ModelState.IsValid)
            {
                var response = await _loginClient.Authenticate(userLogin);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError("Password", "Username or Password wrong!");
                    return View(userLogin);
                }
                // api'den token çekildi, authorization header'ına eklenmeli ki API'da authorization için kullanabilelim
                string token = await response.Content.ReadAsStringAsync();

                #region Token ile user register ediliyor
                JwtSecurityToken jwToken = JWTokenHelper.ReadToken(token);
                DateTime expireDate = jwToken.ValidTo;
                IEnumerable<Claim> claims = jwToken.Claims;
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = expireDate.ToUniversalTime(),
                    AllowRefresh = false,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                #endregion

                // Token cookie'ye koyuluyor
                HttpContext.Response.Cookies.Append("Token", token, new CookieOptions()
                {
                    Secure = false,
                    Expires = expireDate.ToUniversalTime(),
                    HttpOnly = true
                });

                // Kullanıcı tekrar authenticate edilmek için login sayfasına düşmeden önce bir sayfadaysa
                // login olunca tekrar o sayfaya düşecek.
                var returnUrl = TempData["ReturnUrl"]?.ToString();
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Privacy", "Home");
            }

            ModelState.AddModelError("Password", "Username and Password cannot be empty!");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
