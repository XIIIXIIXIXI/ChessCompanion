using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.Core
{
    public class MainState
    {
        public string Moves { get; set; } = "";
        public string FEN { get; set; } = "";
        public string CP { get; set; } = "";

        public bool IsWhite { get; set; }
    }
}
