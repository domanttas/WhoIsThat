using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsThat.Models;

namespace WhoIsThat.Connections
{
    public interface IRestService
    {
        Task<List<ImageObject>> GetImageObjects();
    }
}