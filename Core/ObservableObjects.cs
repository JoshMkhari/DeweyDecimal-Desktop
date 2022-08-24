using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JoshMkhariPROG7312Game.Core
{
    class ObservableObjects : INotifyPropertyChanged //Updating UI when binding
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
