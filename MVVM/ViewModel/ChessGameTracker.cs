using ChessCompanion.MVVM.Model;
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
        private readonly GameMediator mediator;

        public ChessGameTracker(GameMediator mediator)
        {
            this.mediator = mediator;
        }
        public void TestFindGame()
        {
          while(true)
            {
                //wait for game to start
                mediator.WaitForResignElement(1000);
                mediator.InitGameScraper();
                if (mediator.PlayingAsWhite())
                {
                    mediator.State.Moves = "e2e4";
                    mediator.FirstMove();
                    mediator.makeMove(); 
                }
                else
                {
                    mediator.WaitForFirstMove();
                }
                
                mediator.EvaluationBarOn();
                while (mediator.IsResignElementPresent())
                {
                    mediator.WaitForOpponentToMove();
                    mediator.GetBestMoveWithInfo();
                    mediator.UpdateEvaluationBar();
                    mediator.WaitForPlayerToMove();
                    mediator.AnalyzeMove();
                    mediator.UpdateEvaluationBar();
                    Debug.WriteLine("------------");
                }
            }
            
        }
    }

}
