

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
        public void CheckPawnDoubleMoveWhenObstructedByFriendlyPiece()
        {
            var board = new Board("rnbqkbnr/pppppppp/8/8/8/NNNNNNNN/PPPPPPPP/RNBQKBNR");
            var state = new GameState(board);

            var pawnMoves = state.GetLegalMoves(Color.White).Where(x => x.EndSquarePiece is Pawn).ToList();
            Assert.Empty(pawnMoves);
        }
        [Fact]
        public void CheckPawnDoubleMoveWhenObstructedByEnnemyPiece()
        {
            var board = new Board("rnbqkbnr/pppppppp/8/8/8/pppppppp/PPPPPPPP/RNBQKBNR");
            var state = new GameState(board);

            var pawnMoves = state.GetLegalMoves(Color.White).Where(x => x.EndSquarePiece is Pawn && x.EndSquare.Rank == 3);
            Assert.Empty(pawnMoves);
        }
        [Fact]
        public void CheckKingCastle()
        {
            var board = new Board("rnbqkbnr/p1pp1pp1/4p2p/8/1pBPP3/5N2/PPP2PPP/RNBQK2R");
            var state = new GameState(board);
            var whiteKing = state.Board.GetPiece(4, 0);
            var kingMoves = state.GetLegalMoves(Color.White).Where(x => x.StartSquare.Piece is King).ToList();
            Assert.Equal(4, kingMoves.Count());
            Assert.Contains(kingMoves, x => x.EndSquare.File == 6 && x.EndSquare.Rank == 0 && x.EndSquarePiece is King);
            var castleMove = kingMoves.First(x => x.EndSquare.File == 6 && x.EndSquare.Rank == 0 && x.EndSquarePiece is King);
            castleMove.MakeMove(state);
            var kingAfterCastling = state.Board.GetPiece(6, 0) as King;
            Assert.True(kingAfterCastling.HasMoved);
            var kingAfterCastlingMoves = kingAfterCastling.GetMoves(state);
            Assert.True(kingAfterCastlingMoves.Count == 1);
            var rook = state.Board.GetPiece(5, 0) as Rook;
            Assert.True(rook.HasMoved);
            var rookMoves = rook.GetMoves(state);
            Assert.True(rookMoves.Count == 1);
        }
        [Fact]
        public void CheckEnPassant()
        {
            var board = new Board("4k3/3pp3/8/8/8/8/4P3/4K3");
            var state = new GameState(board);
            state.MakeMove("e2e4", Color.White);
            state.MakeMove("e7e6", Color.Black);
            state.MakeMove("e4e5", Color.White);
            state.MakeMove("d7d5", Color.Black);
            var pawn = state.Board.GetPiece(4, 4);
            var pawnMoves = pawn.GetMoves(state);
            Assert.True(pawnMoves.Count == 1);
            state.MakeMove("e5d6", Color.White);
        }
        [Fact]
        public void CheckPromotion()
        {
            var board = new Board("k7/4P3/8/8/8/8/8/4K3");
            var state = new GameState(board);
            state.MakeMove("e7e8q", Color.White);
        }
        [Fact]
        public void CheckFailingGame()
        {
            var board = new Board("5bnr/3q2k1/p2pr3/P2N2pp/1QpPpp1P/2P4N/1P4P1/2BRK3");
            var state = new GameState(board);
            state.MakeMove("e4e3", Color.Black);
            var legalMoves = state.GetLegalMoves(Color.White);
            Assert.True(!legalMoves.Any(x => x.Notation is null));
            var queen = board.GetPiece(1, 3);
            var queenMoves = legalMoves.Where(x => x.StartSquare.Piece is Queen).ToList();
            var knightMoves = legalMoves.Where(x => x.StartSquare.Piece is Knight).ToList();
            Assert.Equal(10, queenMoves.Count);
            Assert.Equal(10, knightMoves.Count);
            legalMoves.ForEach(x =>
            {
                var tempBoard = board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
                var copyState = new GameState(tempBoard)
                {
                    MoveHistory = new(state.MoveHistory)
                };
                copyState.MakeMove(x.Notation, Color.White);

            });
        }
        [Fact]
        public void CheckFailingGame2()
        {
            var board = new Board("rnbqk1nr/pppp1ppp/8/4p3/1b3P2/P5P1/1PPPP2P/RNBQKBNR");
            var state = new GameState(board);
        }
        [Fact]
        public void CheckFailingGame3()
        {
            var board = new Board("6nr/n3k3/p6p/2b2pp1/1r1B2PP/1PR1P2K/6PR/2N2B2");
            var state = new GameState(board);
            state.MakeMove("h2h1",Color.White);
            var legalMoves = state.GetLegalMoves(Color.White);
            TestLegalMoves(legalMoves, state);
        }
        [Fact]
        public void CheckFailingGame4()
        {
            var board = new Board();
            var state = new GameState(board);
            state.MakeMove("e2e4", Color.White);
            state.MakeMove("e7e5", Color.Black);
            state.MakeMove("d1h5", Color.White);
            state.MakeMove("b8a6", Color.Black);
            state.MakeMove("h5e2", Color.White);
            state.MakeMove("f7f5", Color.Black);
            state.MakeMove("e2a6", Color.White);
            state.MakeMove("h7h5", Color.Black);
            state.MakeMove("a6f6", Color.White);
            state.MakeMove("a8b8", Color.Black);
            state.MakeMove("b1a3", Color.White);
            state.MakeMove("a7a5", Color.Black);
            state.MakeMove("f6e6", Color.White);
            state.MakeMove("d7e6", Color.Black);
            state.MakeMove("a3b1", Color.White);
            state.MakeMove("c7c6", Color.Black);
            state.MakeMove("d2d4", Color.White);
            state.MakeMove("f8b4", Color.Black);
            state.MakeMove("b1c3", Color.White);
            state.MakeMove("c6c5", Color.Black);
            state.MakeMove("e1e2",Color.White);
            state.MakeMove("c5c4", Color.Black);
            var legalMoves = state.GetLegalMoves(Color.White);
            TestLegalMoves(legalMoves, state);
        }
        private void TestLegalMoves(List<Move> moves,GameState initialState )
        {
            var board = initialState.Board;
            moves.ForEach(x =>
            {
                var tempBoard = board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
                var copyState = new GameState(tempBoard)
                {
                    MoveHistory = new(initialState.MoveHistory)
                };
                copyState.MakeMove(x.Notation, Color.White);
            });
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
