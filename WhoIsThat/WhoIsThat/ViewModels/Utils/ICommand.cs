using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.ViewModels.Utils
{
    public interface ICommand
    {
        void Execute(object arg);
        bool CanExecute(object arg);
        event EventHandler CanExecuteChanged;
    }
}
