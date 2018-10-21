using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class LoginViewModel
    {
        public ICommand NavigateHomePageCommand { get; private set; }
        public ICommand NavigateRegistrationPageCommand { get; private set; }
        public INavigation Navigation { get; set; }

        public LoginViewModel()
        {
            NavigateHomePageCommand = new Command(NavigateToHomePage);
            NavigateRegistrationPageCommand = new Command(NavigateToRegistrationPage);
        }

        public async void NavigateToHomePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage(new HomeViewModel()));
        }

        public async void NavigateToRegistrationPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage(new RegistrationViewModel()));
        }
    }
}
