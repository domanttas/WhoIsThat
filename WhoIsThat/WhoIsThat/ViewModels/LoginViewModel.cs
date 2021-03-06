﻿using Acr.UserDialogs;
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
using WhoIsThat.ConstantsUtil;
using WhoIsThat.Exceptions;
using WhoIsThat.Handlers;
using WhoIsThat.Handlers.Utils;
using WhoIsThat.LogicUtil;
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

        private MediaFile takenPhoto { get; set; }
        private ImageObject _personObject;

        public ImageObject PersonObject
        {
            get
            {
                return _personObject;
            }
            set
            {
                _personObject = value;
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
            try
            {
                takenPhoto = await TakingPhotoHandler.TakePhoto();
            }

            catch (ManagerException photoNotTakenException)
            {
                ToastUtil.ShowToast(photoNotTakenException.ErrorCode);
            }
        }

        public async void SavePerson()
        {
            if (!FieldsAreFilled())
            {
                ToastUtil.ShowToast("All fields must be filled!");

                return;
            }

            PersonObject.ImageName = PersonObject.PersonFirstName + PersonObject.PersonLastName + ".jpg";
            PersonObject.Score = 0;

            await CloudStorageService.SaveBlockBlob(takenPhoto, PersonObject.ImageName);
            PersonObject.ImageContentUri = CloudStorageService.GetImageUri(_personObject.ImageName);

            try
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

                PersonObject = await _restService.CreateImageObject(PersonObject);
            }

            catch (ManagerException creationException)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(creationException.ErrorCode);

                return;
            }

            try
            {
                var status = await _restService.InsertUserIntoRecognition(PersonObject);

                var features = await _restService.GetFaceFeatures(PersonObject);

                var insertedFeatures = await _restService.InsertFaceFeatures(features);
            }

            catch (ManagerException recognitionException)
            {
                UserDialogs.Instance.HideLoading();

                ToastUtil.ShowToast(recognitionException.ErrorCode);

                return;
            }

            SaveProperties();

            UserDialogs.Instance.HideLoading();

            NavigateToHomePage();
        }

        private bool FieldsAreFilled()
        {
            if (PersonObject.PersonFirstName != null && PersonObject.PersonLastName != null &&
                PersonObject.DescriptiveSentence != null && takenPhoto != null) return true;

            return false;
        }

        public async void NavigateToHomePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomeNavigationPage(new HomeViewModel(PersonObject)));
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
