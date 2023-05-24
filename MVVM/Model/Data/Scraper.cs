using ChessCompanion.MVVM.Model.Data;
using ChessCompanion.MVVM.Model.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    //when element is present game has started
    public void WaitForResignElement(int seconds)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("span.small-controls-icon.icon-font-chess.flag, span.icon-font-chess.flag.resign-button-icon")));
    }
    //draw icon on square that indicates how good a move was
    public void ShowAnalyzedIcon (string lastBestMove, IconData lastMoveScore)
    {
        string jsCode = @"
            // Get the board element
            const board = document.querySelector('.board-layout-main .board');

            const div = document.createElement('div');
            div.classList.add('effect', 'square-" + lastBestMove + @"');
            div.setAttribute('bis_skin_checked', '1');

            // Create the svg element
            const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            svg.setAttribute('class', '');
            svg.setAttribute('width', '100%');
            svg.setAttribute('height', '100%');
            svg.setAttribute('viewBox', '0 0 18 19');

            // Create the g element
            const g = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            g.setAttribute('id', 'excellent');

            // Create the first group of paths
            const g1 = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            const shadowPath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            shadowPath.setAttribute('class', 'icon-shadow');
            shadowPath.setAttribute('opacity', '0.3');
            shadowPath.setAttribute('d', 'M9,.5a9,9,0,1,0,9,9A9,9,0,0,0,9,.5Z');
            const backgroundPath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            backgroundPath.setAttribute('class', 'icon-background');
            backgroundPath.setAttribute('fill', '" + lastMoveScore.Color + @"');
            backgroundPath.setAttribute('d', 'M9,0a9,9,0,1,0,9,9A9,9,0,0,0,9,0Z');
            g1.appendChild(shadowPath);
            g1.appendChild(backgroundPath);

            // Create the second group of paths
            const g2 = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            g2.setAttribute('class', 'icon-component-shadow');
            g2.setAttribute('opacity', '0.0');
            const path1 = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path1.setAttribute('d', 'M13.79,11.34c0-.2.4-.53.4-.94S14,9.72,14,9.58a2.06,2.06,0,0,0,.18-.83,1,1,0,0,0-.3-.69,1.13,1.13,0,0,0-.55-.2,10.29,10.29,0,0,1-2.07,0c-.37-.23,0-1.18.18-1.7S11.9,4,10.62,3.7c-.69-.17-.66.37-.78.9-.05.21-.09.43-.13.57A5,5,0,0,1,7.05,8.23a1.57,1.57,0,0,1-.42.18v4.94A7.23,7.23,0,0,1,8,13.53c.52.12.91.25,1.44.33A11.11,11.11,0,0,0,11,14a6.65,6.65,0,0,0,1.18,0,1.09,1.09,0,0,0,1-.59.66.66,0,0,0,.06-.2,1.63,1.63,0,0,1,.07-.3c.13-.28.37-.3.5-.68S13.74,11.53,13.79,11.34Z');
            const path2 = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path2.setAttribute('d', 'M5.49,8.09H4.31a.5.5,0,0,0-.5.5v4.56a.5.5,0,0,0,.5.5H5.49a.5.5,0,0,0,.5-.5V8.59A.5.5,0,0,0,5.49,8.09Z');
            g2.appendChild(path1);
            g2.appendChild(path2);

            // Create the third group of paths
            const g3 = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            const path3 = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path3.setAttribute('class', 'icon-component');
            path3.setAttribute('fill', '#fff');
            path3.setAttribute('d', '" + lastMoveScore.IconComponent1 + @"');
            const path4 = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path4.setAttribute('class', 'icon-component');
            path4.setAttribute('fill', '#fff');
            path4.setAttribute('d', '" + lastMoveScore.IconComponent2 + @"');
            g3.appendChild(path3);
            g3.appendChild(path4);

            // Append the elements
            g.appendChild(g1);
            g.appendChild(g2);
            g.appendChild(g3);
            svg.appendChild(g);
            div.appendChild(svg);
            // Add the element to the document
            board.appendChild(div);";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript(jsCode);
    }
    public void removeAnalyzeIcon()
    {
        try
        {
            string jsCode = "document.querySelector('[class*=\"effect\"]').remove();";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript(jsCode);
        }
        catch { }
        
    }




}
