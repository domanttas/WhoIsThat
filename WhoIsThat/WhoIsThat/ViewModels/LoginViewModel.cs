using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        public ICommand SavePersonCommand { get; private set; }
        public ICommand TakePhotoCommand { get; set; }
        private RestService _restService;
        public LoginViewModel()
        {
            SavePersonCommand = new Command(SavePerson);
            TakePhotoCommand = new Command(TakePhoto);
            PersonObject = new ImageObject();
            _restService = new RestService();
        }
        public bool ErrorLabel { get; set; }
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
                //OnPropertyChanged();
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

        }

        public async void SavePerson()
        {
            //Checks if all required fields are filled
            if (!FieldsAreFilled())
            {
                return;
            }

            //If so, stores his information in the db
            /*PersonObject.ImageName = PersonObject.PersonFirstName + PersonObject.PersonLastName + ".jpg";
            await CloudStorageService.SaveBlockBlob(takenPhoto,PersonObject.ImageName);
            PersonObject = await _restService.CreateImageObject(PersonObject);*/

            PersonObject.ImageName = PersonObject.PersonFirstName + PersonObject.PersonLastName + ".jpg";
            await CloudStorageService.SaveBlockBlob(takenPhoto, PersonObject.ImageName);

            SaveProperties(); 
            NavigateToHomePage();
        }

        private bool FieldsAreFilled()
        {
            if (PersonObject.PersonFirstName == null || PersonObject.PersonLastName == null || PersonObject.DescriptiveSentence == null || takenPhoto == null)
            {
                ErrorLabel = true;
                OnPropertyChanged("ErrorLabel");
                return false;
            }
            return true;
        }

        public async void NavigateToHomePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage(new HomeViewModel()));
        }

        private async void SaveProperties()
        { 
            if (Application.Current.Properties.ContainsKey("UserId"))
            {
                Application.Current.Properties.Remove("UserId");
            }
            Application.Current.Properties.Add("UserId", PersonObject.Id);

            //Saves the 'state' of user as registered
            if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                Application.Current.Properties.Remove("UserRegistered");
            }
            Application.Current.Properties.Add("UserRegistered", true);
            await Application.Current.SavePropertiesAsync();
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
