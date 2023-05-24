using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessCompanion.MVVM.Model.Utility;

namespace ChessCompanion.MVVM.Model
{
    public interface IEngine : IDisposable
    {
        (string bestMove, int? cp, int? mate, bool promotion, string pv) GetBestMoveWithInfo(int searchTimeMs);
        void SetOption(string name, object value);
        void SetPosition(string fen, params string[] moves);
        string GetBestMove(int milliseconds);

        public MoveScore AnalyzeLastMove(TopMove lastBestMove, TopMove currentMove);

        public void setLines(int lines);

        public TopMove[] GetMultipleLines(int searchTimeMs);

        public void OrderTopMoves(TopMove[] topMoves);
    }
}
