using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebMvcForRequestToApi.Services;
using WebMvcForRequestToApi.ViewModel;

namespace WebMvcForRequestToApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientServiceImplementation _customeHttpClient;
        public AuthController(IHttpClientServiceImplementation customeHttpClient)
        {
            _customeHttpClient = customeHttpClient;
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM signInVM)
        {
            var returnedVal = await _customeHttpClient.SendRequestWithDefAdjForSignIn("https://localhost:7193/api/Auth/", "SignIn", signInVM);
            var tokenObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(returnedVal);
            var token = tokenObject["token"];
            var expires = tokenObject["expiration"];
            HttpContext.Session.SetString("JwtToken", token);
            return RedirectToAction("GetBookWithAuth", "BookRequest");
            //return RedirectToAction("GetBook", "BookRequest");
        }
    }
}
