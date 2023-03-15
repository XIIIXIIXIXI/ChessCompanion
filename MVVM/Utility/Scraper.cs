using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

public class Scraper
{
    public IWebDriver driver { get; private set; }
    private readonly Dictionary<int, string> movesList = new Dictionary<int, string>();
    public bool isWhite;

    public Scraper()
    {
        driver = new ChromeDriver();
        /*NavigateToWebsite();
        LogIn("jagabomba@hotmail.com", "Jagabomba9");
        //PlayComputer();
        FindGame();
        FindPlayerColor();*/
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
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100000));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://www.chess.com/play/computer"));

        Thread.Sleep(15000);
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
    public char BlackOrWhiteToMove()
    {
        var moveListElem = driver.FindElement(By.TagName("vertical-move-list"));
        var moves = moveListElem.FindElements(By.CssSelector("div.move [data-ply]"));
        int lastMove;
        int.TryParse(moves.Last().GetAttribute("data-ply"), out lastMove);
        if (lastMove % 2 == 0)
        {
            return 'b';
        }
        else if (lastMove % 2 == 1)
        {
            return 'w';
        }
        else
        {
            return 'x';
        }
        /*
        // Find the clock element
        IWebElement clockElement = driver.FindElement(By.CssSelector(".clock-component.clock-top"));

        // Get the value of the "class" attribute
        string classValue = clockElement.GetAttribute("class");

        // Check if the "clock-white" class is present

        //opponent turn and black
        if (classValue.Contains("clock-player-turn") && classValue.Contains("clock-black"))
        {
            return 'b';
        }
        //your turn and white
        else if (classValue.Contains("clock-black"))
        {
            return 'w';
        }
        //opponent turn and white
        else if (classValue.Contains("clock-white") && classValue.Contains("clock-player-turn"))
        {
            return 'w';
        }
        //your turn and black
        else
        {
            return 'b';
        }*/
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
        // Find the moves list
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

}
