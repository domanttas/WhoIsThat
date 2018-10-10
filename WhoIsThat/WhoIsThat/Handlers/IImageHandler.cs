using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.Handlers
{
    public interface IImageHandler
    {
        /// <summary>
        /// Calls method to get all image objects in RestService
        /// </summary>
        /// <returns>Image object list</returns>
        Task<List<ImageObject>> GetImageObjects();

        /// <summary>
        /// Creates Image from provided URI (which is present in ImageObject)
        /// </summary>
        /// <param name="uri">URI of image in Azure cloud</param>
        /// <returns>Variable of type Image which contains Source of image in Azure cloud</returns>
        Image GetImageFromUri(string uri);
    }
}