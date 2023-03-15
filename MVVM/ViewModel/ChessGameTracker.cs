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

       
        public static void TestFindGame(ChessViewModel viewModel)
        {
            

            while (true)
            {
                viewModel.WaitForOpponentToMove();
               
                viewModel.UpdateMoves();
                viewModel.WaitForPlayerToMove();
            }
        }
    }

}
