using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class King : Piece
    {
        public bool HasMoved { get; set; }
        private List<Move> _moves;
        public King(Color color, Square currentLocation) : base(PieceValue.King, color, currentLocation)
        {
        }

        public override List<Move> GetMoves(GameState state)
        {
            _moves = new List<Move>();
            _moves.AddRange(this.GetDiagonalMoves(state.Board,true));
            _moves.AddRange(this.GetVerticalHorizontalMoves(state.Board,true));
            AddCastles(state.Board);
            return _moves;
        }
        private void AddCastles(Board board)
        {
            if (CanCastleKingSide(board))
            {
                var squares = board.Squares;
                var targetSquare = squares[CurrentLocation.File + 2, CurrentLocation.Rank];
                _moves.Add(new Move(CurrentLocation, targetSquare, null, false, true));
            }
            if (CanCastleQueenSide(board))
            {
                var squares = board.Squares;
                var targetSquare = squares[CurrentLocation.File - 2, CurrentLocation.Rank];
                _moves.Add(new Move(CurrentLocation, targetSquare, null, false, true));
            }
        }
        private bool CanCastleQueenSide(Board board)
        {
            if (HasMoved || !KingIsInStartingSquare(board)) return false;
            var squares = board.Squares;
            var quuenSideRookSquare = Color == Color.White ? squares[0, 0] : squares[0, 7];
            if(quuenSideRookSquare.Piece is null) return false;
            var queenSideRook = Color == Color.White ? squares[0,0]?.Piece : squares[0,7].Piece;
            if(queenSideRook == null || queenSideRook is not Rook) return false;
            if (queenSideRook is Rook queenSideColoredRook && queenSideColoredRook.HasMoved) return false;
            for(int i = 1; i <= 3; i++)
            {
                if (squares[i, CurrentLocation.Rank].Piece != null)
                {
                    return false;
                }
            }
            return true;
        }
        private bool KingIsInStartingSquare(Board board)
        {
            var startingKingSquare = Color == Color.White ? board.Squares[4,0] : board.Squares[4,7];
            return CurrentLocation.SquareEquals(startingKingSquare);
        }
        private bool CanCastleKingSide(Board board)
        {
            if (HasMoved || !KingIsInStartingSquare(board)) return false;
            var squares = board.Squares;
            var kingSideRookSquare = Color == Color.White ? squares[7, 0] : squares[7, 7];
            if(kingSideRookSquare.Piece is null) return false;
            var kingSideRook = Color == Color.White ? squares[7, 0]?.Piece : squares[7, 7].Piece;
            if (kingSideRook == null || kingSideRook is not Rook) return false;
            if (kingSideRook is Rook queenSideColoredRook && queenSideColoredRook.HasMoved) return false;
            for (int i = 5; i < 7; i++)
            {
                if (squares[i, CurrentLocation.Rank].Piece != null)
                {
                    return false;
                }
            }
            return true;
        }
        public override string ToString()
        {
            return Color == Color.White ? "K" : "k";
        }
    }
}
