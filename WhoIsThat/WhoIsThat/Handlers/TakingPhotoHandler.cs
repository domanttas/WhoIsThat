using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsThat.Handlers
{
    public class TakingPhotoHandler
    {
        /// <summary>
        /// Takes photo using CrossMedia plugin and returns MediaFile of it
        /// </summary>
        /// <returns>MediaFile</returns>
        public static async Task<MediaFile> TakePhoto()
        {
            try
            {
                //Initializing media hardware
                await CrossMedia.Current.Initialize();

                //Taking picture and storing it in default directory which variable file refers to
                var file = await CrossMedia.Current.TakePhotoAsync(
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        //Directory = "Sample",
                        //Name = "test.jpg"
                    });

                if (file == null)
                {
                    throw new ArgumentException("Photo was not successfully taken", "MediaFile");
                }
                
                return file;
            }

            catch (Exception exception)
            {
                //Not sure if view is a good choice here
                //Not sure if we should log or display caught exception, gotta figure it out
                throw exception;
            }
        }
    }
}
