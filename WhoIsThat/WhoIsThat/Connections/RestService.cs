using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.ConstantsUtil;
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

        /// <inheritdoc/>
        public async Task<List<ImageObject>> GetImageObjects()
        {
            try
            {
                const string restUrl = "https://teststorageserver.azurewebsites.net/api/images/all";
                var uri = new Uri(string.Format(restUrl, string.Empty));

                var response = await HttpClient.GetAsync(uri);
                if (!response.IsSuccessStatusCode) throw new Exception();
                
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ImageObject>>(content);
            }

            catch (Exception exception)
            {
                //After using this API call in every code part should be checked if it's null and FatalStorageError message should be displayed
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<ImageObject> CreateImageObject(ImageObject personObject)
        {
            const string restUrl = "https://teststorageserver.azurewebsites.net/api/images/add";
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
            try
            {
                var responseObject = JsonConvert.DeserializeObject<ImageObject>(responseContent);
                return responseObject;
            }

            catch (JsonException jsonException)
            {
                //Mapped error message needs to be displayed after checking
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<string> Identify()
        {
            const string restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/identify";
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return Constants.FatalRecognitionError;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(content);

        }

        /// <inheritdoc/>
        public async Task<ImageObject> GetUserById(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/images/user/" + id;
            var uri = new Uri(string.Format(restUrl, string.Empty));

            //Mapped error message needs to be displayed after checking
            
            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return null;
            
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageObject>(content);

        }

        /// <inheritdoc/>
        public async Task<bool> InsertUserIntoRecognition(ImageObject user)
        {
            const string uri = "https://testrecognition.azurewebsites.net/api/recognitionservices/insert";
            var response = await HttpClient.PostAsJsonAsync(
                uri, user);

            if (!response.IsSuccessStatusCode) return false;
            
            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var responseObject = JsonConvert.DeserializeObject<bool>(responseContent);
                return responseObject;
            }

            catch (JsonException jsonException)
            {
                //Mapped error message needs to be displayed after checking
                return false;
            }
        }
    }
}
