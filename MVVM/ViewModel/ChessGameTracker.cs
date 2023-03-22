using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.ViewModel
{
    class ChessGameTracker
    {

       
        public static void TestFindGame(ChessViewModel viewModel)
        {
           //wait for game to start
           //if playing as white
           //else

            while (true)
            {
                viewModel.WaitForOpponentToMove();
               
                //viewModel.GetBestMove();
                viewModel.GetBestMoveWithInfo();
                viewModel.makeMove();
                viewModel.WaitForPlayerToMove();
                Debug.WriteLine("------------");
            }
        }
    }

}
