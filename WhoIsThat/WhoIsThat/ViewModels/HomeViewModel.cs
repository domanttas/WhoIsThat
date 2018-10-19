using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using WhoIsThat.Connections;
using WhoIsThat.Handlers;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public ICommand TakePhotoCommand { get; private set; }

        public ImageSource DisplayStream { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayReturnedName { get; set; }
        public string DisplayMessage { get; set; }

        public HomeViewModel()
        {
            TakePhotoCommand = new Command(TakePhoto);
        }

        public async void TakePhoto()
        {
            DisplayReturnedName = "Please wait...";
            OnPropertyChanged("DisplayReturnedName");

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

            await CloudStorageService.SaveBlockBlob(takenPhoto);
            
            DisplayStream = ImageSource.FromStream(() =>
            {
                var stream = takenPhoto.GetStream();
                takenPhoto.Dispose();
                return stream;
            });
            
            OnPropertyChanged("DisplayStream");

            RestService restService = new RestService();
            var recognizedName = await restService.Identify();
            DisplayReturnedName = recognizedName;
            OnPropertyChanged("DisplayReturnedName");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool IsIdentified(string message)
        {
            if (message == "No faces were detected!" || message == "No one was indetified!")
            {
                return false;
            }

            return true;
        }
    }
}
