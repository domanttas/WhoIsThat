using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterUserInfo : ContentPage
	{
		public RegisterUserInfo (RegisterUserInfoViewModel registerUserInfoViewModel)
		{
            registerUserInfoViewModel.Navigation = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = registerUserInfoViewModel;

            InitializeComponent ();
		}
	}
}