using System;
using System.Collections.Generic;
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
using ChessCompanion.MVVM.Utility;
using ChessCompanion.MVVM.ViewModel;
using OpenQA.Selenium.Chrome;

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
            var driver = new ChromeDriver();
            var scraper = new Scraper(driver);
            var gameScraper = new GameScraper(driver);
            var board = new ChessBoard();
            IEngine engine = new Engine(@"C:\Users\marti\source\repos\chessEval\chessEval\stockfish_20090216_x64_avx2");
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
