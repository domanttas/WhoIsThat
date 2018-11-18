using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WhoIsThat.Connections;
using WhoIsThat.Models;

namespace WhoIsThat.ViewModels
{
    public class HistoryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DisplayHistoryModel> History { get; set; }

        public HistoryPageViewModel(List<DisplayHistoryModel> historyList)
        {
            History = new ObservableCollection<DisplayHistoryModel>();

            foreach (var element in historyList)
            {
                History.Add(element);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
