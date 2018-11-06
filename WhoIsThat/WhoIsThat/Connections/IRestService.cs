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

        /// <summary>
        /// Identifies person by photo
        /// </summary>
        /// <returns>ID or error message which is handled when calling this method</returns>
        Task<string> Identify();

        /// <summary>
        /// Gets user by ID
        /// </summary>
        /// <param name="id">ID of user</param>
        /// <returns>User object</returns>
        Task<ImageObject> GetUserById(int id);

        /// <summary>
        /// Inserts user into person group for recognition. Used during registration
        /// </summary>
        /// <param name="user">User to insert</param>
        /// <returns>True if user was inserted</returns>
        Task<bool> InsertUserIntoRecognition(ImageObject user);

        /// <summary>
        /// Assigns random target
        /// </summary>
        /// <param name="id">ID of current user</param>
        /// <returns>Assigned target's ID</returns>
        Task<int> GetRandomTarget(int id);
    }
}