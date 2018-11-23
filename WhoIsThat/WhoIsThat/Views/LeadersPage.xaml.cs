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
	public partial class LeadersPage : ContentPage
	{
		public LeadersPage (LeadersPageViewModel leadersPageViewModel)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = leadersPageViewModel;
            
            InitializeComponent ();
		}
	}
}