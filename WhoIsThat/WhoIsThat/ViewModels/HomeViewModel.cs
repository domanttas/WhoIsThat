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
using WhoIsThat.LogicUtil;
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
        public ICommand GiveHintCommand { get; private set; }
        public ICommand NavigateToHistoryPageCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayStatus { get; set; }
        public string DisplayMessage { get; set; }

        public string DisplayAge { get; set; }
        public string DisplayGender { get; set; }

        public string TargetDescriptionSentence { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }

        public INavigation Navigation { get; set; }

        public ImageHandler ImageHandler { get; set; }

        public ImageObject User { get; set; }

        public TargetObject Target { get; set; }
        public bool IsTargetAlreadyAssigned { get; set; }

        private RestService _restService { get; set; }

        public string TargetImageUri { get; set; }

        public bool IsHintAvailable { get; set; }

        public HomeViewModel(ImageObject user)
        {
            UserDialogs.Instance.HideLoading();

            _restService = new RestService();

            TakePhotoCommand = new Command(TakePhoto);
            NavigateToListPageCommand = new Command(NavigateToListPage);
            NavigateToLeadersPageCommand = new Command(NavigateToLeadersPage);
            GetTargetCommand = new Command(GetTarget);
            GiveHintCommand = new Command(GetHint);
            NavigateToHistoryPageCommand = new Command(NavigateToHistoryPage);

            ImageHandler = new ImageHandler();

            User = user;

            Name = "Your score is: " + User.Score.ToString();
            OnPropertyChanged("Name");

            UserName = User.PersonFirstName;
            OnPropertyChanged("UserName");

            IsHintAvailable = true;
        }

        /// <summary>
        /// Checks for permissions, takes photo and checks whether target was hit
        /// </summary>
        public async void TakePhoto()
        {
            var cameraPermission = await PermissionHandler.CheckForCameraPermission();
            if (!cameraPermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var storagePermission = await PermissionHandler.CheckForCameraPermission();
            if (!storagePermission)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            try
            {
                var takenPhoto = await TakingPhotoHandler.TakePhoto();

                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

                var saveImage = User.Id + ".jpg";
                await CloudStorageService.SaveBlockBlob(takenPhoto, saveImage);
            }
            
            catch (ManagerException photoNotTakenException)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(photoNotTakenException.ErrorCode);

                return;
            }

            try
            {
                var imageName = User.Id + ".jpg";
                var recognitionMessage = await _restService.Identify(imageName);
                var isTargetDead = await _restService.IsPreyHunted(User.Id, Convert.ToInt32(recognitionMessage));

                if (isTargetDead)
                {
                    DisplayMessage = "It's a direct hit!";
                    OnPropertyChanged("DisplayMessage");

                    var hitResult = await _restService.GetUserById(Convert.ToInt32(recognitionMessage));

                    var historyResult = await _restService.UpdateHistoryModel(User.Id, hitResult.Id);

                    UserDialogs.Instance.HideLoading();

                    //DisplayMessage = "Connect!";
                    //OnPropertyChanged("DisplayMessage");

                    DisplayStatus = "Name: " + hitResult.PersonFirstName;
                    OnPropertyChanged("DisplayStatus");

                    User = await _restService.UpdateUserScore(User.Id);

                    Name = "Your score is: " + User.Score.ToString();
                    OnPropertyChanged("Name");
                }
            }

            catch (ManagerException noFacesFoundException) when (noFacesFoundException.ErrorCode == Constants.NoFacesIdentifiedError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(noFacesFoundException.ErrorCode);
            }

            catch (ManagerException noOneIdentifiedException) when (noOneIdentifiedException.ErrorCode == Constants.NoMatchFoundError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(noOneIdentifiedException.ErrorCode);
            }

            catch (ManagerException targetNotFoundException) when (targetNotFoundException.ErrorCode == Constants.TargetNotFoundError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(targetNotFoundException.ErrorCode);
            }

            catch (ManagerException userNotFoundException) when (userNotFoundException.ErrorCode == Constants.UserDoesNotExistError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast("Something went wrong");
            }

            //This catch is just for testing purposes
            catch (ManagerException)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast("Something went wrong");
            }
        }

        /// <summary>
        /// Assigns random target after checking if it is already assigned
        /// </summary>
        public async void GetTarget()
        {
            DisplayMessage = "";
            OnPropertyChanged("DisplayMessage");

            DisplayStatus = "";
            OnPropertyChanged("DisplayStatus");

            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

            var checkTargetStatus = await CheckForTarget();
            if (checkTargetStatus)
            {
                var fetchedTarget = await _restService.GetUserById(Target.PreyPersonId);
                TargetDescriptionSentence = fetchedTarget.DescriptiveSentence;
                //TargetImageUri = fetchedTarget.ImageContentUri;

                //OnPropertyChanged("TargetImageUri");
                OnPropertyChanged("TargetDescriptionSentence");

                var fetchedFeatures = await _restService.GetFaceFeatures(fetchedTarget);

                DisplayAge = "Age: " + fetchedFeatures.Age.ToString();
                DisplayGender = "Gender: " + fetchedFeatures.Gender;

                OnPropertyChanged("DisplayAge");
                OnPropertyChanged("DisplayGender");

                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(Constants.TargetAlreadyAssignedError);

                return;
            }

            try
            {
                DisplayMessage = "";
                OnPropertyChanged("DisplayMessage");

                DisplayStatus = "";
                OnPropertyChanged("DisplayStatus");

                var targetId = await _restService.GetRandomTarget(User.Id);

                var fetchedTarget = await _restService.GetUserById(targetId);
                TargetDescriptionSentence = fetchedTarget.DescriptiveSentence;
                //TargetImageUri = fetchedTarget.ImageContentUri;

                //OnPropertyChanged("TargetImageUri");
                OnPropertyChanged("TargetDescriptionSentence");

                var result = _restService.InsertHistoryModel(new HistoryModel()
                {
                    UserId = User.Id,
                    TargetId = targetId,
                    Status = Constants.TargetNotHuntedHistory
                });

                var fetchedFeatures = await _restService.GetFaceFeatures(fetchedTarget);

                DisplayAge = "Age: " + fetchedFeatures.Age.ToString();
                DisplayGender = "Gender: " + fetchedFeatures.Gender;

                OnPropertyChanged("DisplayAge");
                OnPropertyChanged("DisplayGender");

                IsHintAvailable = true;

                UserDialogs.Instance.HideLoading();

                return;
            }

            catch (ManagerException getTargetException) when (getTargetException.ErrorCode == Constants.TargetAlreadyAssignedError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(getTargetException.ErrorCode);
            }

            catch (ManagerException noPlayersException) when (noPlayersException.ErrorCode == Constants.ThereAreNoPlayersError)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(noPlayersException.ErrorCode);
            }
        }

        /// <summary>
        /// Shows photo of target as a hint only one time for one target
        /// </summary>
        public async void GetHint()
        {
            if (!IsHintAvailable)
            {
                ToastUtil.ShowToast("You already used your hint!");

                return;
            }

            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = "Do you want to use up your hint?",
                OkText = "Yes please",
                CancelText = "Nah"
            });

            if (!result)
            {
                return;
            }

            var checkTargetStatus = await CheckForTarget();
            if (checkTargetStatus)
            {
                var fetchedTarget = await _restService.GetUserById(Target.PreyPersonId);

                TargetImageUri = fetchedTarget.ImageContentUri;
                OnPropertyChanged("TargetImageUri");

                IsHintAvailable = false;

                await Task.Delay(3000);

                TargetImageUri = "";
                OnPropertyChanged("TargetImageUri");

                return;
            }

            ToastUtil.ShowToast("You don't have a target!");
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
        }

        /// <summary>
        /// Navigates to the page which is responsible for displaying players sorted by score
        /// </summary>
        public async void NavigateToLeadersPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LeadersPage(new LeadersPageViewModel(await ImageHandler.GetImageObjects())));
        }

        public async void NavigateToHistoryPage()
        {
            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

            try
            {
                var historyList = await _restService.GetHistoryById(User.Id);

                var displayHistoryList = new List<DisplayHistoryModel>();

                foreach (var element in historyList)
                {
                    var target = await _restService.GetUserById(element.TargetId);

                    displayHistoryList.Add(new DisplayHistoryModel()
                    {
                        Status = element.Status,
                        ImageUri = target.ImageContentUri,
                        FirstName = target.PersonFirstName,
                    });
                }

                UserDialogs.Instance.HideLoading();

                await Application.Current.MainPage.Navigation.PushAsync(new HistoryPage(new HistoryPageViewModel(displayHistoryList)));
            }
            
            catch (ManagerException managerException) when (managerException.ErrorCode == Constants.HistoryElementNotFoundError)
            {
                ToastUtil.ShowToast(Constants.HistoryElementNotFoundError);
            }
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
