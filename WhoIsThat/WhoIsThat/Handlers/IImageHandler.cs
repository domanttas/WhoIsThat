using System.Collections.Generic;
using System.IO;
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
        /// Source will be URI, not memory stream
        /// Could be used for displaying only
        /// </summary>
        /// <param name="uri">URI of image in Azure cloud</param>
        /// <returns>Variable of type Image which contains Source of image in Azure cloud</returns>
        Image GetImageFromUri(string uri);

        /// <summary>
        /// Creates Memory Stream from photo in Azure Cloud referenced by URI
        /// </summary>
        /// <param name="uri">URI of image in Azure cloud</param>
        /// <returns>Memory stream of photo</returns>
        MemoryStream GetStreamFromUri(string uri);

        /// <summary>
        /// Creates Image object from provided stream
        /// </summary>
        /// <param name="stream">Memory stream of image</param>
        /// <returns>Image object</returns>
        Image GetImageFromStream(Stream stream);
    }
}