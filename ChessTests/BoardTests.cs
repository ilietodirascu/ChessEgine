
using ChessEngine.Utility;

namespace ChessTests
{
    public class BoardTests
    {
        [Fact]
        public void BoardInitialization()
        {
            var board = new Board();

            TestPawnInitialization(board, Color.White, 1);
            TestPawnInitialization(board, Color.Black, 6);
            TestBackRow(board, Color.White);
            TestBackRow(board, Color.Black);
        }

        [Fact]
        public void TestGetFenFromBoard()
        {
            var board = new Board();
            var fen = board.GetFenOfBoardState();
            Assert.Equal(FenNotation.StartPosFen, fen);
            var state = new GameState(board);
            var pawn = board.GetPiecesFromBoard(state).First(x => x is Pawn && x.Color == Color.White && x.CurrentLocation.File == 4);
            var gameState = new GameState(board);
            pawn.GetMoves(gameState).First(x => x.EndSquare.File == 4 && x.EndSquare.Rank == 3).MakeMove(gameState);
            var updatedFen = board.GetFenOfBoardState();
            var fenForE4 = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR";
            Assert.Equal(fenForE4, updatedFen);
        }
        [Fact]
        public void TestGenBoardFromFen1()
        {
            var fen = "3k4/3Q4/8/8/8/8/8/3K4";
            var board = new Board(fen);
            var squares = board.Squares;
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Assert.True(squares[i, j] != null);
                }
            }
        }
        private void TestPawnInitialization(Board board, Color color, int rank)
        {
            for (int file = 0; file < 8; file++)
            {
                var pawn = board.Squares[file, rank].Piece;
                Assert.NotNull(pawn);
                Assert.True(pawn.Value == PieceValue.Pawn && pawn.Color == color);
            }
        }
        private void TestBackRow(Board board, Color color)
        {
            var rank = color == Color.White ? 0 : 7;
            var backRowNotation = "RNBQKBNR";
            var expected = color == Color.White ? backRowNotation : backRowNotation.ToLower();
            var actual = "";
            for (int file = 0; file < 8; file++)
            {
                var piece = board.Squares[file, rank].Piece;
                Assert.NotNull(piece);
                Assert.Equal(color, piece.Color);
                actual += piece.ToString();
            }
            Assert.Equal(expected, actual);
        }
    }
}