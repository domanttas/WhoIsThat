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
<<<<<<< HEAD
                
        private MediaFile TakenPhoto { get; set; }
=======
        public string test { get; set; }
                
        private MediaFile takenPhoto { get; set; }
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)
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
<<<<<<< HEAD
            TakenPhoto = await TakingPhotoHandler.TakePhoto();
=======
            takenPhoto = await TakingPhotoHandler.TakePhoto();
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)

            //Save taken photo to Azure cloud for recognition, later on it is deleted

            //await CloudStorageService.SaveBlockBlob(takenPhoto);
        }

        public async void NavigateToHomePage()
        {
<<<<<<< HEAD
            //Checks if all requirements are faced
=======
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)
            if(PersonObject.PersonFirstName == null || PersonObject.PersonLastName == null || PersonObject.PersonDescription == null || takenPhoto == null)
            {
                return;
            }
<<<<<<< HEAD

            //If so, gives person an id and stores his information in the db
            PersonObject.Id = 1;
            PersonObject.ImageName = String.Format(PersonObject.PersonFirstName, PersonObject.PersonLastName);



=======
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)
            //Saves the 'state' of user as registered
            if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                Application.Current.Properties.Remove("UserRegistered");
            }
            Application.Current.Properties.Add("UserRegistered", true);
            await Application.Current.SavePropertiesAsync();

<<<<<<< HEAD
            //Navigates to homepage
=======
>>>>>>> 544ccba... Created registration page, binded user input to loginViewModel(through ImageObject), made the app able to determine wether the user is registered or not (for development purposes it is not currently functioning)
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
