using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessCompanion.MVVM.ViewModel
{
    /* 
        State for user interactable objects.
     */
    public class UIState : INotifyPropertyChanged
    {

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnSelectedIndexChanged();

            }
        }
        public event EventHandler SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged()
        {
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }


        private bool _isAutomoveEnabled;
        public bool IsAutomoveEnabled
        {
            get { return _isAutomoveEnabled; }
            set
            {
                _isAutomoveEnabled = value;
                OnPropertyChanged(nameof(IsAutomoveEnabled));
            }
        }

        private string _test = "hi";
        public string Test
        {
            get { return _test; }
            set
            {
                _test = value;
                OnPropertyChanged(nameof(Test));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
