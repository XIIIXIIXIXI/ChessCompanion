﻿using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ChessCompanion
{
    public class ChessViewModel : INotifyPropertyChanged
    {
        Scraper scraper = new Scraper();
        ChessBoard board = new ChessBoard();
        IEngine engine = new Engine(@"C:\Users\marti\source\repos\chessEval\chessEval\stockfish_20090216_x64_avx2");
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

        public void GetBestMove()
        {
           
            board.ModifyBoard(scraper.ExtractChessPieces());
            State.FEN = board.GetFENString(scraper.BlackOrWhiteToMove());
            engine.SetPosition(State.FEN);
            State.Moves = engine.GetBestMove(300);
            //SET FEN POSITION HERE
            //GET BEST MOVE HERE
        }
        public void GetBestMoveWithInfo()
        {
            board.ModifyBoard(scraper.ExtractChessPieces());
            State.FEN = board.GetFENString(scraper.BlackOrWhiteToMove());
            engine.SetPosition(State.FEN);
            // Get the best move, PV, and score
            (string bestMove, int? cp, string pv) result = engine.GetBestMoveWithInfo(300);

            // Save the values to variables
            State.Moves = result.bestMove;
            State.PV = result.pv;
            State.CP = result.cp;
            
        }
        public void makeMove()
        {
            scraper.MakeMove(State.Moves);
        }
        public void WaitForOpponentToMove()
        {
            scraper.WaitForOpponentToMove();
        }
        public void WaitForPlayerToMove()
        {
            scraper.WaitForPlayerToMove();
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