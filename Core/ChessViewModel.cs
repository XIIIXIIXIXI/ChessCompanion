using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using ChessCompanion.MVVM.ViewModel;
using OpenQA.Selenium;
using System.ComponentModel;


namespace ChessCompanion
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        private readonly GameMediator mediator;

        public ChessViewModel(GameMediator mediator)
        {
            this.mediator = mediator;
        }

        public MainState State => mediator.State;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    
}