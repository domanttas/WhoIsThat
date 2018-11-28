using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WhoIsThat.Connections;
using WhoIsThat.Models;
using WhoIsThat.Views;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class HistoryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DisplayHistoryModel> History { get; set; }

        public ICommand HistoryBackPopUpCommand { get; private set; }

        public string Name { get; set; }
        public string Status { get; set; }

        public HistoryPageViewModel(List<DisplayHistoryModel> historyList)
        {
            History = new ObservableCollection<DisplayHistoryModel>();

            foreach (var element in historyList)
            {
                History.Add(element);
            }

            HistoryBackPopUpCommand = new Command(HistoryBackPopUp);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void ItemIsTapped(DisplayHistoryModel history)
        {
            Name = history.FirstName;
            Status = history.Status;

            OnPropertyChanged("Name");
            OnPropertyChanged("Status");

            await PopupNavigation.Instance.PushAsync(new HistoryPopUp(this));
        }

        public async void HistoryBackPopUp()
        {
            if (PopupNavigation.Instance.PopupStack.Any(p => p is HistoryPopUp))
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
