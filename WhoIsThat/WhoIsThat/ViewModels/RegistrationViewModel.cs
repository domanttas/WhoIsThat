using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class RegistrationViewModel
    {
        public ICommand NavigateHomePageCommand { get; private set; }
        public INavigation Navigation { get; set; }

        public RegistrationViewModel()
        {
            NavigateHomePageCommand = new Command(NavigateToHomePage);
        }

        public async void NavigateToHomePage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
        }
    }
}
