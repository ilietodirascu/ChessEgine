using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Pieces;
using ChessEngine.Utility;

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
        public void MakeMove(string notation,Color color)
        {
            var legalMoves = GetLegalMoves(color);
            var requestedMove = legalMoves.Where(x => x.Notation ==  notation).SingleOrDefault() ?? throw new Exception("No Such Move");
            requestedMove.MakeMove(this);
        }
        public bool VerifyIsCheckForColor(Color checkedColor)
        {
            var opposingColor = Color.White == checkedColor ? Color.Black : Color.White;
            var movesForOppositeColor = Board.GetPseudoLegalMoves(opposingColor, this);
            var kingOfCheckedColor = Board.GetPiecesFromBoard(this).First(p => p is King && p.Color == checkedColor);
            var movesThatResultInKingCapture = movesForOppositeColor.Where(x => x.EndSquare.SquareEquals(kingOfCheckedColor.CurrentLocation)).ToList();
            return movesThatResultInKingCapture.Any();
        }
        public void MakeMoveByNotation(string movesString, Color color)
        {
            var moves = movesString.Split(' ');
            var moveNotation = moves[^1];
            if (moveNotation.Length < 4) throw new Exception("Invalid Move Notation");
            var legalMoves = GetLegalMoves(color);
            var moveToMake = legalMoves.FirstOrDefault(x => x.Notation == moveNotation)
                ?? throw new Exception("Move Not found");
            moveToMake.MakeMove(this);
        }
       
        public List<Move> GetLegalMoves(Color color)
        {
            var board = Board;
            var legalMoves = new List<Move>();
            var pieces = board.GetPiecesFromBoard(this).Where(x => x.Color == color).ToList();
            var initialGameState = new GameState(board)
            {
                MoveHistory = new List<Move>(MoveHistory)
            };
            pieces.ForEach(piece =>
            {
               
                var pieceMoves = piece.GetMoves(initialGameState);
                pieceMoves.ForEach(move =>
                {
                    var tempBoard = board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
                    var tempGameState = new GameState(tempBoard)
                    {
                        MoveHistory = new(MoveHistory)
                    };
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
