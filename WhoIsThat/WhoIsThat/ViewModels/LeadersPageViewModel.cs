using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WhoIsThat.Models;

namespace WhoIsThat.ViewModels
{
    public class LeadersPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ImageObject> People { get; set; }

        public LeadersPageViewModel(List<ImageObject> list)
        {
            People = new ObservableCollection<ImageObject>();

            foreach (var person in list)
            {
                People.Add(person);
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
