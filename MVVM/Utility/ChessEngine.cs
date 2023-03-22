using Stockfish.NET;
using Stockfish.NET.Models;
using System;


namespace ChessCompanion.MVVM.Utility
{
    public class ChessEngine
    {
        public IStockfish stockfish;
        public ChessEngine()
        {
            this.stockfish = new Stockfish.NET.Stockfish(@"C:\Users\marti\source\repos\chessEval\chessEval\stockfish_20090216_x64_avx2");
        }
        public void testFEN(string FEN)
        {
            setFENPosition(FEN);
            var bestMove = stockfish.GetBestMove();
            Console.WriteLine(bestMove);


            var evaluation = stockfish.GetEvaluation();
            Console.WriteLine("Evaluation Type: " + evaluation.Type);
            Console.WriteLine("Evaluation Value: " + evaluation.Value);

        }
        public void testMove(string[] moves)
        {
            stockfish.SetPosition(moves[0], moves[1]);
            var bestMove = stockfish.GetBestMove();
            Console.WriteLine(bestMove);
            var evaluation = stockfish.GetEvaluation();
            Console.WriteLine("Evaluation Type: " + evaluation.Type);
            Console.WriteLine("Evaluation Value: " + evaluation.Value);

        }
        public void setFENPosition(string fenPosition)
        {
            stockfish.SetFenPosition(fenPosition);

        }
        public void setNextMoves(string[] moves)
        {
            stockfish.SetPosition(moves[0], moves[1]);
        }
        public string getBestMove()
        {
            try
            {
                string bestMove = stockfish.GetBestMove();
                return bestMove;
            }
            catch
            {
                return "Couldn't find the best move";
            }

        }
        public Evaluation getEvaluation()
        {
            var evaluation = stockfish.GetEvaluation();
            return evaluation;
        }

    }

}
