using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChessCompanion.MVVM.Utility
{
    public class GameScraper
    {
        private readonly IWebDriver driver;
        public bool isWhite;
        private int squareWidth;
        private IWebElement gameboard;

        public GameScraper(IWebDriver driver)
        {
            this.driver = driver;
            //CaptureBoardPosition();
            //FindPlayerColor();
        }
        public void Setup()
        {
            CaptureBoardPosition();
            FindPlayerColor();
        }

        public IReadOnlyCollection<IWebElement> ExtractChessPieces()
        {

            // Find all the chess piece elements using the CSS selector
            IReadOnlyCollection<IWebElement> chessPieceElements = driver.FindElements(By.CssSelector("div.piece"));

            return chessPieceElements;
        }
        // wait for the class "move to be present
        public void WaitForFirstMove()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6000));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div.move")));
        }
        public char BlackOrWhiteToMove()
        {
            var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
            var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
            int lastMove;
            int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
            if (lastMove % 2 == 0)
            {
                return 'w';
            }
            else if (lastMove % 2 == 1)
            {
                return 'b';
            }
            else
            {
                return 'x';
            }
        }

        //Return the players playing color
        public void FindPlayerColor()
        {
            // Find the <chess-board> element
            var chessBoardElement = driver.FindElement(By.CssSelector("chess-board"));

            // Check the value of its "class" attribute
            var isPlayingAsWhite = chessBoardElement.GetAttribute("class") == "board";

            // Print the result
            if (isPlayingAsWhite)
            {
                isWhite = true;

            }
            //classValue.Contains("clock-white")
            else
            {
                isWhite = false;
            }
        }

        public void WaitForOpponentToMove()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6000));
            if (isWhite)
            {
                // Wait up to 6000 seconds for the move list to have an odd number of moves
                
                    wait.Until(driver => {
                        var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                        var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                        int lastMove;
                        int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                        return lastMove % 2 == 0 || !IsResignElementPresent();
                    });
            }
            else
            {
                wait.Until(driver => {
                    var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                    var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                    int lastMove;
                    int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                    return lastMove % 2 == 1 || !IsResignElementPresent();
                });
            }
        }

        public void WaitForPlayerToMove()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6000));
            if (isWhite)
            {
                // Wait up to 6000 seconds for the move list to have an odd number of moves

                wait.Until(driver => {
                    var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                    var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                    int lastMove;
                    int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                    return lastMove % 2 == 1 || !IsResignElementPresent();
                });
            }
            else
            {
                wait.Until(driver => {
                    var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                    var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                    int lastMove;
                    int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                    return lastMove % 2 == 0 || !IsResignElementPresent();
                });
            }
        }

        //Make move for player by sending a click action to the website
        public void MakeMove(string move)
        {
            // Get the square width and offsets
            int squareWidth = this.squareWidth;
            int offsetX = this.gameboard.Location.X;
            int offsetY = gameboard.Location.Y;

            // Get the source and destination squares
            string sourceSquare = move.Substring(0, 2);
            string destSquare = move.Substring(2, 2);

            // Map the file and rank characters to indices
            int fileIndex = ChessConstants.FileLookup[sourceSquare[0]];

            sourceSquare = new string(new char[] { ChessConstants.FileLookup[sourceSquare[0]], sourceSquare[1] });

            // Get the source square element
            IWebElement sourceElement = driver.FindElement(By.CssSelector(".square-" + sourceSquare));

            // Calculate the destination coordinates
            int destX = offsetX + (int)(squareWidth * (destSquare[0] - 'a' + 0.5));
            int destY = offsetY + (int)(squareWidth * (8 - int.Parse(destSquare[1].ToString()) + 0.5));
            int destFX = destX - sourceElement.Location.X - (squareWidth / 2);
            int destFY = destY - sourceElement.Location.Y - (squareWidth / 2);

            // Click and drag the piece from the source square to the destination coordinates
            Actions actions = new Actions(driver);
            actions.MoveToElement(sourceElement)
                   .ClickAndHold()
                   .MoveByOffset(destFX, destFY)
                   .Release()
                   .Perform();

        }
        // Find the position of the board and square widths
        public void CaptureBoardPosition()
        {
            IWebElement piece = driver.FindElement(By.CssSelector("#board-vs-personalities > div.piece.br.square-18"));
            //IWebElement piece = driver.FindElement(By.CssSelector("#board-single > div.piece.br.square-18"));

            IWebElement gameBoard = driver.FindElement(By.ClassName("coordinates"));
            this.squareWidth = piece.Size.Width;
            this.gameboard = gameBoard;
        }
        
        public string GetLatestMoveForWhite()
        {
            // find all the move elements
            IReadOnlyCollection<IWebElement> moveElements = driver.FindElements(By.CssSelector(".move"));

            // iterate through the move elements in reverse order to find the latest move
            for (int i = moveElements.Count - 1; i >= 0; i--)
            {
                // get the white and black move elements for the current move
                IWebElement whiteMoveElement = moveElements.ElementAt(i).FindElement(By.CssSelector(".white"));
                //IWebElement blackMoveElement = moveElements.ElementAt(i).FindElement(By.CssSelector(".black"));
                
                // check if the player color matches the color of the last move made
                if (isWhite && whiteMoveElement != null)
                {
                    return whiteMoveElement.Text;
                }
                /*else if (!isWhite && blackMoveElement != null)
                {
                    return blackMoveElement.Text;
                }*/
            }

            // no move was found
            return null;
        }
       //if resign element is not present the game is over
        public bool IsResignElementPresent()
        {
            //, span.icon-font-chess.flag.resign-button-icon
            
            try
            {
                 driver.FindElement(By.CssSelector("button[data-cy='Resign']"));
                return true;
                
            }
            catch (NoSuchElementException)
            {
                try
                {
                    driver.FindElement(By.CssSelector("button[data-cy='Abort']"));
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
                
            }
        }

    }
    
}
