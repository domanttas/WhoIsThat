﻿using Acr.UserDialogs;
using System;
using WhoIsThat.Connections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WhoIsThat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(new ViewModels.LoginViewModel()));
        }

        protected override async void OnStart()
        {
            var restService = new RestService();
            // Handle when your app starts
            if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                if (Application.Current.Properties["UserRegistered"].Equals(true))
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

                    var user = await restService.GetUserById(Convert.ToInt32(Application.Current.Properties["UserId"].ToString()));

                    MainPage = new NavigationPage(new HomeNavigationPage(new ViewModels.HomeViewModel(user)));
                }
            }

            else MainPage = new NavigationPage(new MainPage(new ViewModels.LoginViewModel()));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
