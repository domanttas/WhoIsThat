using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Connections;
using WhoIsThat.Handlers;
using WhoIsThat.Models;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage (HomeViewModel homeViewModel)
		{
            homeViewModel.Navigation = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = homeViewModel;

            InitializeComponent();
		}

        public HomePage()
        {
            InitializeComponent();
        }
    }
}