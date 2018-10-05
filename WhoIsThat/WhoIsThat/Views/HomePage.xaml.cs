using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
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
            //Requesting permissions to user camera and storage, too lazy to implement checks if user already have those, later 'todo'
            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

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
                
                //Displaying taken photo
                takenPicture.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }

            catch (Exception ex)
            {
                //Not sure if view is a good choice here
                //Not sure if we should log or display caught exception, gotta figure it out
                await DisplayAlert("Something went wrong", "Please try again", "OK");
            }
        }
    }
}