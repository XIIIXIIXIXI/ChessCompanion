using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using ChessCompanion.MVVM.ViewModel;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

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
            UiState.SelectedIndexChanged += OnSelectedIndexChanged;
            ButtonClickCommand = new RelayCommand(OnButtonClick);
          
        }
        public MainState State => mediator.State;
        public UIState UiState => uiState;

        private void OnButtonClick(object parameter)
        {
            
            Debug.WriteLine("Clicked");
            UiState.IsAutomoveEnabled = !UiState.IsAutomoveEnabled;
            UiState.Test = "clicked";
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"Selected index changed to {UiState.SelectedIndex}");
            var mainWindow = Application.Current.MainWindow;
            //var grid0 = mainWindow.FindName("grid0") as Grid;
            var grid1 = mainWindow.FindName("grid1") as Grid;
            var grid2 = mainWindow.FindName("grid2") as Grid;
            var grid3 = mainWindow.FindName("grid3") as Grid;
            var grid4 = mainWindow.FindName("grid4") as Grid;
            switch (UiState.SelectedIndex)
            {
                case 0:
                    //Visible
                    //grid0.Visibility = Visibility.Visible;
                    // Hidden
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    // Visible
                    grid1.Visibility = Visibility.Visible;
                    // Hidden
                    grid2.Visibility = Visibility.Collapsed;
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    // Visible
                    grid1.Visibility = Visibility.Visible;
                    grid2.Visibility = Visibility.Visible;
                    // Hidden
                    grid3.Visibility = Visibility.Collapsed;
                    grid4.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    // Visible
                    grid1.Visibility = Visibility.Visible;
                    grid2.Visibility = Visibility.Visible;
                    grid3.Visibility = Visibility.Visible;
                    // Hidden
                    grid4.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    // Visible
                    grid1.Visibility = Visibility.Visible;
                    grid2.Visibility = Visibility.Visible;
                    grid3.Visibility = Visibility.Visible;
                    grid4.Visibility = Visibility.Visible;
                    // Hidden
                    break;
            }

            mediator.SetEngineOption("MultiPV", UiState.SelectedIndex+1);
        }




        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            UiState.SelectedIndexChanged -= OnSelectedIndexChanged;
        }
    }
    
}