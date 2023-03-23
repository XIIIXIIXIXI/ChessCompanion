using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ChessCompanion.MVVM.Model
{
    // ChessBoard class definition
    public class ChessBoard
    {
        // Private 2D array to represent the chess board
        private readonly ChessPiece[,] board;

        // Constructor to initialize the chess board
        public ChessBoard()
        {
            board = new ChessPiece[8, 8];
            InitializeBoard();
        }

        // Private method to set all squares of the chess board to empty
        private void InitializeBoard()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = ChessPiece.Empty;
                }
            }
        }

        // Indexer to access the chess board
        public ChessPiece this[int x, int y]
        {
            get => board[x, y];
            set => board[x, y] = value;
        }

        // Method to clear the chess board
        public void ClearBoard()
        {
            InitializeBoard();
        }

        // Method to modify the chess board based on the input chess piece elements
        public void ModifyBoard(IReadOnlyCollection<IWebElement> chessPieceElements)
        {
            var watch = Stopwatch.StartNew();

            
            ClearBoard();

            // Loop through the chess piece elements and update the corresponding squares on the chess board
            foreach (var chessPieceElement in chessPieceElements)
            {
                // Get the class attribute value of the chess piece element and split it into parts
                var classAttributeValue = chessPieceElement.GetAttribute("class");
                var classAttributeParts = classAttributeValue.Split(' ');

                // Ignore the chess piece element if it doesn't have enough class attribute parts
                if (classAttributeParts.Length < 3)
                {
                    continue;
                }

                // Get the position and type of the chess piece from the class attribute parts
                
                var position = classAttributeParts[2];
                try
                {
                    var positionNumbers = position.Substring(7);
                    var letter = int.Parse(positionNumbers[0].ToString()) - 1;
                    var number = int.Parse(positionNumbers[1].ToString()) - 1;
                    var type = classAttributeParts[1];
                        // Translate the chess piece type string to the corresponding ChessPiece enum and update the board
                    board[number, letter] = ChessPiece.TranslateStringToChessPiece(type);
                }
                catch
                {
                    var errorPosition = classAttributeParts[1];
                    var positionNumbers = errorPosition.Substring(7);
                    var letter = int.Parse(positionNumbers[0].ToString()) - 1;
                    var number = int.Parse(positionNumbers[1].ToString()) - 1;
                    var type = classAttributeParts[2];
                    // Translate the chess piece type string to the corresponding ChessPiece enum and update the board
                    board[number, letter] = ChessPiece.TranslateStringToChessPiece(type);

                }
                
                

                
            }

            watch.Stop();
            Debug.WriteLine($"ModifyBoard: {watch.ElapsedMilliseconds} ms");
        }

        
        public string GetFENString(char toMove)
        {
            var watch = Stopwatch.StartNew();

            var fen = new StringBuilder();
            var emptySquareCount = 0;

            // Loop through the rows and columns of the chess board to build the FEN string
            for (int row = board.GetLength(0) - 1; row >= 0; row--)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    var piece = board[row, col];

                    // If the square is empty, increment the empty square count
                    if (piece == ChessPiece.Empty)
                    {
                        emptySquareCount++;
                    }
                    // If the square is not empty, append the piece FEN character to the FEN string and reset the empty square count
                    else
                    {
                        if (emptySquareCount > 0)
                        {
                            fen.Append(emptySquareCount);
                            emptySquareCount = 0;
                        }

                        fen.Append(ChessPiece.GetFenChar(piece));
                    }
                }

                // If there are any empty squares left in the row, append their count to the FEN string.
                if (emptySquareCount > 0)
                {
                    fen.Append(emptySquareCount);
                    emptySquareCount = 0;
                }

                if (row != 0)
                {
                    fen.Append('/');
                }
            }

            fen.Append(' ');
            fen.Append(toMove);

            watch.Stop();
            Debug.WriteLine($"FEN: {watch.ElapsedMilliseconds} ms");

            return fen.ToString();
        }
    }
}
