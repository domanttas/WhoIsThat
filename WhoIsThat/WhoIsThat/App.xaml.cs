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
            //This is for testing purposes only
            /*if (Application.Current.Properties.ContainsKey("UserRegistered"))
            {
                Application.Current.Properties.Remove("UserRegistered");
            }*/

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
                    MainPage = new NavigationPage(new HomePage(new ViewModels.HomeViewModel(await restService.GetUserById(Convert.ToInt32(Application.Current.Properties["UserId"].ToString())))));
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
