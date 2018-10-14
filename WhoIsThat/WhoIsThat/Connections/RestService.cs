using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                    throw new Exception("Something went wrong");
                }
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
