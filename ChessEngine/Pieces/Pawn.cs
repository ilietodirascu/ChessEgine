using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;

namespace ChessEngine.Pieces
{
    public class Pawn : Piece
    {
        public bool CanPromote { get; set; }
        private List<Move> _moves;
        public Pawn(Color color, Square currentLocation) : base(PieceValue.Pawn, color, currentLocation)
        {
        }

        public override List<Move> GetMoves(GameState state)
        {
            _moves = new();
            var board = state.Board;
            var lastMove = state.MoveHistory.LastOrDefault();
            AddAdvancements(board);
            AddCaptures(board);
            AddEnPassant(board,lastMove);
            return _moves;
        }
        public void AddPromotions(Square nextSquare)
        {
            var knight = new Knight(Color, nextSquare);
            var rook = new Rook(Color, nextSquare);
            var queen = new Queen(Color, nextSquare);
            var bishop = new Bishop(Color, nextSquare);
            _moves.Add(new Move(CurrentLocation, nextSquare, knight,false,false,true));
            _moves.Add(new Move(CurrentLocation,nextSquare, rook, false, false, true));
            _moves.Add(new Move(CurrentLocation, nextSquare, queen, false, false, true));
            _moves.Add(new Move(CurrentLocation,nextSquare,bishop, false, false, true));
        }
        public void AddCaptures(Board board)
        {
            int direction = Color == Color.White ? 1 : -1;
            var nextRank = CurrentLocation.Rank + direction;
            var fileCandidates = new int[] { CurrentLocation.File - 1, CurrentLocation.File + 1 };
            foreach(int file in fileCandidates)
            {
                if (file < 0 || file > 7) continue;
                var sidewaysSquare = board.Squares[file,nextRank];
                var sidewaysPiece = sidewaysSquare.Piece;
                var canCapture = sidewaysPiece != null && sidewaysPiece.Color != Color;
                var isLastRank = (nextRank == 7 && Color == Color.White) || (nextRank == 0 && Color == Color.Black);
                if (canCapture && isLastRank)
                {
                    AddPromotions(sidewaysSquare);
                }
                else if (canCapture)
                {
                    var captureMove = new Move(CurrentLocation, sidewaysSquare);
                    _moves.Add(captureMove);
                }
            }

        }
        public void AddAdvancements(Board board)
        {
            int direction = Color == Color.White ? 1 : -1;
            int nextRank = CurrentLocation.Rank + direction;
            var squares = board.Squares;
            if (nextRank < 0 || nextRank > 7 || squares[CurrentLocation.File,nextRank].Piece != null)
            {
                return; // Outside the valid rank range, no advancements possible. Or piece in front of it.
            }

            Square nextSquare = board.Squares[CurrentLocation.File, nextRank];

            if ((nextRank == 7 && Color == Color.White) || (nextRank == 0 && Color == Color.Black))
            {
                AddPromotions(nextSquare);
            }
            else
            {
                _moves.Add(new Move(CurrentLocation, nextSquare));
                if((Color == Color.White && CurrentLocation.Rank == 1) || (Color == Color.Black && CurrentLocation.Rank == 6))
                {
                    var nextNextSquare = board.Squares[CurrentLocation.File, nextRank + direction];
                    _moves.Add(new Move(CurrentLocation, nextNextSquare));
                }
            }
        }
        private bool CanEnpassant(Move? lastMove)
        {
            if (lastMove == null) return false;
            var lastMovePiece = lastMove.EndSquarePiece;
            var isPawn = lastMovePiece is Pawn;
            var pawnMovedTwoSquares = Math.Abs(lastMove.EndSquare.Rank - lastMove.StartSquare.Rank) == 2;
            var fileAdjacency = new int[] { CurrentLocation.File + 1, CurrentLocation.File - 1 };
            var pawnIsAdjacent = fileAdjacency.Contains(lastMove.EndSquare.File) && lastMove.EndSquare.Rank == CurrentLocation.Rank;
            var pawnIsOppositeColor = lastMovePiece.Color != Color; //you never know
            if (isPawn && pawnMovedTwoSquares && pawnMovedTwoSquares && pawnIsAdjacent && pawnIsOppositeColor) return true;
            return false;
        }
        public void AddEnPassant(Board board, Move? lastMove)
        {
            int direction = Color == Color.White ? 1 : -1;
            var nextRank = CurrentLocation.Rank + direction;
            if (CanEnpassant(lastMove))
            {
                var squares = board.Squares;
                var enPassant = new Move(CurrentLocation, squares[lastMove!.EndSquare.File, nextRank],null,true);
                _moves.Add(enPassant);
            }
        }
        public override string ToString()
        {
            return Color == Color.White ? "P" : "p";
        }
    }
}
