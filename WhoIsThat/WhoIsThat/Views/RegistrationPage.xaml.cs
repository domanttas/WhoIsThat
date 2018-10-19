using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        //Navigate to registration page if button 'Sign Up' is clicked
        private async void NavigateToHome(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new HomePage());
        }
    }
}