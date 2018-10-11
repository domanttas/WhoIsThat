using System.IO;
using NUnit.Framework;
using Shouldly;
using WhoIsThat.Handlers;
using Xamarin.Forms;

namespace WhoIsThatUnitTests
{
    public class ImageHandlerUnitTests
    {
        [Test]
        public void GetStreamFromUri_ShouldReturn()
        {
            //Arrange
            var uri = "https://whoisthatserverimages.blob.core.windows.net/images/Domantas_test.jpg";
            var fakeImageHandler = new ImageHandler();

            //Act
            var result = fakeImageHandler.GetStreamFromUri(uri);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(MemoryStream));
        }

        [Test]
        public void GetImageFromStream_ShouldReturn()
        {
            //Arrange
            var uri = "https://whoisthatserverimages.blob.core.windows.net/images/Domantas_test.jpg";
            var fakeImageHandler = new ImageHandler();
            var memoryStream = fakeImageHandler.GetStreamFromUri(uri);
            
            //Act
            var result = fakeImageHandler.GetImageFromStream(memoryStream);
            
            //Asert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(Image));
        }
    }
}