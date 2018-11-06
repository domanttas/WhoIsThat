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
using WhoIsThat.Exceptions;
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
        public ICommand GetTargetCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayStatus { get; set; }
        public string DisplayMessage { get; set; }

        public INavigation Navigation { get; set; }

        public ImageHandler ImageHandler { get; set; }

        public ImageObject User { get; set; }

        public HomeViewModel(ImageObject user)
        {
            TakePhotoCommand = new Command(TakePhoto);
            NavigateToListPageCommand = new Command(NavigateToListPage);
            NavigateToLeadersPageCommand = new Command(NavigateToLeadersPage);
            GetTargetCommand = new Command(GetTarget);

            ImageHandler = new ImageHandler();

            User = user;
        }

        public async void TakePhoto()
        {
            DisplayStatus = "Please wait...";
            OnPropertyChanged("DisplayStatus");

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

            if (takenPhoto == null)
            {
                return;
            }

            //Save taken photo to Azure cloud for recognition, later on it is deleted
            await CloudStorageService.SaveBlockBlob(takenPhoto,"temp.jpg");
 
            //Initiating recognition API
            var restService = new RestService();

            try
            {
                var recognitionMessage = await restService.Identify();

                DisplayMessage = "It's a direct hit!";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = "Please wait...";
                OnPropertyChanged("DisplayStatus");

                var hitResult = await restService.GetUserById(Convert.ToInt32(recognitionMessage));

                DisplayStatus = hitResult.PersonFirstName;
                OnPropertyChanged("DisplayStatus");
            }

            catch (ManagerException managerException)
            {
                DisplayMessage = "It's not your target...";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = managerException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }
        }

        public async void GetTarget()
        {

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
