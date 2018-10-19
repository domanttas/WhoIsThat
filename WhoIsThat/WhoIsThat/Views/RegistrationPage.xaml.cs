using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage(RegistrationViewModel registrationViewModel)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = registrationViewModel;
            registrationViewModel.Navigation = Navigation;

            InitializeComponent();
        }
    }
}