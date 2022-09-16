using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestApp_DataGrid.ViewModels
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}
