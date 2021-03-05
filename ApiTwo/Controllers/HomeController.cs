using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    public class HomeController :Controller
    {
        private readonly IHttpClientFactory _httpCleintFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpCleintFactory = httpClientFactory;
        }


        [Route("/home")]
        public async Task<IActionResult> Index()
        {
            var serverClient = _httpCleintFactory.CreateClient();
            //well known endpoint^^
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44305/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest { 
                Address=discoveryDocument.TokenEndpoint,
                ClientId="client_id",
                ClientSecret="client_secret",
                Scope="ApiOne"
            });

            var apiClient = _httpCleintFactory.CreateClient();

            //vazoume to token
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44374/secret");

            var content = response.Content.ReadAsStringAsync();

            return Json(new {access_token=tokenResponse.AccessToken,message=content });
        } 
    }
}
