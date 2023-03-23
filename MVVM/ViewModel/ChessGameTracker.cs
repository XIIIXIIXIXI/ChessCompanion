﻿using ChessCompanion.MVVM.Model;
using ChessCompanion.MVVM.Utility;
using OpenQA.Selenium.Chrome;
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
          while(true)
            {
                //wait for game to start
                viewModel.WaitForResignElement(1000);
                viewModel.InitGameScraper();
                if (viewModel.PlayingAsWhite())
                {
                    viewModel.State.Moves = "e2e4";
                    viewModel.makeMove();
                }

                while (viewModel.IsResignElementPresent())
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

}
