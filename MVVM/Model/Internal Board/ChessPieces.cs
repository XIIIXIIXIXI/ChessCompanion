using System;

namespace ChessCompanion.Core
{
    public enum PieceColor
    {
        White,
        Black
    }

    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
        Empty
    }

    public class ChessPiece
    {
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }

        public ChessPiece(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }

        public static readonly ChessPiece Empty = new ChessPiece(PieceType.Empty, PieceColor.White);

        public static char GetFenChar(ChessPiece piece)
        {
            return piece.Color switch
            {
                PieceColor.White => piece.Type switch
                {
                    PieceType.Pawn => 'P',
                    PieceType.Knight => 'N',
                    PieceType.Bishop => 'B',
                    PieceType.Rook => 'R',
                    PieceType.Queen => 'Q',
                    PieceType.King => 'K',
                    _ => throw new ArgumentException("Invalid piece type")
                },
                PieceColor.Black => piece.Type switch
                {
                    PieceType.Pawn => 'p',
                    PieceType.Knight => 'n',
                    PieceType.Bishop => 'b',
                    PieceType.Rook => 'r',
                    PieceType.Queen => 'q',
                    PieceType.King => 'k',
                    _ => throw new ArgumentException("Invalid piece type")
                },
                _ => throw new ArgumentException("Invalid piece color")
            };
        }

        public static ChessPiece TranslateStringToChessPiece(string pieceString)
        {
            var color = pieceString[0] switch
            {
                'w' => PieceColor.White,
                'b' => PieceColor.Black,
                _ => throw new ArgumentException("Invalid piece string: " + pieceString)
            };

            var type = pieceString[1] switch
            {
                'p' => PieceType.Pawn,
                'n' => PieceType.Knight,
                'b' => PieceType.Bishop,
                'r' => PieceType.Rook,
                'q' => PieceType.Queen,
                'k' => PieceType.King,
                _ => throw new ArgumentException("Invalid piece string: " + pieceString)
            };

            return new ChessPiece(type, color);
        }
    }
}
