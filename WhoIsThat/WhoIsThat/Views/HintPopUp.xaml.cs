using Rg.Plugins.Popup.Pages;
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
	public partial class HintPopUp : PopupPage
	{
		public HintPopUp (HomeViewModel homeViewModel)
		{
            BindingContext = homeViewModel;

			InitializeComponent ();
		}
	}
}