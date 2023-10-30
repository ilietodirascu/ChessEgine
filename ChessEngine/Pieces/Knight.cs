using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color, Square currentLocation) : base(PieceValue.Knight, color, currentLocation)
        {
        }

        public override List<Move> GetMoves(GameState state)
        {
            return this.GetKnightMoves(state.Board);
        }
        public override string ToString()
        {
            return Color == Color.White ? "N" : "n";
        }
    }
}
