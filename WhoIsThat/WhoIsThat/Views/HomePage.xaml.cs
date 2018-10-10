using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
using WhoIsThat.Handlers;
using WhoIsThat.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent();
		}

        private async void TakePicture(object sender, EventArgs e)
        {
            //Checking for camera permissions
            bool cameraPermission = await PermissionHandler.CheckForCameraPermission();
            if (!cameraPermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            //Checking for storage permissions
            bool storagePermission = await PermissionHandler.CheckForCameraPermission();
            if (!storagePermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

            //Taking photo and storing it in MediaFile variable 'takenPhoto'
            MediaFile takenPhoto = await TakingPhotoHandler.TakePhoto();

            //For testing purposes displaying it
            takenPicture.Source = ImageSource.FromStream(() =>
            {
                var stream = takenPhoto.GetStream();
                takenPhoto.Dispose();
                return stream;
            });
        }
    }
}