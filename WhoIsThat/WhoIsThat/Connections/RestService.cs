using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections.ErrorModels;
using WhoIsThat.ConstantsUtil;
using WhoIsThat.Exceptions;
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
            const string restUrl = "https://teststorageserver.azurewebsites.net/api/images/all";
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ImageObject>>(content);  
        }

        /// <inheritdoc/>
        public async Task<ImageObject> CreateImageObject(ImageObject personObject)
        {
            const string restUrl = "https://teststorageserver.azurewebsites.net/api/images/add";
            var uri = new Uri(restUrl);

            var jsonContent = JsonConvert.SerializeObject(personObject, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<ImageObject>(responseContent);
            return responseObject;
        }

        /// <inheritdoc/>
        public async Task<string> Identify()
        {
            const string restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/identify";
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new ManagerException(await response.Content.ReadAsStringAsync());
            }
 
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(content);
        }

        /// <inheritdoc/>
        public async Task<ImageObject> GetUserById(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/images/user/" + id;
            var uri = new Uri(restUrl);
            
            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageObject>(content);
        }

        /// <inheritdoc/>
        public async Task<bool> InsertUserIntoRecognition(ImageObject user)
        {
            const string uri = "https://testrecognition.azurewebsites.net/api/recognitionservices/insert";
            var response = await HttpClient.PostAsJsonAsync(
                uri, user);

            if (!response.IsSuccessStatusCode)
            {
                throw new ManagerException(await response.Content.ReadAsStringAsync());
            }
            
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var responseObject = JsonConvert.DeserializeObject<bool>(responseContent);
            return responseObject;
        }

        /// <inheritdoc/>
        public async Task<int> GetRandomTarget(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/game/" + id;
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<TargetObject> GetCurrentTarget(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/game/element/" + id;
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TargetObject>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<bool> IsPreyHunted(int hunterId, int preyId)
        {
            var requestObject = new TargetObject()
            {
                HunterPersonId = hunterId,
                PreyPersonId = preyId,
                IsHunted = false
            };

            const string restUrl = "https://teststorageserver.azurewebsites.net/api/game/remove";
            var uri = new Uri(restUrl);

            var jsonContent = JsonConvert.SerializeObject(requestObject, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<ImageObject> UpdateUserScore(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/images/score/" + id;
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageObject>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<FaceFeaturesModel> GetFaceFeatures(ImageObject user)
        {
            var restUrl = "https://testrecognition.azurewebsites.net/api/recognitionservices/detect";
            var uri = new Uri(restUrl);

            var response = await HttpClient.PostAsJsonAsync(
                uri, user);

            if (!response.IsSuccessStatusCode)
            {
                throw new ManagerException(await response.Content.ReadAsStringAsync());
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FaceFeaturesModel>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<FaceFeaturesModel> InsertFaceFeatures(FaceFeaturesModel faceFeaturesModel)
        {
            const string restUrl = "https://teststorageserver.azurewebsites.net/api/features";
            var uri = new Uri(restUrl);

            var jsonContent = JsonConvert.SerializeObject(faceFeaturesModel, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = await HttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FaceFeaturesModel>(responseContent);
        }

        /// <inheritdoc/>
        public async Task<FaceFeaturesModel> GetFeaturesById(int id)
        {
            var restUrl = "https://teststorageserver.azurewebsites.net/api/features" + id;
            var uri = new Uri(restUrl);

            var response = await HttpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ManagerException((JsonConvert.DeserializeObject<BadRequestModel>(errorContent)).Message);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FaceFeaturesModel>(responseContent);
        }
    }
}
