using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Models;

namespace WhoIsThat.Connections
{
    public class RestService : IRestService
    {
        private HttpClient Client { get; set; }

        public RestService()
        {
            Client = new HttpClient();
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

                var response = await Client.GetAsync(uri);
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

<<<<<<< HEAD

=======
        public async void PutImageObjetToDb()
        {
            try
            {
                string restUrl = "https://teststorageserver.azurewebsites.net/api/images/all";
                var uri = new Uri(string.Format(restUrl, string.Empty));
            }
        }
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)
        public async Task<string> Identify()
        {
            string restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/identify";
            var uri = new Uri(string.Format(restUrl, string.Empty));

            var response = await Client.GetAsync(uri);
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
