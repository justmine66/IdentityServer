using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GetIdentity()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5001/api/Values");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var client = new HttpClient())
            {
                var responseMessage = await client.SendAsync(requestMessage);
                responseMessage.EnsureSuccessStatusCode();
                var userInfoResponse = await responseMessage.Content.ReadAsStringAsync();

                return Ok(new { value = userInfoResponse });
            }
        }

        [Authorize]
        public async Task<ActionResult> RefreshToken()
        {
            DiscoveryResponse authorizationServerInfo = await DiscoveryClient.GetAsync("http://localhost:5000/");
            var tokenClient = new TokenClient(authorizationServerInfo.TokenEndpoint, "web.code.client", "secret");
            string refresh_token = await HttpContext.GetTokenAsync("refresh_token");
            TokenResponse response = await tokenClient.RequestRefreshTokenAsync(refresh_token);
            string id_token = await HttpContext.GetTokenAsync("id_token");
            DateTimeOffset expiresAt = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
            
            var tokens = new[] {
                new AuthenticationToken(){
                    Name=OpenIdConnectParameterNames.IdToken,
                    Value=id_token
                },
                new AuthenticationToken(){
                    Name=OpenIdConnectParameterNames.AccessToken,
                    Value=response.AccessToken
                },
                new AuthenticationToken(){
                    Name=OpenIdConnectParameterNames.RefreshToken,
                    Value=response.RefreshToken
                },
                new AuthenticationToken(){
                    Name="expires_at",
                    Value=expiresAt.ToString("o",CultureInfo.InvariantCulture)
                }
            };
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            authenticateResult.Properties.StoreTokens(tokens);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticateResult.Principal, authenticateResult.Properties);
            return new RedirectResult("/");
        }
    }
}
