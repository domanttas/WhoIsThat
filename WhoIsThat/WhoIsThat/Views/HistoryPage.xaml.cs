using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WhoIsThat.Models;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        HistoryPageViewModel HistoryPageViewModel { get; set; }

        public HistoryPage(HistoryPageViewModel viewModel)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel;

            HistoryPageViewModel = viewModel;

            InitializeComponent();
        }

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = (ListView)sender;
            var selected = list.SelectedItem;
            var index = HistoryPageViewModel.History.IndexOf((DisplayHistoryModel)selected);

            HistoryPageViewModel.ItemIsTapped(HistoryPageViewModel.History.ElementAt(index));
        }
    }
}
