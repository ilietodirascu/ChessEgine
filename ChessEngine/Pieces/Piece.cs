using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;

namespace ChessEngine.Pieces
{
    public abstract class Piece 
    {
        public PieceValue Value { get; set; }
        public Color Color { get; set; }
        public abstract List<Move> GetMoves(GameState state);
        public Square CurrentLocation { get; set; }
        protected Piece(PieceValue value, Color color, Square currentLocation)
        {
            Value = value;
            Color = color;
            CurrentLocation = currentLocation;
        }
    }
}
