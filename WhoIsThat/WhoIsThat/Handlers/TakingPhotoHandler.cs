﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.ConstantsUtil;
using WhoIsThat.Exceptions;

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
            //Initializing media hardware
            await CrossMedia.Current.Initialize();

            //Taking picture and storing it in default directory which variable file refers to
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    //Directory = "Sample",
                    //Name = "test.jpg"
                });

            if (file == null)
            {
                throw new ManagerException(Constants.PhotoNotTakenError);
            }
                
            return file;
        }
    }
}
