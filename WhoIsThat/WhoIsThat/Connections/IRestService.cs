using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsThat.Models;

namespace WhoIsThat.Connections
{
    public interface IRestService
    {
        /// <summary>
        /// Calls ImageObjectElementController in backend and returns list of image objects
        /// </summary>
        /// <returns>List of ImageObject instances</returns>
        Task<List<ImageObject>> GetImageObjects();

        /// <summary>
        /// Creates user in backend
        /// </summary>
        /// <param name="personObject">User to create</param>
        /// <returns>Created user with new ID</returns>
        Task<ImageObject> CreateImageObject(ImageObject personObject);
    }
}