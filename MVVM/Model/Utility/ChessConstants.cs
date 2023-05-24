using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCompanion.MVVM.Model.Utility
{
    public static class ChessConstants
    {
        public static readonly Dictionary<char, char> FileLookup = new Dictionary<char, char>()
    {
        {'a', '1'}, {'b', '2'}, {'c', '3'}, {'d', '4'}, {'e', '5'}, {'f', '6'}, {'g', '7'}, {'h', '8'}
    };

    }
}
