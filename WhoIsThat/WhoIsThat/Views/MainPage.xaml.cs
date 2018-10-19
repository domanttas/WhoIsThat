using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.ViewModels;
using Xamarin.Forms;

namespace WhoIsThat
{
    public partial class MainPage : ContentPage
    {
        public MainPage(LoginViewModel loginViewModel)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = loginViewModel;
            loginViewModel.Navigation = Navigation;

            InitializeComponent();
        }
    }
}
