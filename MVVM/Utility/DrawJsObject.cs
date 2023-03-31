using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.Utility
{
   public class LastMoveIcon
    {
        public string ShowAnalyzedIcon (string lastMoveScore)
        {
            string jsCode = @"
            // Get the board element
            const board = document.querySelector('.board-layout-main .board');

            const div = document.createElement('div');
            div.classList.add('effect', 'square-54');
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
            backgroundPath.setAttribute('fill', '#b33430');
            backgroundPath.setAttribute('d', 'M9,0a9,9,0,1,0,9,9A9,9,0,0,0,9,0Z');
            g1.appendChild(shadowPath);
            g1.appendChild(backgroundPath);

            // Create the second group of paths
            const g2 = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            g2.setAttribute('class', 'icon-component-shadow');
            g2.setAttribute('opacity', '0.2');
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
            path3.setAttribute('d', 'M13.79,10.84c0-.2.4-.53.4-.94S14,9.22,14,9.08a2.06,2.06,0,0,0,.18-.83,1,1,0,0,0-.3-.69,1.13,1.13,0,0,0-.55-.2,10.29,10.29,0,0,1-2.07,0c-.37-.23,0-1.18.18-1.7s.51-2.12-.77-2.43c-.69-.17-.66.37-.78.9-.05.21-.09.43-.13.57A5,5,0,0,1,7.05,7.73a1.57,1.57,0,0,1-.42.18v4.94A7.23,7.23,0,0,1,8,13c.52.12.91.25,1.44.33a11.11,11.11,0,0,0,1.62.16,6.65,6.65,0,0,0,1.18,0,1.09,1.09,0,0,0,1-.59.66.66,0,0,0,.06-.2,1.63,1.63,0,0,1,.07-.3c.13-.28.37-.3.5-.68S13.74,11,13.79,10.84Z');
            const path4 = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            path4.setAttribute('class', 'icon-component');
            path4.setAttribute('fill', '#fff');
            path4.setAttribute('d', 'M5.49,7.59H4.31a.5.5,0,0,0-.5.5v4.56a.5.5,0,0,0,.5.5H5.49a.5.5,0,0,0,.5-.5V8.09A.5.5,0,0,0,5.49,7.59Z');
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
            return jsCode;
            
        }
    }
}
