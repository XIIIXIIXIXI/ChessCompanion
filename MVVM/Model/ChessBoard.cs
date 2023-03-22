using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.Model
{
    public class ChessBoard
    {
        private readonly ChessPiece[,] board = new ChessPiece[8, 8];

        public ChessBoard()
        {
            // Initialize the board with empty squares
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    board[row, col] = ChessPiece.Empty;
                }
            }
        }

        public ChessPiece this[int x, int y]
        {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }
        public void InitializeBoard()
        {
            // Set up the white pieces
            this[0, 0] = new ChessPiece(PieceType.Rook, PieceColor.White);
            this[0, 1] = new ChessPiece(PieceType.Knight, PieceColor.White);
            this[0, 2] = new ChessPiece(PieceType.Bishop, PieceColor.White);
            this[0, 3] = new ChessPiece(PieceType.Queen, PieceColor.White);
            this[0, 4] = new ChessPiece(PieceType.King, PieceColor.White);
            this[0, 5] = new ChessPiece(PieceType.Bishop, PieceColor.White);
            this[0, 6] = new ChessPiece(PieceType.Knight, PieceColor.White);
            this[0, 7] = new ChessPiece(PieceType.Rook, PieceColor.White);


            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new ChessPiece(PieceType.Pawn, PieceColor.White);
            }

            // Set up the black pieces
            this[7, 0] = new ChessPiece(PieceType.Rook, PieceColor.Black);
            this[7, 1] = new ChessPiece(PieceType.Knight, PieceColor.Black);
            this[7, 2] = new ChessPiece(PieceType.Bishop, PieceColor.Black);
            this[7, 3] = new ChessPiece(PieceType.Queen, PieceColor.Black);
            this[7, 4] = new ChessPiece(PieceType.King, PieceColor.Black);
            this[7, 5] = new ChessPiece(PieceType.Bishop, PieceColor.Black);
            this[7, 6] = new ChessPiece(PieceType.Knight, PieceColor.Black);
            this[7, 7] = new ChessPiece(PieceType.Rook, PieceColor.Black);

            for (int i = 0; i < 8; i++)
            {
                this[6, i] = new ChessPiece(PieceType.Pawn, PieceColor.Black);
            }

            // Fill the rest of the board with empty squares
            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    this[row, col] = ChessPiece.Empty;
                }
            }
        }
        public void UpdateBoard(int startX, int startY, int endX, int endY)
        {
            ChessPiece piece = board[startX, startY];
            board[startX, startY] = ChessPiece.Empty;
            board[endX, endY] = piece;
        }

        public void EmptyBoard()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    board[row, col] = ChessPiece.Empty;
                }
            }
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("EmptyBoard: " + elapsedMs);
        }
        public void ModifyBoard(IReadOnlyCollection<IWebElement> chessPieceElements)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            EmptyBoard();
            foreach (var chessPieceElement in chessPieceElements)
            {
                 try
                 {
                    

                    var classAttributeValue = chessPieceElement.GetAttribute("class");
                    var classAttributeParts = classAttributeValue.Split(' ');

                    var position = classAttributeParts[2]; // Get the position class (e.g. "square-88")
                    var positionNumbers = position.Substring(7); // Get the position numbers (e.g. "88")
                    var letter = int.Parse(positionNumbers[0].ToString()) - 1; // Get the first digit as x (e.g. 8)
                    var number = int.Parse(positionNumbers[1].ToString()) - 1; // Get the second digit as y (e.g. 8)
                    var type = classAttributeParts[1]; // Get the type class (e.g. "bp")



                    board[number, letter] = ChessPiece.TranslateStringToChessPiece(type);
                
                 }
                 catch
                 {
                     continue;
                 }
            }
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("ModifyBoard: " + elapsedMs);
        }

        public string GetFEN(char toMove)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            StringBuilder fen = new StringBuilder();
            int emptySquareCount = 0;
            for (int row = 7; row >= 0; row--)
            {
                for (int col = 0; col < 8; col++)
                {
                    ChessPiece piece = board[row, col];

                    //if square is empty
                    if (piece == ChessPiece.Empty)
                    {
                        emptySquareCount++;
                    }
                    else
                    {
                        //if there was empty squares before this piece
                        if (emptySquareCount > 0)
                        {
                            fen.Append(emptySquareCount);
                            emptySquareCount = 0;
                        }
                        fen.Append(ChessPiece.GetFenChar(piece));
                    }
                }
                // If there were empty squares at the end of the column
                if (emptySquareCount > 0)
                {
                    fen.Append(emptySquareCount);
                    emptySquareCount = 0;
                }

                // If it's not the last row, append the separator
                if (row != 0)
                {
                    fen.Append('/');
                }
            }
            fen.Append(' ');
            fen.Append(toMove);
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.WriteLine("FEN: " + elapsedMs);
            return fen.ToString();
        }

    }
}
