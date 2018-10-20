using System.IO;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;
using WhoIsThat.Handlers;
using Xamarin.Forms;
using HttpWebRequestWrapper;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using TypeMock;
using WhoIsThat.Handlers.Utils;
using WhoIsThat.ViewModels;

namespace WhoIsThatUnitTests
{
    public class ImageHandlerUnitTests
    {
        [Test]
        public void GetStreamFromUri_ShouldReturnCorrectValue()
        {
            //Arrange
            byte[] fakeBytes = new byte[1000];
            var fakeWebClient = A.Fake<ICustomWebClientFactory>();
            var mockWebClient = fakeWebClient.Create();
            A.CallTo(() => mockWebClient.DownloadData("https://whoisthatserverimages.blob.core.windows.net/images/Domantas_test.jpg")).Returns(fakeBytes);
            
            //Act
            var imageHandler = new ImageHandler()
            {
                CustomWebClient = mockWebClient
            };
            var result =
                imageHandler.GetStreamFromUri(
                    "https://whoisthatserverimages.blob.core.windows.net/images/Domantas_test.jpg");
            
            //Assert
            var resultBytes = result.ToArray();
            
            resultBytes.ShouldBe(fakeBytes);
            
            A.CallTo(() =>
                    mockWebClient.DownloadData(
                        "https://whoisthatserverimages.blob.core.windows.net/images/Domantas_test.jpg"))
                .MustHaveHappenedOnceExactly();
        }

        [TestCase("Domantas")]
        [TestCase("AnyName")]
        [Test]
        public void IsIdentified_ShouldReturnTrue(string message)
        {
            var homeViewModel = new HomeViewModel();
            Assert.IsTrue(homeViewModel.IsIdentified(message));
        }
    }
}