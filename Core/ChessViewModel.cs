using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ChessCompanion
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        Scraper scraper = new Scraper();
        private MainState _state = new MainState();
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
            scraper.FindPlayerColor();
            State.IsWhite = scraper.isWhite;
        }

        public void UpdateMoves()
        {

        }






        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}






    /*

        private ChessEngine engine;
        private ChessBoard board;

        private string _fen;
        public string FEN
        {
            get { return _fen; }
            set
            {
                if (_fen != value)
                {
                    _fen = value;
                    OnPropertyChanged("FEN");
                }
            }
        }

        private string _bestMove;
        public string BestMove
        {
            get { return _bestMove; }
            set
            {
                if (_bestMove != value)
                {
                    _bestMove = value;
                    OnPropertyChanged("BestMove");
                }
            }
        }

        private string _evaluation;
        public string Evaluation
        {
            get { return _evaluation; }
            set
            {
                if (_evaluation != value)
                {
                    _evaluation = value;
                    OnPropertyChanged("Evaluation");
                }
            }
        }

        public ChessViewModel()
        {
            engine = new ChessEngine();
            scraper = new Scraper();
            board = new ChessBoard();

            Task.Run(() => TestFindGame());
        }

        private async Task TestFindGame()
        {
            while (true)
            {
                //TODO: Execute when your turn
                scraper.WaitForPlayerToMove();

                board.ModifyBoard(scraper.ExtractChessPieces());
                string fen = board.GetFEN(scraper.BlackOrWhiteToMove());
                FEN = fen;
                engine.setFENPosition(fen);
                BestMove = engine.getBestMove();
                // Evaluation = engine.getEvaluation().Value.ToString();

                scraper.WaitForOpponentToMove();
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}*/