using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [Route("Secret")]
        public async Task<IActionResult> Secret()
        {
            var access_token = await HttpContext.GetTokenAsync("access_token");
            var id_token = await HttpContext.GetTokenAsync("id_token");
            var refresh_token = await HttpContext.GetTokenAsync("refresh");


            var id_handler = new JwtSecurityTokenHandler()
                .ReadJwtToken(id_token);
            var access_handler = new JwtSecurityTokenHandler()
                .ReadJwtToken(access_token);

            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(access_token);
            var res = await apiClient.GetAsync("https://localhost:7246/secret");
            var con = await res.Content.ReadAsStringAsync();
            ViewBag.message = con;
            return View();
        }
    }
}