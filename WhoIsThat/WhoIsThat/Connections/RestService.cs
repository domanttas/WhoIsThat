using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WhoIsThat.Connections
{
    public class RestService
    {
        HttpClient Client { get; set; }

        public RestService()
        {
            Client = new HttpClient();
        }
    }
}
