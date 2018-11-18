using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WhoIsThat.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhoIsThat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage(HistoryPageViewModel historyPageViewModel)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = historyPageViewModel;

            InitializeComponent();
        }
    }
}
