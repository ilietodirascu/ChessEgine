using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, Square currentLocation) : base(PieceValue.Queen, color, currentLocation)
        {
        }

        public override List<Move> GetMoves(GameState state)
        {
            var moves = new List<Move>();
            moves.AddRange(this.GetDiagonalMoves(state.Board));
            moves.AddRange(this.GetVerticalHorizontalMoves(state.Board));
            return moves;
        }
        public override string ToString()
        {
            return Color == Color.White ? "Q" : "q";
        }
    }
}
