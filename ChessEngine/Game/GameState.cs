using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Pieces;

namespace ChessEngine.Game
{
    public sealed class GameState
    {

        public Board Board { get; init; }
        public List<Move> MoveHistory { get; set; } = new();
        public bool IsCheck { get; set; }
        public bool IsMate { get; set; }

        public GameState(Board board)
        {
            Board = board;
        }
        public bool VerifyIsCheckForColor(Color checkedColor)
        {
            var opposingColor = Color.White == checkedColor ? Color.Black : Color.White;
            var movesForOppositeColor = Board.GetPseudoLegalMoves(opposingColor, this);
            var kingOfCheckedColor = Board.GetPiecesFromBoard(this).First(p => p is King && p.Color == checkedColor);
            var movesThatResultInKingCapture = movesForOppositeColor.Where(x => x.EndSquare.SquareEquals(kingOfCheckedColor.CurrentLocation)).ToList();
            return movesThatResultInKingCapture.Any();
        }
        public List<Move> GetLegalMoves(Color color)
        {
            var board = Board;
            var legalMoves = new List<Move>();
            var pieces = board.GetPiecesFromBoard(this).Where(x => x.Color == color).ToList();
            var initialGameState = new GameState(board);
            pieces.ForEach(piece =>
            {
               
                var pieceMoves = piece.GetMoves(initialGameState);
                pieceMoves.ForEach(move =>
                {
                    var tempBoard = board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
                    var tempGameState = new GameState(tempBoard);
                    move.MakeMove(tempGameState);
                    if (!tempGameState.VerifyIsCheckForColor(color))
                    {
                        legalMoves.Add(move);
                    }
                });
            });
            return legalMoves;
        }
    }
}
