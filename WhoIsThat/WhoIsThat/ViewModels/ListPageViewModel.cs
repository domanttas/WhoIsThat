using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WhoIsThat.Handlers;
using WhoIsThat.Models;
using WhoIsThat.Views;
using Xamarin.Forms;

namespace WhoIsThat.ViewModels
{
    public class ListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ImageObject> People { get; set; }

        public ICommand ParticipantsBackPopUpCommand { get; private set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }

        public ListPageViewModel(List<ImageObject> list)
        {
            People = new ObservableCollection<ImageObject>();

            foreach (var person in list)
            {
                People.Add(person);
            }

            ParticipantsBackPopUpCommand = new Command(ParticipantsBackPopUp);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void ItemIsTapped(ImageObject user)
        {
            Name = user.PersonFirstName;
            LastName = user.PersonLastName;
            Description = user.DescriptiveSentence;
            Score = user.Score;

            OnPropertyChanged("Name");
            OnPropertyChanged("LastName");
            OnPropertyChanged("Description");
            OnPropertyChanged("Score");

            await PopupNavigation.Instance.PushAsync(new ParticipantsPopUp(this));
        }

        public async void ParticipantsBackPopUp()
        {
            if (PopupNavigation.Instance.PopupStack.Any(p => p is ParticipantsPopUp))
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
