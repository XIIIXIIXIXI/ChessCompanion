using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.Model
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
        Empty,


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

        public static ChessPiece Empty = new ChessPiece(PieceType.Empty, PieceColor.White);

        public static char GetFenChar(ChessPiece piece)
        {
            if (piece.Color == PieceColor.White)
            {
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        return 'P';
                    case PieceType.Knight:
                        return 'N';
                    case PieceType.Bishop:
                        return 'B';
                    case PieceType.Rook:
                        return 'R';
                    case PieceType.Queen:
                        return 'Q';
                    case PieceType.King:
                        return 'K';
                    default:
                        throw new ArgumentException("Invalid piece type");
                }
            }
            else if (piece.Color == PieceColor.Black)
            {
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        return 'p';
                    case PieceType.Rook:
                        return 'r';
                    case PieceType.Knight:
                        return 'n';
                    case PieceType.Bishop:
                        return 'b';
                    case PieceType.Queen:
                        return 'q';
                    case PieceType.King:
                        return 'k';
                    default:
                        throw new ArgumentException("Invalid piece type");
                }
            }
            else
            {
                throw new ArgumentException("Invalid piece color");
            }
        }
        public static ChessPiece TranslateStringToChessPiece(string pieceString)
        {
            PieceType type;
            PieceColor color;

            // Determine the piece type
            switch (pieceString[1])
            {
                case 'p':
                    type = PieceType.Pawn;
                    break;
                case 'n':
                    type = PieceType.Knight;
                    break;
                case 'b':
                    type = PieceType.Bishop;
                    break;
                case 'r':
                    type = PieceType.Rook;
                    break;
                case 'q':
                    type = PieceType.Queen;
                    break;
                case 'k':
                    type = PieceType.King;
                    break;
                default:
                    throw new ArgumentException("Invalid piece string: " + pieceString);
            }

            // Determine the piece color
            switch (pieceString[0])
            {
                case 'w':
                    color = PieceColor.White;
                    break;
                case 'b':
                    color = PieceColor.Black;
                    break;
                default:
                    throw new ArgumentException("Invalid piece string: " + pieceString);
            }

            return new ChessPiece(type, color);
        }
    }
}
