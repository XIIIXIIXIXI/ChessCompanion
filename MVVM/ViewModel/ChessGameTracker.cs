using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.ViewModel
{
    class ChessGameTracker
    {

     
        public static void TestFindGame()
        {
            ChessBoard board = new ChessBoard();
            ChessEngine engine = new ChessEngine();
            Scraper scraper = new Scraper();
            ChessViewModel viewModel = new ChessViewModel();

            while (true)
            {
                //TODO: Execute when your turn
                scraper.WaitForOpponentToMove();
                board.ModifyBoard(scraper.ExtractChessPieces());
                string FEN = board.GetFEN(scraper.BlackOrWhiteToMove());
                engine.setFENPosition(FEN);
                viewModel.FEN = FEN;
                viewModel.BestMove = engine.getBestMove();
                viewModel.Evaluation = engine.getEvaluation().Value.ToString();
                scraper.WaitForPlayerToMove();
            }
        }
    }

}
