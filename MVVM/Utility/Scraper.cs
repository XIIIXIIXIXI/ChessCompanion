﻿using ChessCompanion.MVVM.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Windows.Input;


public class Scraper
{
    public IWebDriver driver { get; private set; }
    private readonly Dictionary<int, string> movesList = new Dictionary<int, string>();
    public bool isWhite;
    private int squareWidth;
    private IWebElement gameboard;


    public Scraper()
    {
        driver = new ChromeDriver();
        //NavigateToWebsite();
        //LogIn("jagabomba@hotmail.com", "Jagabomba9");
        PlayComputer();

        //FindGame();
        CaptureBoardPosition();
        FindPlayerColor();
    }

    public void NavigateToWebsite()
    {
        driver.Navigate().GoToUrl("https://www.chess.com/login");

    }
    public void LogIn(string username, string password)
    {
        IWebElement emailField = driver.FindElement(By.Id("username"));
        emailField.SendKeys(username);

        IWebElement passwordField = driver.FindElement(By.Id("password"));
        passwordField.SendKeys(password);

        Thread.Sleep(1000);

        IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
        loginButton.Click();

        // Wait for the login process to complete
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        driver.Manage().Window.Maximize();
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://www.chess.com/home"));

    }
    public void PlayComputer()
    {
        driver.Navigate().GoToUrl("https://www.chess.com/play/computer");
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100000));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://www.chess.com/play/computer"));


    }

    public void FindGame()
    {
        driver.Navigate().GoToUrl("https://www.chess.com/live");

        // Wait for the game to start
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100000));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://www.chess.com/play/online"));

        Thread.Sleep(2000);

        IWebElement playButton = driver.FindElement(By.XPath("//*[@id=\"board-layout-sidebar\"]/div/div[2]/div/div[1]/div[1]/button"));
        playButton.Click();

        wait.Until(ExpectedConditions.UrlContains("https://www.chess.com/game/live/"));

    }
    public IReadOnlyCollection<IWebElement> ExtractChessPieces()
    {

        // Find all the chess piece elements using the CSS selector
        IReadOnlyCollection<IWebElement> chessPieceElements = driver.FindElements(By.CssSelector("div.piece"));


        return chessPieceElements;
    }

    public void SetUpRemoteBoard()
    {
        IWebElement element = driver.FindElement(By.Id("myElement"));
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
    

       
    public List<string> GetMoveList()
    {
        
        var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
        
        if (moveListElem == null)
        {
            return null;
        }

        // Select all children with class containing "white node" or "black node"
        // Moves that are not pawn moves have a different structure
        // containing children
        IReadOnlyCollection<IWebElement> moves;
        if (!movesList.Any())
        {
            // If the moves list is empty, find all moves
            moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
        }
        else
        {
            // If the moves list is not empty, find only the new moves
            moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]:not([data-processed])"));
        }

        foreach (var move in moves)
        {
            var moveClass = move.GetAttribute("class");

            // Check if it is indeed a move
            if (moveClass.Contains("white node") || moveClass.Contains("black node"))
            {
                // Check if it has a figure
                string figure = null;
                try
                {
                    var child = move.FindElement(By.XPath("./*"));
                    figure = child.GetAttribute("data-figurine");
                }
                catch (NoSuchElementException)
                {
                    // Ignore if there is no figure
                }

                // Check if it was en-passant or figure-move
                if (figure == null)
                {
                    // If the movesList is empty or the last move was not the current move
                    movesList[Convert.ToInt32(move.GetAttribute("data-ply"))] = move.Text;
                }
                else
                {
                    // Check if it is promotion
                    if (move.Text.Contains("="))
                    {
                        var m = move.Text + figure;

                        // If the move is a check, add the + in the end
                        if (m.Contains("+"))
                        {
                            m = m.Replace("+", "") + "+";
                        }

                        // If the movesList is empty or the last move was not the current move
                        movesList[Convert.ToInt32(move.GetAttribute("data-ply"))] = m;
                    }
                    else
                    {
                        // If the movesList is empty or the last move was not the current move
                        movesList[Convert.ToInt32(move.GetAttribute("data-ply"))] = figure + move.Text;
                    }
                }

                // Mark the move as processed
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].setAttribute('data-processed', 'true')", move);
            }
        }

        return movesList.Values.ToList();
    }


    public void WaitForOpponentToMove()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6000));
        if (isWhite)
        {
            // Wait up to 60 seconds for the move list to have an odd number of moves
            
            wait.Until(driver => {
                var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                int lastMove;
                int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                return lastMove % 2 == 0;
            });
        }
        else
        {
            wait.Until(driver => {
                var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                int lastMove;
                int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                return lastMove % 2 == 1;
            });
        }
        
        
    }

    public void WaitForPlayerToMove()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6000));
        if (isWhite)
        {
            // Wait up to 60 seconds for the move list to have an odd number of moves

            wait.Until(driver => {
                var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                int lastMove;
                int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                return lastMove % 2 == 1;
            });
        }
        else
        {
            wait.Until(driver => {
                var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
                var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
                int lastMove;
                int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
                return lastMove % 2 == 0;
            });
        }
    }

    public void MakeMove(string move)
    {
        //IWebElement piece = driver.FindElement(By.CssSelector("#board-vs-personalities > div.piece.br.square-18"));
        //IWebElement gameBoard = driver.FindElement(By.ClassName("coordinates"));

        // Get the square width and offsets
        int squareWidth = this.squareWidth;
        int offsetX = this.gameboard.Location.X;
        int offsetY = gameboard.Location.Y;

        // Mouse down on the center of the e2 square
        //int clientX = squareWidth * 4 + offsetX + squareWidth / 2;
        //int clientY = squareWidth * 6 + offsetY + squareWidth / 2;

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
        int destFY = destY - sourceElement.Location.Y  - (squareWidth / 2);


        // Click and drag the piece from the source square to the destination coordinates
        Actions actions = new Actions(driver);
        actions.MoveToElement(sourceElement)
               .ClickAndHold()
               .MoveByOffset(destFX, destFY)
               .Release()
               .Perform();

    }

    public void CaptureBoardPosition()
    {
        IWebElement piece = driver.FindElement(By.CssSelector("#board-vs-personalities > div.piece.br.square-18"));
        //IWebElement piece = driver.FindElement(By.CssSelector("#board-single > div.piece.br.square-18"));

        IWebElement gameBoard = driver.FindElement(By.ClassName("coordinates"));
        this.squareWidth = piece.Size.Width;
        this.gameboard = gameBoard;
    }

}
