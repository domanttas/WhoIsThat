using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsThat.Models;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
        ListPageViewModel ListPageViewModel { get; set; }

		public ListPage (ListPageViewModel viewModel)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel;

            ListPageViewModel = viewModel;

            InitializeComponent ();
		}

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var selected = list.SelectedItem;
            var index = ListPageViewModel.People.IndexOf((ImageObject)selected);

            ListPageViewModel.ItemIsTapped(ListPageViewModel.People.ElementAt(index));
        }
    }
}