using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.Handlers
{
    public class ImageHandler : IImageHandler
    {
        private IRestService restService;

        public ImageHandler()
        {
            restService = new RestService();
        }

        /// <summary>
        /// Calls method to get all image objects in RestService
        /// </summary>
        /// <returns>Image object list</returns>
        public async Task<List<ImageObject>> GetImageObjects()
        {
            return await restService.GetImageObjects();
        }
        
        /// <summary>
        /// Creates Image from provided URI (which is present in ImageObject)
        /// </summary>
        /// <param name="uri">URI of image in Azure cloud</param>
        /// <returns>Variable of type Image which contains Source of image in Azure cloud</returns>
        public Image GetImageFromUri(string uri)
        {
            var fetchedImage = new Image
            {
                Source = ImageSource.FromUri(new Uri(uri))
            };

            return fetchedImage;
        }
        
    }
}
