using ChessEngine.Pieces;

namespace ChessEngine.BoardRepresentation
{
    public class Square
    {
        public ushort File { get; set; }
        public ushort Rank { get; set; }
        public Piece? Piece { get; set; }

        public Square(ushort file, ushort rank, Piece? piece = null)
        {
            File = file;
            Rank = rank;
            Piece = piece;
        }
    }
}
