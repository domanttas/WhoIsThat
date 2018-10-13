using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
using WhoIsThat.Handlers.Utils;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.Handlers
{
    public class ImageHandler : IImageHandler
    {
        private readonly IRestService _restService;
        private ICustomWebClientFactory WebClientFactory { get; set; }
        public ICustomWebClient CustomWebClient { get; set; }

        public ImageHandler()
        {
            _restService = new RestService();
            WebClientFactory = new CustomWebClientFactory();
            CustomWebClient = WebClientFactory.Create();
        }

        /// <summary>
        /// Calls method to get all image objects in RestService
        /// </summary>
        /// <returns>Image object list</returns>
        public async Task<List<ImageObject>> GetImageObjects()
        {
            return await _restService.GetImageObjects();
        }
        
        /// <summary>
        /// Creates Image from provided URI (which is present in ImageObject)
        /// Source will be URI, not memory stream
        /// Could be used for displaying only
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

        /// <summary>
        /// Creates Memory Stream from photo in Azure Cloud referenced by URI
        /// </summary>
        /// <param name="uri">URI of image in Azure cloud</param>
        /// <returns>Memory stream of photo</returns>
        public MemoryStream GetStreamFromUri(string uri)
        {
            byte[] imageData = null;
            
            imageData = CustomWebClient.DownloadData(uri);

            return new MemoryStream(imageData);
        }

        /// <summary>
        /// Creates Image object from provided stream
        /// </summary>
        /// <param name="stream">Memory stream of image</param>
        /// <returns>Image object</returns>
        public Image GetImageFromStream(Stream stream)
        {
            var fetchedImage = new Image
            {
                Source = ImageSource.FromStream(() =>
                {
                    return stream;
                })
            };

            return fetchedImage;
        }
    }
}
