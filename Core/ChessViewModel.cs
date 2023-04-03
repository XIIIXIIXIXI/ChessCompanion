using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using OpenQA.Selenium;
using System.ComponentModel;


namespace ChessCompanion
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        private readonly IWebDriver driver;
        private readonly Scraper scraper;
        private readonly GameScraper gameScraper;
        private readonly ChessBoard board;
        private readonly IEngine engine;
        private MainState _state = new MainState();
        private TopMove currentBestMove = new TopMove();
        private TopMove lastBestMove = new TopMove();
        //private string lastMoveScore;

        public ChessViewModel(IWebDriver driver, Scraper scraper, ChessBoard board, IEngine engine, GameScraper Gamescraper)
        {
            this.driver = driver;
            this.scraper = scraper;
            this.board = board;
            this.engine = engine;
            this.gameScraper = Gamescraper;
        }
        

        public MainState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
        

        public void UpdatePlayerColor()
        {
            gameScraper.FindPlayerColor();
            State.IsWhite = gameScraper.isWhite;
        }

        public void GetBestMove()
        {
           
            board.ModifyBoard(gameScraper.ExtractChessPieces());
            State.FEN = board.GetFENString(gameScraper.BlackOrWhiteToMove());
            engine.SetPosition(State.FEN);
            State.Moves = engine.GetBestMove(300);
            //SET FEN POSITION HERE
            //GET BEST MOVE HERE
        }
        public void GetBestMoveWithInfo()
        {

            board.ModifyBoard(gameScraper.ExtractChessPieces());
            State.FEN = board.GetFENString(gameScraper.BlackOrWhiteToMove());
            engine.SetPosition(State.FEN);
            // Get the best move, PV, and score
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);
            currentBestMove.setTopMove(bestMove, cp, mate, promotion, pv);

            // Save the values to variables
            State.Moves = currentBestMove.bestMove;
            State.PV = currentBestMove.pv;
            if (currentBestMove.mate == null)
            {
                State.CP = currentBestMove.cp;
            }
            else
            {
                State.MATE = currentBestMove.mate;
            }

        }
        public void AnalyzeMove()
        {
            scraper.removeAnalyzeIcon();
            lastBestMove.setTopMove(currentBestMove.bestMove, currentBestMove.cp, currentBestMove.mate, currentBestMove.promotion, currentBestMove.pv);
            lastBestMove.FEN = board.GetFENFromMove(lastBestMove.bestMove, gameScraper.BlackOrWhiteToMove());
            board.ModifyBoard(gameScraper.ExtractChessPieces());
            currentBestMove.FEN = board.GetFENString(gameScraper.BlackOrWhiteToMove());
            engine.SetPosition(currentBestMove.FEN);
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);

            currentBestMove.setTopMove(bestMove, cp, mate, promotion, pv);

            MoveScore score = engine.AnalyzeLastMove(lastBestMove, currentBestMove);

            string move = gameScraper.GetLatestMoveForWhite();
            string square = board.TranslateMoveToSquare(move, gameScraper.isWhite);
            scraper.ShowAnalyzedIcon(square, MoveScoreColors.IconData[score]);
        }
        public void FirstMove()
        {
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);

            currentBestMove.setTopMove(bestMove, cp, mate, promotion, pv);
        }
        public void makeMove()
        {
            gameScraper.MakeMove(State.Moves);
        }
        public void WaitForOpponentToMove()
        {
            gameScraper.WaitForOpponentToMove();
        }
        public void WaitForPlayerToMove()
        {
            gameScraper.WaitForPlayerToMove();
        }
        public bool IsResignElementPresent()
        {
            return gameScraper.IsResignElementPresent();
        }

        public void WaitForResignElement(int seconds)
        {
            scraper.WaitForResignElement(seconds);
        }
        public void InitGameScraper()
        {
            gameScraper.Setup();
        }
        public bool PlayingAsWhite()
        {
            return gameScraper.isWhite;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}