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

        /// <summary>
        /// Gets target if it is present
        /// </summary>
        /// <param name="id">ID of hunter</param>
        /// <returns>TargetObject</returns>
        Task<TargetObject> GetCurrentTarget(int id);

        /// <summary>
        /// Checks whether hunter hit their target
        /// </summary>
        /// <param name="hunterId">ID of hunter (current user)</param>
        /// <param name="preyId">ID of prey returned from recognition</param>
        /// <returns>Boolean</returns>
        Task<bool> IsPreyHunted(int hunterId, int preyId);

        /// <summary>
        /// Updates user score
        /// </summary>
        /// <param name="id">ID of user</param>
        /// <returns>Updated object</returns>
        Task<ImageObject> UpdateUserScore(int id);

        /// <summary>
        /// Gets face features of person
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Face features model</returns>
        Task<FaceFeaturesModel> GetFaceFeatures(ImageObject user);

        /// <summary>
        /// Inserts features from recognition into DB
        /// </summary>
        /// <param name="faceFeaturesModel">Features from recognition</param>
        /// <returns>Inserted object</returns>
        Task<FaceFeaturesModel> InsertFaceFeatures(FaceFeaturesModel faceFeaturesModel);

        /// <summary>
        /// Gets features of target
        /// </summary>
        /// <param name="id">ID of target</param>
        /// <returns>Face features object</returns>
        Task<FaceFeaturesModel> GetFeaturesById(int id);
    }
}