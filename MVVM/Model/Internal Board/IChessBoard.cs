using OpenQA.Selenium;
using System.Collections.Generic;

namespace ChessCompanion.Core
{
    public interface IChessBoard
    {
        ChessPiece this[int x, int y] { get; set; }

        void ClearBoard();
        string GetFENFromMove(string move, char toMove);
        string GetFENString(char toMove);
        void ModifyBoard(IReadOnlyCollection<IWebElement> chessPieceElements);
        string TranslateMoveToSquare(string move, bool isWhite);
    }
}