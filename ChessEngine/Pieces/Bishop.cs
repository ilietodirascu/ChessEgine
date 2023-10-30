using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color, Square currentLocation) : base(PieceValue.Bishop, color, currentLocation)
        {
        }

        public override List<Move> GetMoves(GameState state)
        {
            return this.GetDiagonalMoves(state.Board);
        }
        public override string ToString()
        {
            return Color == Color.White ? "B" : "b";
        }
    }
}
