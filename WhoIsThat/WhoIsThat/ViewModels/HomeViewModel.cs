using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using System.Threading.Tasks;
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

        public string DisplayAge { get; set; }
        public string DisplayGender { get; set; }

        public string TargetDescriptionSentence { get; set; }

        public string Name { get; set; }

        public INavigation Navigation { get; set; }

        public ImageHandler ImageHandler { get; set; }

        public ImageObject User { get; set; }

        public TargetObject Target { get; set; }
        public bool IsTargetAlreadyAssigned { get; set; }

        private RestService _restService { get; set; }

        public string TargetImageUri { get; set; }

        public HomeViewModel(ImageObject user)
        {
            UserDialogs.Instance.HideLoading();

            _restService = new RestService();

            TakePhotoCommand = new Command(TakePhoto);
            NavigateToListPageCommand = new Command(NavigateToListPage);
            NavigateToLeadersPageCommand = new Command(NavigateToLeadersPage);
            GetTargetCommand = new Command(GetTarget);

            ImageHandler = new ImageHandler();

            User = user;

            Name = "Welcome, " + User.PersonFirstName + ". Your score: " + User.Score.ToString();
            OnPropertyChanged("Name");
        }

        /// <summary>
        /// Checks for permissions, takes photo and checks whether target was hit
        /// </summary>
        public async void TakePhoto()
        {
            DisplayStatus = "Please wait...";
            OnPropertyChanged("DisplayStatus");

            var cameraPermission = await PermissionHandler.CheckForCameraPermission();
            if (!cameraPermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var storagePermission = await PermissionHandler.CheckForCameraPermission();
            if (!storagePermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            try
            {
                var takenPhoto = await TakingPhotoHandler.TakePhoto();

                await CloudStorageService.SaveBlockBlob(takenPhoto, "temp.jpg");
            }
            
            catch (ManagerException photoNotTakenException)
            {
                DisplayStatus = photoNotTakenException.ErrorCode;
                OnPropertyChanged("DisplayStatus");

                return;
            }

            try
            {
                var recognitionMessage = await _restService.Identify();
                var isTargetDead = await _restService.IsPreyHunted(User.Id, Convert.ToInt32(recognitionMessage));

                if (isTargetDead)
                {
                    DisplayMessage = "It's a direct hit!";
                    OnPropertyChanged("DisplayMessage");

                    DisplayStatus = "Please wait...";
                    OnPropertyChanged("DisplayStatus");

                    var hitResult = await _restService.GetUserById(Convert.ToInt32(recognitionMessage));

                    DisplayMessage = "Get to know each other!";
                    OnPropertyChanged("DisplayMessage");

                    DisplayStatus = hitResult.PersonFirstName;
                    OnPropertyChanged("DisplayStatus");

                    User = await _restService.UpdateUserScore(User.Id);

                    Name = "Welcome, " + User.PersonFirstName + ". Your score: " + User.Score.ToString();
                    OnPropertyChanged("Name");
                }
            }

            catch (ManagerException noFacesFoundException) when (noFacesFoundException.ErrorCode == Constants.NoFacesIdentifiedError)
            {
                DisplayMessage = "It's not your target...";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = noFacesFoundException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }

            catch (ManagerException noOneIdentifiedException) when (noOneIdentifiedException.ErrorCode == Constants.NoMatchFoundError)
            {
                DisplayMessage = "Person is not a player...";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = noOneIdentifiedException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }

            catch (ManagerException targetNotFoundException) when (targetNotFoundException.ErrorCode == Constants.TargetNotFoundError)
            {
                DisplayMessage = "It's not your target...";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = targetNotFoundException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }

            catch (ManagerException userNotFoundException) when (userNotFoundException.ErrorCode == Constants.UserDoesNotExistError)
            {
                DisplayMessage = "Something went wrong...";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = "Please try again!";
                OnPropertyChanged("DisplayStatus");
            }
        }

        /// <summary>
        /// Assigns random target after checking if it is already assigned
        /// </summary>
        public async void GetTarget()
        {
            var checkTargetStatus = await CheckForTarget();
            if (checkTargetStatus)
            {
                DisplayStatus = Constants.TargetAlreadyAssignedError;
                OnPropertyChanged("DisplayStatus");

                var fetchedTarget = await _restService.GetUserById(Target.PreyPersonId);
                TargetDescriptionSentence = fetchedTarget.DescriptiveSentence;
                TargetImageUri = fetchedTarget.ImageContentUri;

                OnPropertyChanged("TargetImageUri");
                OnPropertyChanged("TargetDescriptionSentence");

                var fetchedFeatures = await _restService.GetFaceFeatures(fetchedTarget);

                DisplayAge = "Age: " + fetchedFeatures.Age.ToString();
                DisplayGender = "Gender: " + fetchedFeatures.Gender;

                OnPropertyChanged("DisplayAge");
                OnPropertyChanged("DisplayGender");

                return;
            }

            try
            {
                var targetId = await _restService.GetRandomTarget(User.Id);

                var fetchedTarget = await _restService.GetUserById(targetId);
                TargetDescriptionSentence = fetchedTarget.DescriptiveSentence;
                TargetImageUri = fetchedTarget.ImageContentUri;

                OnPropertyChanged("TargetImageUri");
                OnPropertyChanged("TargetDescriptionSentence");

                var fetchedFeatures = await _restService.GetFaceFeatures(fetchedTarget);

                DisplayAge = "Age: " + fetchedFeatures.Age.ToString();
                DisplayGender = "Gender: " + fetchedFeatures.Gender;

                OnPropertyChanged("DisplayAge");
                OnPropertyChanged("DisplayGender");

                return;
            }

            catch (ManagerException getTargetException) when (getTargetException.ErrorCode == Constants.TargetAlreadyAssignedError)
            {
                DisplayStatus = getTargetException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }

            catch (ManagerException noPlayersException) when (noPlayersException.ErrorCode == Constants.ThereAreNoPlayersError)
            {
                DisplayStatus = noPlayersException.ErrorCode;
                OnPropertyChanged("DisplayStatus");
            }
        }

        /// <summary>
        /// Event handler for changing binded data
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Navigates to list page which is responsible for displayed players
        /// </summary>
        public async void NavigateToListPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ListPage(new ListPageViewModel(await ImageHandler.GetImageObjects())));
            //await Application.Current.MainPage.Navigation.PushAsync(new HomeNavigationPage(new HomeViewModel(User)));
        }

        /// <summary>
        /// Navigates to the page which is responsible for displaying players sorted by score
        /// </summary>
        public async void NavigateToLeadersPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LeadersPage(new LeadersPageViewModel(await ImageHandler.GetImageObjects())));
        }
        
        /// <summary>
        /// Checks whether target was already assigned, mainly used for launch of app
        /// </summary>
        /// <returns>boolean value</returns>
        private async Task<bool> CheckForTarget()
        {
            try
            {
                Target = await _restService.GetCurrentTarget(User.Id);
                return true;
            }

            catch (ManagerException)
            {
                return false;
            }
        }
    }
}
