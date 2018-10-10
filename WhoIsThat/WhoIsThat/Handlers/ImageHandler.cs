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

        public async Task<List<ImageObject>> GetImageObjects()
        {
            return await restService.GetImageObjects();
        }
    }
}
