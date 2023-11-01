using ChessEngine.BoardRepresentation;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI
{
    public static class Evaluator
    {
        public static void AdjustValueBasedOnKingDistance(this Move move, GameState state)
        {
            var color = move.EndSquarePiece.Color;
            var board = state.Board;
            var opposingColor = color.GetOpposingColor();
            var opposingKing = board.GetPiecesOfColor(state,opposingColor).First(x => x is King);
            var distance = UtilityExtensions
                .CalculateEuclideanDistance(move.EndSquare.File, move.EndSquare.Rank, opposingKing.CurrentLocation.File, opposingKing.CurrentLocation.Rank);
            move.Value -= distance;
        }

    }
}
