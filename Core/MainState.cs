using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.Core
{
    /* This function acts as a dataclass in Kotlin, using jetpack compose. It is set up like this
     * so the UI updates automatically whenever the state value changes. 
     */
    public class MainState : INotifyPropertyChanged
    {

        public MainState()
        {
            MoveInfos = new MoveInfo[5];
            for (int i = 0; i < 5; i++)
            {
                MoveInfos[i] = new MoveInfo();
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

        private MoveInfo[] _moveInfos = new MoveInfo[5];
        public MoveInfo[] MoveInfos
        {
            get { return _moveInfos; }
            set
            {
                _moveInfos = value;
                OnPropertyChanged(nameof(MoveInfos));
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
        private string _moves = "";
       
    }
    public class MoveInfo : INotifyPropertyChanged
    {
        private string _moves;
        public string Moves
        {
            get { return _moves; }
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        private int? _cp;
        public int? CP
        {
            get { return _cp; }
            set
            {
                _cp = value;
                OnPropertyChanged(nameof(CP));
            }
        }

        private int? _mate;
        public int? MATE
        {
            get { return _mate; }
            set
            {
                _mate = value;
                OnPropertyChanged(nameof(MATE));
            }
        }

        private string _pv;
        public string PV
        {
            get { return _pv; }
            set
            {
                _pv = value;
                OnPropertyChanged(nameof(PV));
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
/*
 *  public string Moves
        {
            get { return _moves; }
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        private int? _cp = null;
        public int? CP
        {
            get { return _cp; }
            set
            {
                _cp = value;
                OnPropertyChanged(nameof(CP));
            }
        }

        private int? _mate = null;
        public int? MATE
        {
            get { return _mate; }
            set
            {
                _mate = value;
                OnPropertyChanged(nameof(MATE));
            }
        }
        private string _pv = "";
        public string PV
        {
            get { return _pv; }
            set
            {
                _pv = value;
                OnPropertyChanged(nameof(PV));
            }
        }
 */
