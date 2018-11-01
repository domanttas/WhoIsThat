using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Models;

namespace WhoIsThat.Handlers.Utils
{
    public class HttpClientHandler : IHttpClientHandler
    {
        public HttpClient httpClient;

        public HttpClientHandler()
        {
            httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> Get(Uri restUrl)
        { 
            return await httpClient.GetAsync(restUrl);
        }

        public async Task<HttpResponseMessage> Post(Uri uri, ImageObject imageObject)
        {
            return await httpClient.PostAsJsonAsync(uri, imageObject);
        }
    }
}
//, new JsonMediaTypeFormatter()