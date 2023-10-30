

namespace ChessTests
{
    public class MoveTests
    {
        [Fact]
        public void CheckLegalStartingMoves()
        {
            var board = new Board();
            var state = new GameState(board);
            var legalMovesForWhite = state.GetLegalMoves(Color.White);
            Assert.True(legalMovesForWhite.Count == 20);
        }
        [Fact]
        public void CheckLegalMovesKingMustCapture()
        {
            var board = new Board("3k4/3Q4/8/8/8/8/8/3K4");
            var state = new GameState(board);
            var legalMovesForBlack = state.GetLegalMoves(Color.Black);
            Assert.True(legalMovesForBlack.Count == 1);
            var theOnlyMove = legalMovesForBlack.First();
            Assert.True(theOnlyMove.EndSquare.File == 3 && theOnlyMove.EndSquare.Rank == 6);
        }
        [Fact]
        public void IsMate()
        {
            var board = new Board("3k4/2RQ4/8/8/8/8/8/3K4");
            var state = new GameState(board);
            var legalMovesForBlack = state.GetLegalMoves(Color.Black);
            Assert.Empty(legalMovesForBlack);
        }
        [Fact]
        public void CheckQueenMoves()
        {
            var board = new Board("3k4/8/8/3Q4/8/8/8/3K4");
            var state = new GameState(board);
            var legalMovesForWhite = state.GetLegalMoves(Color.White);
            var legalMovesForKing = legalMovesForWhite.Where(x => x.EndSquarePiece is King).Count();
            var legalMovesForQueen = legalMovesForWhite.Where(x => x.EndSquarePiece is Queen).Count();
            Assert.Equal(5, legalMovesForKing);
            Assert.Equal(26, legalMovesForQueen);
        }
        [Fact]
        public void CheckLegalMovesBishopMustProtect()
        {
            var board = new Board("2bk4/8/8/8/8/8/3QR3/3K4");
            var state = new GameState(board);
            var legalMovesForBlack = state.GetLegalMoves(Color.Black);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 2 && x.EndSquare.Rank == 6 && x.EndSquarePiece is King);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 3 && x.EndSquare.Rank == 6 && x.EndSquarePiece is Bishop);
            Assert.True(legalMovesForBlack.Count == 2);
        }
        [Fact]
        public void CheckLegalMovesKingMustFlee()
        {
            var board = new Board("3k4/8/8/3Q4/8/8/8/3K4");
            var state = new GameState(board);
            var legalMovesForBlack = state.GetLegalMoves(Color.Black);
            Assert.True(legalMovesForBlack.Count == 4);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 2 && x.EndSquare.Rank == 7);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 4 && x.EndSquare.Rank == 7);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 2 && x.EndSquare.Rank == 6);
            Assert.Contains(legalMovesForBlack, x => x.EndSquare.File == 4 && x.EndSquare.Rank == 6);
        }
        [Fact]
        public void GetStartingMoves()
        {
            var board = new Board();
            var state = new GameState(board);
            var rookMoves = new List<Move>();
            var pawnMoves = new List<Move>();
            var bishopMoves = new List<Move>();
            var knightMoves = new List<Move>();
            var kingMoves = new List<Move>();
            var queenMoves = new List<Move>();
            var gameState = new GameState(board);
            var pieces = board.GetPiecesFromBoard(state);
            var rooks = pieces.Where(x => x is Rook).ToList();
            var pawns = pieces.Where(x => x is Pawn).ToList();
            var knights = pieces.Where(x => x is Knight).ToList();
            var bishops = pieces.Where(x => x is Bishop).ToList();
            var king = pieces.First(x => x is King);
            var queen = pieces.First(x => x is Queen);
            rooks.ForEach(x => rookMoves.AddRange(x.GetMoves(gameState)));
            pawns.ForEach(x => pawnMoves.AddRange(x.GetMoves(gameState)));
            knights.ForEach(x => knightMoves.AddRange(x.GetMoves(gameState)));
            bishops.ForEach(x => bishopMoves.AddRange(x.GetMoves(gameState)));
            kingMoves.AddRange(king.GetMoves(gameState));
            queenMoves.AddRange(queen.GetMoves(gameState));
            Assert.Equal(32, pawnMoves.Count);
            Assert.Equal(8, knightMoves.Count);
            Assert.Empty(bishopMoves);
            Assert.Empty(kingMoves);
            Assert.Empty(queenMoves);
            Assert.Empty(rookMoves);
            Assert.True(pawnMoves.All(x => x.StartSquare.Piece != null));
        }
    }
}
