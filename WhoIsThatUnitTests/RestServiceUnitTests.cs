using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WhoIsThat.Connections;
using Windows.Media.Protection.PlayReady;
using HttpClient = System.Net.Http.HttpClient;

namespace WhoIsThatUnitTests
{
    class RestServiceUnitTests
    {
        public async void GetImageObjects_()
        {
            //Assert
            var fakeResponse = new System.Net.Http.HttpResponseMessage();
            var fakeClient = A.Fake<HttpClient>();
            var restUrl = "https://teststorageserver.azurewebsites.net/api/images/all";
            A.CallTo(() => fakeClient.GetAsync(string.Format(restUrl, string.Empty))).Returns(fakeResponse);
            var restService = new RestService()
            {
                Client = fakeClient
            };

            //Act
            var result = await restService.GetImageObjects();

        }
    }
}
