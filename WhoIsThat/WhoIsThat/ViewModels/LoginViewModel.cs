using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using WhoIsThat.Connections;
using WhoIsThat.Handlers;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateHomePageCommand { get; private set; }
        public ICommand TakePhotoCommand { get; set; }

        public LoginViewModel()
        {
            NavigateHomePageCommand = new Command(NavigateToHomePage);
            TakePhotoCommand = new Command(TakePhoto);
            PersonObject = new ImageObject();
        }
        

        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public string test { get; set; }
                
        private MediaFile takenPhoto { get; set; }
        private ImageObject personObject;

        public ImageObject PersonObject
        {
            get
            {
                return personObject;
            }
            set
            {
                personObject = value; OnPropertyChanged();
            }
        }

        public async void TakePhoto()
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
            takenPhoto = await TakingPhotoHandler.TakePhoto();

            //Save taken photo to Azure cloud for recognition, later on it is deleted

            //await CloudStorageService.SaveBlockBlob(takenPhoto);
        }

        public async void NavigateToHomePage()
        {
            if(PersonObject.PersonFirstName == null || PersonObject.PersonLastName == null || PersonObject.PersonDescription == null || takenPhoto == null)
            {
                return;
            }
            //Saves the 'state' of user as registered
            if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                Application.Current.Properties.Remove("UserRegistered");
            }
            Application.Current.Properties.Add("UserRegistered", true);
            await Application.Current.SavePropertiesAsync();

            await Application.Current.MainPage.Navigation.PushAsync(new HomePage(new HomeViewModel()));
        }

        public void OnPropertyChanged([CallerMemberName] string propertiesName = "")
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;
            handler(this, new PropertyChangedEventArgs(propertiesName));
        }

    }
}
