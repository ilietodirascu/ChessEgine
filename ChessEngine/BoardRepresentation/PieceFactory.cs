using ChessEngine.Enums;
using ChessEngine.Pieces;

namespace ChessEngine.BoardRepresentation
{
    public static class PieceFactory
    {
        public static Piece? CreatePiece(PieceValue pieceValue,Color color, Square square)
        {
            return pieceValue switch
            {
                PieceValue.Pawn => new Pawn(color, square),
                PieceValue.Rook => new Rook(color, square),
                PieceValue.Knight => new Knight(color, square),
                PieceValue.Bishop => new Bishop(color, square),
                PieceValue.Queen => new Queen(color, square),
                PieceValue.King => new King(color, square),
                PieceValue.None => null,
                _ => throw new Exception("invalid piece")
            };
        }
    }
}
