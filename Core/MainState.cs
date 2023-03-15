using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.Core
{
    /* This function as a dataclass in Kotlin, using jetpack compose. It is set up like this
     * so the UI updates automatically whenever the state value changes. 
     * 
     */
    public class MainState : INotifyPropertyChanged
    {
        private string _moves = "";
        public string Moves
        {
            get { return _moves; }
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        private string _fen = "";
        public string FEN
        {
            get { return _fen; }
            set
            {
                _fen = value;
                OnPropertyChanged(nameof(FEN));
            }
        }

        private string _cp = "";
        public string CP
        {
            get { return _cp; }
            set
            {
                _cp = value;
                OnPropertyChanged(nameof(CP));
            }
        }

        private bool _isWhite;
        public bool IsWhite
        {
            get { return _isWhite; }
            set
            {
                _isWhite = value;
                OnPropertyChanged(nameof(IsWhite));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
