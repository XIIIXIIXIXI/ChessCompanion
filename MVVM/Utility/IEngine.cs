using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.Utility
{
    public interface IEngine : IDisposable
    {
        (string bestMove, int? cp, int? mate, bool promotion, string pv) GetBestMoveWithInfo(int searchTimeMs);
        void SetOption(string name, object value);
        void SetPosition(string fen, params string[] moves);
        string GetBestMove(int milliseconds);

        public MoveScore AnalyzeLastMove(TopMove lastBestMove, TopMove currentMove);
    }
}
