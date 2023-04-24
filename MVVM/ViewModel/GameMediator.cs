using ChessCompanion.Core;
using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.ViewModel
{
    public class GameMediator
    {
        private readonly IWebDriver driver;
        private readonly Scraper scraper;
        private readonly GameScraper gameScraper;
        private readonly ChessBoard board;
        private readonly IEngine engine;
        private readonly MainState state = new MainState();
        private readonly TopMove currentBestMove = new TopMove();
        private readonly TopMove lastBestMove = new TopMove();
        private readonly EvaluationBar evaluationBar;

        //game flow variables
        public bool isAutoMoveEnabled = false;
        

        public GameMediator(IWebDriver driver, Scraper scraper, ChessBoard board, IEngine engine, GameScraper gameScraper, EvaluationBar evaluationBar)
        {
            this.driver = driver;
            this.scraper = scraper;
            this.board = board;
            this.engine = engine;
            this.gameScraper = gameScraper;
            this.evaluationBar = evaluationBar;
        }

        public MainState State => state;

        public void UpdatePlayerColor()
        {
            gameScraper.FindPlayerColor();
            State.IsWhite = gameScraper.isWhite;
        }

        public void GetBestMove()
        {
            UpdateBoardState();

            engine.SetPosition(State.FEN);
            //State.Moves = engine.GetBestMove(300);
        }

        public void GetBestMoveWithInfo()
        {
            UpdateBoardState();

            engine.SetPosition(State.FEN);
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);
            UpdateCurrentBestMove(bestMove, cp, mate, promotion, pv);
            //UpdateStateWithCurrentBestMove();
        }
        public void GetBestMoveMultiLines()
        {
            UpdateBoardState();
            engine.SetPosition(State.FEN);
            TopMove[] topMoves = engine.GetMultipleLines(300);
            UpdateCurrentBestMove(topMoves[0].bestMove, topMoves[0].cp, topMoves[0].mate, topMoves[0].promotion, topMoves[0].pv);
            UpdateStatesWithCurrentTopMoves(topMoves);
        }
        public void makeBestMove()
        {
            gameScraper.MakeMove(currentBestMove.bestMove);
        }

        public void AnalyzeMove()
        {
            scraper.removeAnalyzeIcon();
            UpdateLastBestMove();
            UpdateBoardState();

            engine.SetPosition(State.FEN);
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);
            UpdateCurrentBestMove(bestMove, cp, mate, promotion, pv);

            MoveScore score = engine.AnalyzeLastMove(lastBestMove, currentBestMove);

            string move = gameScraper.GetLatestMoveForWhite();
            string square = board.TranslateMoveToSquare(move, gameScraper.isWhite);
            scraper.ShowAnalyzedIcon(square, MoveScoreColors.IconData[score]);
        }

        public void EvaluationBarOn()
        {
            evaluationBar.CreateBar(gameScraper.isWhite);
        }

        public void UpdateEvaluationBar()
        {
            evaluationBar.UpdateBar(gameScraper.isWhite, currentBestMove.cp, currentBestMove.mate, gameScraper.BlackOrWhiteToMove());
        }
        public void SetEngineOption(string name, object value)
        {
            engine.SetOption(name, value);
        }

        public void setEngineLines(int lines)
        {
            engine.setLines(lines);
        }

        private void UpdateBoardState()
        {
            board.ModifyBoard(gameScraper.ExtractChessPieces());
            State.FEN = board.GetFENString(gameScraper.BlackOrWhiteToMove());
        }

        private void UpdateCurrentBestMove(string bestMove, int? cp, int? mate, bool promotion, string pv)
        {
            currentBestMove.setTopMove(bestMove, cp, mate, promotion, pv);
            currentBestMove.FEN = board.GetFENString(gameScraper.BlackOrWhiteToMove());
        }

        private void UpdateStatesWithCurrentTopMoves(TopMove[] topMoves)
        {
            for (int i = 0; i < topMoves.Length; i++)
            {
                State.MoveInfos[i].Moves = topMoves[i].bestMove;
                State.MoveInfos[i].PV = topMoves[i].pv;

                if (State.MoveInfos[i].MATE == null)
                {
                    State.MoveInfos[i].MATE = null;
                    State.MoveInfos[i].CP = topMoves[i].cp;
                }
                else
                {
                    State.MoveInfos[i].CP = null;
                    State.MoveInfos[i].MATE = topMoves[i].mate;
                }
                State.MoveInfos[i].CP = topMoves[i].cp;
                State.MoveInfos[i].MATE = topMoves[i].mate;
            }
        }

/*        private void UpdateStateWithCurrentBestMove()
        {
            State.Moves = currentBestMove.bestMove;
            State.PV = currentBestMove.pv;
            if (currentBestMove.mate == null)
            {
                State.MATE = null;
                State.CP = currentBestMove.cp;
            }
            else
            {
                State.CP = null;
                State.MATE = currentBestMove.mate;
            }
        }
*/
        
        private void UpdateLastBestMove()
        {
            lastBestMove.setTopMove(currentBestMove.bestMove, currentBestMove.cp, currentBestMove.mate, currentBestMove.promotion, currentBestMove.pv);
            lastBestMove.FEN = board.GetFENFromMove(lastBestMove.bestMove, gameScraper.BlackOrWhiteToMove());
        }
        public void FirstMove()
        {
            (string bestMove, int? cp, int? mate, bool promotion, string pv) = engine.GetBestMoveWithInfo(300);

            currentBestMove.setTopMove(bestMove, cp, mate, promotion, pv);
        }
        //Utility code for game loop
      /*  public void makeMove()
        {
            gameScraper.MakeMove(State.Moves);
        }*/
        public void WaitForFirstMove()
        {
            gameScraper.WaitForFirstMove();
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
            State.IsWhite = gameScraper.isWhite;
        }
        public bool PlayingAsWhite()
        {
            return gameScraper.isWhite;
        }

        // Game flow
        public void EnableAutoMove()
        {
            isAutoMoveEnabled = true;
            // abort the wait in WaitForPlayerToMove() function
            if (gameScraper.cancellationTokenSource != null)
            {
                gameScraper.cancellationTokenSource.Cancel();
            }
            gameScraper.cancellationTokenSource =  new CancellationTokenSource(); // create new instance of CancellationTokenSource
            makeBestMove();
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
