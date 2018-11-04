using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using System.Windows.Input;
using WhoIsThat.Connections;
using WhoIsThat.ConstantsUtil;
using WhoIsThat.Handlers;
using WhoIsThat.Handlers.Utils;
using WhoIsThat.Models;
using WhoIsThat.Views;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public ICommand TakePhotoCommand { get; private set; }
        public ICommand NavigateToListPageCommand { get; private set; }
        public ICommand NavigateToLeadersPageCommand { get; private set; }

        public ImageSource DisplayStream { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayReturnedName { get; set; }
        public string DisplayMessage { get; set; }

        public INavigation Navigation { get; set; }

        public ImageHandler ImageHandler { get; set; }

        public ImageObject User { get; set; }

        public HomeViewModel(ImageObject user)
        {
            TakePhotoCommand = new Command(TakePhoto);
            NavigateToListPageCommand = new Command(NavigateToListPage);
            NavigateToLeadersPageCommand = new Command(NavigateToLeadersPage);
            ImageHandler = new ImageHandler();

            User = user;
        }

        public async void TakePhoto()
        {
            DisplayReturnedName = "Please wait...";
            OnPropertyChanged("DisplayReturnedName");

            //Checking for camera permissions
            var cameraPermission = await PermissionHandler.CheckForCameraPermission();
            if (!cameraPermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            //Checking for storage permissions
            var storagePermission = await PermissionHandler.CheckForCameraPermission();
            if (!storagePermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

            //Taking photo and storing it in MediaFile variable 'takenPhoto'
            var takenPhoto = await TakingPhotoHandler.TakePhoto();

            //Save taken photo to Azure cloud for recognition, later on it is deleted
            await CloudStorageService.SaveBlockBlob(takenPhoto,"temp.jpg");
            
            //Binding taken image for display
            DisplayStream = ImageSource.FromStream(() =>
            {
                var stream = takenPhoto.GetStream();
                takenPhoto.Dispose();
                return stream;
            });
            
            OnPropertyChanged("DisplayStream");

            //Initiating recognition API
            var restService = new RestService();
            var recognizedName = await restService.Identify();

            //Checking whether person was identified and deciding on messages to display
            if (IsIdentified(recognizedName))
            {
                DisplayMessage = "It's a match!";
                OnPropertyChanged("DisplayMessage");

                DisplayReturnedName = recognizedName;
                OnPropertyChanged("DisplayReturnedName");
            }

            else
            {
                DisplayMessage = "Sadly, it's not your friend..";
                OnPropertyChanged("DisplayMessage");

                DisplayReturnedName = recognizedName;
                OnPropertyChanged("DisplayReturnedName");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //Public for unit tests
        public bool IsIdentified(string message)
        {
            return message != Constants.NoFacesIdentifiedError && message != Constants.NoMatchFoundError;
        }

        public async void NavigateToListPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ListPage(new ListPageViewModel(await ImageHandler.GetImageObjects())));
        }

        public async void NavigateToLeadersPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LeadersPage(new LeadersPageViewModel(await ImageHandler.GetImageObjects())));
        }
    }
}
