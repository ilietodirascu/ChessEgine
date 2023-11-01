using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
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
        public static Color GetOpposingColor(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
        public static double CalculateEuclideanDistance(int startFile, int startRank, int endFile, int endRank)
        {
            int deltaX = endFile - startFile;
            int deltaY = endRank - startRank;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
