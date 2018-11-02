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
using WhoIsThat.Models;
using WhoIsThat.Handlers.Utils;
using NUnit.Framework;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;
using Newtonsoft.Json;
using Shouldly;

namespace WhoIsThatUnitTests
{
    class RestServiceUnitTests
    {
        [Test]
        public void CreateImageObject_ShouldReturnTheSameValues()
        {
            //Assert
            var imageObject = new ImageObject()
            {
                Id = 0,
                ImageContentUri = "t",
                ImageName = "t",
                PersonFirstName = "t",
                PersonLastName = "t",
                DescriptiveSentence = "t",
                Score = 0
            };
            var expectedJson = JsonConvert.SerializeObject(imageObject);
            HttpResponseMessage expectedResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(expectedJson)
            };
            var fakeHandler = A.Fake<IHttpClientHandler>();
            var url = "https://teststorageserver.azurewebsites.net/api/images/add";
            var uri = new Uri(String.Format(url, string.Empty));
            A.CallTo(() => fakeHandler.Post(uri, imageObject)).Returns(expectedResponse);

            RestService service = new RestService()
            {
                _httpHandler = fakeHandler
            };
            
            var response = service.CreateImageObject(imageObject);
            var responseJson = JsonConvert.SerializeObject(response.Result);
            A.CallTo(() => fakeHandler.Post(uri, imageObject)).MustHaveHappenedOnceExactly();
            responseJson.ShouldBe(expectedJson);
            Assert.AreEqual(response.Result.Id, imageObject.Id);
            Assert.AreEqual(response.Result.PersonFirstName, imageObject.PersonFirstName);
            Assert.AreEqual(response.Result.PersonLastName, imageObject.PersonLastName);
            Assert.AreEqual(response.Result.Score, imageObject.Score);
            Assert.AreEqual(response.Result.DescriptiveSentence, imageObject.DescriptiveSentence);
        }

    }
}
