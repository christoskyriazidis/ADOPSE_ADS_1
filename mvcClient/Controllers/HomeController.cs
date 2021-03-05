using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpCleintFactory;

        public IActionResult Index()
        {
            return View();
        }
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpCleintFactory = httpClientFactory;
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = HttpContext.GetTokenAsync("refresh_token");
            //authenticaiton informatio
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var claims = User.Claims;

            var result = await GetSecret(accessToken);

            return Json(new { secret = User.Identity.Name});
        }

        public async Task<string> GetSecret(string acessToken)
        {
            var apiClient = _httpCleintFactory.CreateClient();

            //vazoume to token
            apiClient.SetBearerToken(acessToken);

            var response = await apiClient.GetAsync("https://localhost:44374/secret");

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        private async Task RefreshAccessToken()
        {
            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //ep 13...


        }
    }
}
