using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ChessCompanion.MVVM.Utility
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

        /*public TopMove(string move, int? cp, int? mate, bool promotion, string pv)
        {
            this.move = move;
            this.cp = cp;
            this.promotion = promotion;
            this.mate = mate;
            this.pv = pv;
        }*/
        /*
        public void UpdateTopMove(string[] line, string move, bool promotion, int? cp, int? mate)
        {
            this.move = move;
            this.line = line;
            this.promotion = promotion;
            this.cp = cp;
            this.mate = mate;
        }
        */
    }
}
