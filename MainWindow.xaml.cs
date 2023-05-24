using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Model.Data;
using ChessCompanion.MVVM.ViewModel;
using OpenQA.Selenium.Chrome;
using ChessCompanion.Core;
using System.IO;

namespace ChessCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameMediator mediator;
        private readonly ChessGameTracker gameTracker;
        
        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true; //Makes window always on top
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // This hides the console window
            var driver = new ChromeDriver(service);
            var scraper = new Scraper(driver);
            var gameScraper = new GameScraper(driver);
            var board = new ChessBoard();

            //IEngine engine = new Engine(@"C:\Users\marti\source\repos\chessEval\chessEval\stockfish_20090216_x64_avx2");
            string folderPath = @"stockfish_20090216_x64"; // folder is located in the same directory as your application

            string currentDirectory = Directory.GetCurrentDirectory(); // Get the current working directory
            string fullPath = System.IO.Path.Combine(currentDirectory, folderPath); // Combine the current directory with the specified folder path
            IEngine engine = new Engine(@"stockfish_20090216_x64");
            //IEngine engine = new Engine(@"C:\Users\marti\source\repos\martinkoch1\Maia");
            var evaluationBar = new EvaluationBar(driver);
            

            mediator = new GameMediator(driver, scraper, board, engine, gameScraper, evaluationBar);
            // UI
            
            ChessViewModel viewModel = new ChessViewModel(mediator);
            //
            DataContext = viewModel;
            gameTracker = new ChessGameTracker(mediator);

            Task.Run(() => gameTracker.TestFindGame());

           
        }

    }
   

}
