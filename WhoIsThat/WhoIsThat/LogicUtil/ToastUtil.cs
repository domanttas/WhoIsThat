using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WhoIsThat.LogicUtil
{
    public class ToastUtil
    {
        public static void ShowToast(string message)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(2000);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(0, 0, 0));
            toastConfig.SetMessageTextColor(Color.Red);

            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
