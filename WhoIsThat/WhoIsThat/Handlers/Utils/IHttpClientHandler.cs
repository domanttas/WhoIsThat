using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Models;

namespace WhoIsThat.Handlers.Utils
{
    public interface IHttpClientHandler
    {
        Task<HttpResponseMessage> Get(Uri url);

        Task<HttpResponseMessage> Post(Uri url, ImageObject imageObject);
    }
}
