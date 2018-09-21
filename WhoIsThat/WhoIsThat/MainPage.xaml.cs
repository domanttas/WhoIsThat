using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhoIsThat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //Navigates to home page if button 'Sign In' is clicked
        private async void navigateToNextPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new HomePage());
        }
    }
}
