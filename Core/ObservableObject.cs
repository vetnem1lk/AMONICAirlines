using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMONIC.Core
{
    internal class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropetryChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); 
        }
    }
}
