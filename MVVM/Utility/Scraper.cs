using ChessCompanion.MVVM.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;



public class Scraper
{
    public IWebDriver driver; 
    


    public Scraper(IWebDriver driver)
    {
        this.driver = driver;
        //NavigateToWebsite();
        //LogIn("jagabomba@hotmail.com", "Jagabomba9");
        PlayComputer();

        //FindGame();
        
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
    

    public void WaitForResignElement(int seconds)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("span.small-controls-icon.icon-font-chess.flag, span.icon-font-chess.flag.resign-button-icon")));
    }
   

    

}
