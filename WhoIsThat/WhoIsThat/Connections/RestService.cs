using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Handlers.Utils;
using WhoIsThat.Models;
using HttpClientHandler = WhoIsThat.Handlers.Utils.HttpClientHandler;

namespace WhoIsThat.Connections
{
    public class RestService : IRestService
    {
        public IHttpClientHandler _httpHandler { get; set; }
        public RestService()
        {
            _httpHandler = new HttpClientHandler();
        }

        /// <summary>
        /// Calls ImageObjectElementController in backend and returns list of image objects
        /// </summary>
        /// <returns>List of ImageObject instances</returns>
        public async Task<List<ImageObject>> GetImageObjects()
        {
            try
            {
                string restUrl = "https://teststorageserver.azurewebsites.net/api/images/all";
                var uri = new Uri(string.Format(restUrl, string.Empty));

                var response = await _httpHandler.Get(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ImageObject>>(content);
                }
                else
                {
                    throw new Exception("Something went wrong: " + response.StatusCode);
                }
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<ImageObject> CreateImageObject(ImageObject personObject)
        {
            try
            {
                string restUrl = "https://teststorageserver.azurewebsites.net/api/images/add";
                var uri = new Uri(string.Format(restUrl, string.Empty));

                var response = await _httpHandler.Post(uri, personObject);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ImageObject>(content);
                }
                else
                {
                    throw new Exception("Something went wrong: " + response.StatusCode);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public async Task<string> Identify()
        {
            string restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/identify";
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var response = await _httpHandler.Get(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string>(content);
            }

            else
            {
                throw new Exception("Something went wrong with recognition: " + response.StatusCode);
            }
        }


    }
}
