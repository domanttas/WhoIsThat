using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhoIsThat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //Navigates to home page if button 'Sign In' is clicked
        private async void NavigateToHome(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new HomePage());
        }

        //Navigate to registration page if button 'Sign Up' is clicked
        private async void NavigateToRegistration(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }
    }
}
