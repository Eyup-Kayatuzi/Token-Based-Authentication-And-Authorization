using System.Threading.Tasks;
using WebMvcForRequestToApi.ViewModel;

namespace WebMvcForRequestToApi.Services
{
    public interface IHttpClientServiceImplementation
    {
         //Task<string> SendRequestWithDefaultAdj(string baseAddress, string controllerName);
        Task<string> SendRequestWithDefaultAdj(string baseAddress, string controllerName, string token);
        //Task<string> SendRequestWithHttpRequestMessage
        //    (string baseAddress, string controllerName);

        //Task<string> SendRequestWithDefAdjForAddingBook(string baseAddress, string controllerName, object newBook);
        Task<string> SendRequestWithDefAdjForSignIn(string baseAddress, string controllerName, object newBook);
        Task<string> SendRequestWithDefAdjForAddingBook(string baseAddress, string controllerName, object newBook, string token);
    }
}
