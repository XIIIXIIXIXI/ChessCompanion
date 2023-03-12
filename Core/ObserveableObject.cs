using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessCompanion.Core
{
    class ObserveableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
