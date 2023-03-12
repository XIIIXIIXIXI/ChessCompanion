using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class Scraper
{
    public IWebDriver driver { get; private set; }
    bool playsAsWhite;

    public Scraper()
    {
        driver = new ChromeDriver();
        NavigateToWebsite();
        LogIn("jagabomba@hotmail.com", "Jagabomba9");
        //PlayComputer();
        FindGame();
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
        }
    }

    //Return the players playing color
    public void FindPlayerColor()
    {
        // Find the clock element
        IWebElement clockElement = driver.FindElement(By.CssSelector(".clock-component.clock-top"));

        // Get the value of the "class" attribute
        string classValue = clockElement.GetAttribute("class");

        if (classValue.Contains("clock-black"))
        {
            playsAsWhite = true;
        }
        //classValue.Contains("clock-white")
        else
        {
            playsAsWhite = false;
        }

    }


    public void WaitForOpponentToMove()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(600));
        wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".clock-component.clock-top.clock-player-turn")));
    }

    public void WaitForPlayerToMove()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(600));
        wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".clock-component.clock-top:not(.clock-player-turn)")));
    }

}
