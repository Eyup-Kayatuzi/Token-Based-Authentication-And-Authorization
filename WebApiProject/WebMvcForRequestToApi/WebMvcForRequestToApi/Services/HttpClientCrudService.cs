using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebMvcForRequestToApi.ViewModel;

namespace WebMvcForRequestToApi.Services
{
    public class HttpClientCrudService : IHttpClientServiceImplementation
    {
        private readonly HttpClient _httpClient;
        public HttpClientCrudService()
        {
            _httpClient = new HttpClient();
        }
        //public async Task<string> SendRequestWithDefaultAdj(string baseAddress, string controllerName)
        //{
        //    //HttpClient _httpClient = new HttpClient();
        //    _httpClient.BaseAddress = new Uri(baseAddress);
        //    _httpClient.Timeout = new TimeSpan(0, 0, 30);
        //    var response = await _httpClient.GetAsync(controllerName);
        //    response.EnsureSuccessStatusCode();
        //    var content = await response.Content.ReadAsStringAsync();
        //    return content;
        //}
        public async Task<string> SendRequestWithDefaultAdj(string baseAddress, string controllerName, string token)
        {
            //HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.GetAsync(controllerName);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        //public async Task<string> SendRequestWithDefAdjForAddingBook(string baseAddress, string controllerName, object newBook)
        //{
        //    //HttpClient _httpClient = new HttpClient();
        //    _httpClient.BaseAddress = new Uri(baseAddress);
        //    var newValue = JsonSerializer.Serialize(newBook);
        //    var requestContent = new StringContent(newValue, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync(controllerName, requestContent);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        return content;
        //    }
        //    else
        //    {
        //        return null;
        //    }


        //}

        public async Task<string> SendRequestWithDefAdjForSignIn(string baseAddress, string controllerName, object newBook)
        {
            //HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
            var newValue = JsonSerializer.Serialize(newBook);
            var requestContent = new StringContent(newValue, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(controllerName, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                return null;
            }


        }
        public async Task<string> SendRequestWithDefAdjForAddingBook(string baseAddress, string controllerName, object newBook, string token)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
            var newValue = JsonSerializer.Serialize(newBook);
            var requestContent = new StringContent(newValue, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.PostAsync(controllerName, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                return null;
            }
        }
        public async Task<string> SendRequestWithHttpRequestMessage(string baseAddress, string controllerName)
        {
            //HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            var request = new HttpRequestMessage(HttpMethod.Get, controllerName);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}
