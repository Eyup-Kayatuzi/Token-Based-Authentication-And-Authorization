using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebMvcForRequestToApi.Services;
using WebMvcForRequestToApi.ViewModel;

namespace WebMvcForRequestToApi.Controllers
{
    public class BookRequestController : Controller
    {
        private readonly IHttpClientServiceImplementation _customeHttpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookRequestController(IHttpClientServiceImplementation customeHttpClient, IHttpClientFactory httpClientFactory)
        {
            _customeHttpClient = customeHttpClient;
            _httpClientFactory = httpClientFactory;
        }
        //public async Task<IActionResult> GetBook()
        //{
        //    //var returnedVal =  _customeHttpClient.SendRequestWithDefaultAdj("https://localhost:7193/api/", "Books");
        //    var returnedVal = await _customeHttpClient.SendRequestWithHttpRequestMessage("https://localhost:7193/api/", "Books");
        //    var retunedBooks = JsonConvert.DeserializeObject<List<BookVm>>(returnedVal);
        //    return View(retunedBooks);
        //}
        public async Task<IActionResult> GetBookWithAuth()
        {

            var returnedVal = await _customeHttpClient.SendRequestWithDefaultAdj("https://localhost:7193/api/", "Books", HttpContext.Session.GetString("JwtToken"));
            var retunedBooks = JsonConvert.DeserializeObject<List<BookVm>>(returnedVal);
            return View(retunedBooks);
        }
        public async Task<IActionResult> GetBookByhttpClientFactory()
        {
            //var returnedVal =  _customeHttpClient.SendRequestWithDefaultAdj("https://localhost:7193/api/", "Books");
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7193/api/Books");
            var retunedBooks = JsonConvert.DeserializeObject<List<BookVm>>(await response.Content.ReadAsStringAsync());
            return View(retunedBooks);
        }
        //public IActionResult SendBook()
        //{
        //    return View(); 
        //}
        public IActionResult SendBookWithAuth()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> SendBook(AddNewBookToVM addNewBookToVM)
        //{
        //    var returnedVal = await _customeHttpClient.SendRequestWithDefAdjForAddingBook("https://localhost:7193/api/", "Books", addNewBookToVM);
        //    return RedirectToAction("GetBook");
        //}
        [HttpPost]
        public async Task<IActionResult> SendBookWithAuth(AddNewBookToVM addNewBookToVM)
        {
            var returnedVal = await _customeHttpClient.SendRequestWithDefAdjForAddingBook("https://localhost:7193/api/", "Books", addNewBookToVM, HttpContext.Session.GetString("JwtToken"));
            return RedirectToAction("GetBookWithAuth");
        }

    }
}
