using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.Handlers
{
    public class ImageHandler
    {
        private RestService restService;

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
        /*
        public Image GetImageFromUri(string uri)
        {
            var fetchedImage = new Image
            {
                Source = ImageSource.FromUri(new Uri(uri))
            };

            return fetchedImage;
        }
        */
    }
}
