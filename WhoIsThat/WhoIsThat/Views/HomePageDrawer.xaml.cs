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
	public partial class HomePageDrawer : ContentPage
	{
		public HomePageDrawer ()
		{
			InitializeComponent ();
		}

        public HomePageDrawer(HomeViewModel homeViewModel)
        {
            InitializeComponent();

            BindingContext = homeViewModel;
        }
    }
}