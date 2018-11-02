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
        public HttpClient HttpClient { get; set; }
        public RestService()
        {
            HttpClient = new HttpClient();;
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

                var response = await HttpClient.GetAsync(uri);
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
            string restUrl = "https://teststorageserver.azurewebsites.net/api/images/add";
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var jsonContent = JsonConvert.SerializeObject(personObject, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = (await HttpClient.SendAsync(request)).EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ImageObject>(responseContent);

            return responseObject;
        }

        public async Task<string> Identify()
        {
            string restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/identify";
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var response = await HttpClient.GetAsync(uri);
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

        public async Task<ImageObject> GetUserById(int id)
        {
            string restUrl = "https://teststorageserver.azurewebsites.net/api/images/user/" + id;
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var response = await HttpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ImageObject>(content);
            }

            else
            {
                //NOTE: TO BE CHANGED WITH CUSTOM ERROR HANDLING LATER ON!!!!
                throw new Exception("Something went wrong with DB: " + response.StatusCode);
            }
        }

        public async Task<bool> InsertUserIntoRecognition(ImageObject user)
        {
            var uri = "https://testrecognition.azurewebsites.net/api/recognitionservices/insert";
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
                uri, user);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<bool>(responseContent);
            return responseObject;
        }
    }
}
