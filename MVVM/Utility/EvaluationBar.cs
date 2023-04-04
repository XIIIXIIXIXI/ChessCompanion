using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChessCompanion.MVVM.Utility
{
    public class EvaluationBar
    {
        public IWebDriver driver;

        public EvaluationBar(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void CreateBar(bool isWhite)
        {
            if (isWhite)
            {
                string jsCode = @"// Remove existing board-layout-evaluation elements
var existingEvaluationBars = document.querySelectorAll('.board-layout-evaluation');
existingEvaluationBars.forEach(function(existingEvaluationBar) {
  existingEvaluationBar.remove();
});
var evaluationBar = document.createElement('div');
evaluationBar.setAttribute('id', 'board-layout-evaluation');
evaluationBar.setAttribute('class', 'board-layout-evaluation');
evaluationBar.setAttribute('bis_skin_checked', '1');

var evaluationBarBar = document.createElement('div');
evaluationBarBar.setAttribute('class', 'evaluation-bar-bar undefined');
evaluationBarBar.setAttribute('bis_skin_checked', '1');

var scoreAbbreviated = document.createElement('span');
scoreAbbreviated.setAttribute('class', 'evaluation-bar-scoreAbbreviated evaluation-bar-dark');
scoreAbbreviated.textContent = '1.6';

var score = document.createElement('span');
score.setAttribute('class', 'evaluation-bar-score evaluation-bar-dark');
score.textContent = '+1.67';

var evaluationBarFill = document.createElement('div');
evaluationBarFill.setAttribute('class', 'evaluation-bar-fill');
evaluationBarFill.setAttribute('bis_skin_checked', '1');

var colorBlack = document.createElement('div');
colorBlack.setAttribute('class', 'evaluation-bar-color evaluation-bar-black');
colorBlack.setAttribute('bis_skin_checked', '1');

var colorDraw = document.createElement('div');
colorDraw.setAttribute('class', 'evaluation-bar-color evaluation-bar-draw');
colorDraw.setAttribute('bis_skin_checked', '1');

var colorWhite = document.createElement('div');
colorWhite.setAttribute('class', 'evaluation-bar-color evaluation-bar-white');
colorWhite.setAttribute('style', 'transform: translate3d(0px, 34.97%, 0px);');
colorWhite.setAttribute('bis_skin_checked', '1');

var critical = document.createElement('span');
critical.setAttribute('class', 'evaluation-bar-critical');
critical.setAttribute('style', 'display: none;');

colorWhite.appendChild(critical);
evaluationBarFill.appendChild(colorBlack);
evaluationBarFill.appendChild(colorDraw);
evaluationBarFill.appendChild(colorWhite);

evaluationBarBar.appendChild(scoreAbbreviated);
evaluationBarBar.appendChild(score);
evaluationBarBar.appendChild(evaluationBarFill);

evaluationBar.appendChild(evaluationBarBar);

var evaluationBarParent = document.querySelector('.board-layout-chessboard');
evaluationBarParent.appendChild(evaluationBar);";

                IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                jsExecutor.ExecuteScript(jsCode);
            }
        }

        public void UpdateBar(bool isWhite, int? cp, int? mate)
        {
            string? scoreText;
            double percentage;
            if (mate == null)
            {
                double score = (double)cp / 1000;
                scoreText = score.ToString();
                double a = 0.239; // decay constant
                percentage = 100 / (1 + Math.Exp(-a * score));

                // Hardcap at 95% and 5%
                percentage = Math.Min(percentage, 95);
                percentage = Math.Max(percentage, 5);
            }
            else
            {
                scoreText ="#" + mate.ToString();
                percentage = 100;
            }
        
            // Find the evaluation bar element
            IWebElement evaluationBar = driver.FindElement(By.Id("board-layout-evaluation"));

            // Find the score abbreviated element
            IWebElement scoreAbbreviated = evaluationBar.FindElement(By.ClassName("evaluation-bar-scoreAbbreviated"));

            // Find the color white element
            IWebElement colorWhite = evaluationBar.FindElement(By.ClassName("evaluation-bar-white"));

            // Change the text content of score abbreviated
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].textContent = arguments[1];", scoreAbbreviated, scoreText);

            // Change the percentage of color white
            jsExecutor.ExecuteScript($"arguments[0].setAttribute('style', 'transform: translate3d(0px, {percentage.ToString("0.00", CultureInfo.InvariantCulture)}%, 0px);')", colorWhite);

        }
    }
}
