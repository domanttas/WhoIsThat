using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

        private async void takePicture(object sender, EventArgs e)
        {
            //Requesting permissions to user camera and storage, too lazy to implement checks if user already have those, later 'todo'
            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

            //Initializing media hardware
            await CrossMedia.Current.Initialize();

            //Checking if camera is available, if not -> return
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", "No camera available.", "OK");
                return;
            }

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
                return;
            }

            //Displaying taken photo
            takenPicture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
	}
}