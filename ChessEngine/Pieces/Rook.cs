using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class Rook : Piece
    {
        public bool HasMoved { get; set; }
        public Rook(Color color, Square currentLocation) : base(PieceValue.Rook, color, currentLocation)
        {
        }
        public override List<Move> GetMoves(GameState state)
        {
            return this.GetVerticalHorizontalMoves(state.Board);
        }
        public override string ToString()
        {
            return Color == Color.White ? "R" : "r";
        }
    }
}
