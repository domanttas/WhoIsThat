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
using WhoIsThat.Handlers.Utils;
using WhoIsThat.Models;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand NavigateHomePageCommand { get; private set; }
        public ICommand TakePhotoCommand { get; set; }
        private RestService _restService;
        public LoginViewModel()
        {
            NavigateHomePageCommand = new Command(SavePerson);
            TakePhotoCommand = new Command(TakePhoto);
            PersonObject = new ImageObject();
            _restService = new RestService();
        }
        

        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

                
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
                personObject = value;
                OnPropertyChanged();
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

        public async void SavePerson()
        { 
            //Checks if all required fields are filled

            /*if(PersonObject.PersonFirstName == null || PersonObject.PersonLastName == null || PersonObject.DescriptiveSentence == null || takenPhoto == null)
            {
                return;
            }*/

            //If so, stores his information in the db
            PersonObject.Id = 1;
            PersonObject.ImageName = PersonObject.PersonFirstName + PersonObject.PersonLastName + ".jpg";
            /*await CloudStorageService.SaveBlockBlob(takenPhoto,PersonObject.ImageName);

            //Information stored and the stored user is returned with a newly generated ID
            ImageObject person = await _restService.CreateImageObject(PersonObject);

            
            */
            //Saves the 'state' of user as registered
            if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                Application.Current.Properties.Remove("UserRegistered");
            }
            Application.Current.Properties.Add("UserRegistered", true);
            await Application.Current.SavePropertiesAsync();
        }

        public async void NavigateToHomePage()
        {
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
