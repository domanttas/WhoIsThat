using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WhoIsThat.Models;

namespace WhoIsThat.ViewModels
{
    public class HistoryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<HistoryModel> History { get; set; }

        public HistoryPageViewModel(List<HistoryModel> historyList)
        {
            History = new ObservableCollection<HistoryModel>();

            foreach (var history in historyList)
            {
                History.Add(history);
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
