using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Enums
{
    public enum PieceValue
    {
        None = 0,
        Pawn = 1,
        Bishop = 2,
        Knight = 3,
        Rook = 5,
        Queen = 9,
        King = int.MaxValue
    }
}
