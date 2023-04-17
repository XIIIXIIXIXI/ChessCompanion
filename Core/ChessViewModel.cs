using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using ChessCompanion.MVVM.ViewModel;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessCompanion
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        private readonly GameMediator mediator;
        //maybe another gamemediator for click logic. 
        private readonly UIState uiState = new UIState();

        

        public RelayCommand ButtonClickCommand { get; }

        public ChessViewModel(GameMediator mediator)
        {
            this.mediator = mediator;
            ButtonClickCommand = new RelayCommand(OnButtonClick);
        }
        public MainState State => mediator.State;
        public UIState UiState => uiState;

        private void OnButtonClick(object parameter)
        {
            
            Debug.WriteLine("Clicked");
            UiState.IsAutomoveEnabled = !UiState.IsAutomoveEnabled;
            UiState.ButtonBackground = UiState.IsAutomoveEnabled ? Brushes.Green : Brushes.Red;
            UiState.Test = "clicked";
        }
        

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    
}