using ChessEngine.BoardRepresentation;
using ChessEngine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Extensions
{
    public static class UtilityExtensions
    {
        public static bool SquareEquals(this Square square, Square other)
        {
            return square.File == other.File && square.Rank == other.Rank;
        }
    }
}
