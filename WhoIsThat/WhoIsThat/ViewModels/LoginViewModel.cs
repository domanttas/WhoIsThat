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
        
        public INavigation Navigation { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private RestService _restService;
        
        public bool ErrorLabel { get; set; }

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
        
        public LoginViewModel()
        {
            SavePersonCommand = new Command(SavePerson);
            TakePhotoCommand = new Command(TakePhoto);
            PersonObject = new ImageObject();
            _restService = new RestService();
        }

        public async void TakePhoto()
        {
            //Checking for camera permissions
            var cameraPermission = await PermissionHandler.CheckForCameraPermission();
            if (!cameraPermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            //Checking for storage permissions
            var storagePermission = await PermissionHandler.CheckForCameraPermission();
            if (!storagePermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

            //Taking photo and storing it in MediaFile variable 'takenPhoto'

            takenPhoto = await TakingPhotoHandler.TakePhoto();

        }

        public async void SavePerson()
        {
            if (!FieldsAreFilled())
            {
                return;
            }

            PersonObject.ImageName = PersonObject.PersonFirstName + PersonObject.PersonLastName + ".jpg";
            PersonObject.Score = 0;

            await CloudStorageService.SaveBlockBlob(takenPhoto, PersonObject.ImageName);
            PersonObject.ImageContentUri = CloudStorageService.GetImageUri(personObject.ImageName);

            PersonObject = await _restService.CreateImageObject(PersonObject);

            var status = await _restService.InsertUserIntoRecognition(PersonObject);

            SaveProperties();
            NavigateToHomePage();
        }

        private bool FieldsAreFilled()
        {
            if (PersonObject.PersonFirstName != null && PersonObject.PersonLastName != null &&
                PersonObject.DescriptiveSentence != null && takenPhoto != null) return true;
            ErrorLabel = true;
            OnPropertyChanged("ErrorLabel");
            return false;
        }

        public async void NavigateToHomePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage(new HomeViewModel(PersonObject)));
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
            handler?.Invoke(this, new PropertyChangedEventArgs(propertiesName));
        }

    }
}
