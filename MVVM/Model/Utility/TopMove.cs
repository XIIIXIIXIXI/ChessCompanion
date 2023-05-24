using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ChessCompanion.MVVM.Model.Utility
{
    public class TopMove
    {

        public string bestMove;
        public int? cp;
        public int? mate;
        public bool promotion;
        public string pv;
        public string FEN { get; set; }

        public TopMove()
        {
            bestMove = string.Empty;
            cp = null;
            mate = null;
            promotion = false;
            pv = string.Empty;
            FEN = string.Empty;
        }

        public void setTopMove(string bestMove, int? cp, int? mate, bool promotion, string pv)
        {
            this.bestMove = bestMove;
            this.cp = cp;
            this.mate = mate;
            this.promotion = promotion;
            this.pv = pv;
        }
    }
}
