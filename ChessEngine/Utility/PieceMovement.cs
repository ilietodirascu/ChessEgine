using ChessEngine.BoardRepresentation;
using ChessEngine.Game;
using ChessEngine.Pieces;

namespace ChessEngine.Utility
{
    public static class PieceMovement
    {
        private static readonly (int[], int[]) _diagonalDeltas = (new int[] { -1, -1, 1, 1 }, new int[] { -1, 1, 1, -1 });
        private static readonly (int[], int[]) _horizontalVerticalDeltas = (new int[] { 0, -1, 0, 1 }, new int[] { -1, 0, 1, 0 });
        private static List<Move> GetMoves(Piece piece, Board board, (int[], int[]) deltas, bool isForKing = false)
        {
            var moves = new List<Move>();
            var currentLocation = piece.CurrentLocation;
            var currentRank = currentLocation.Rank;
            var currentFile = currentLocation.File;
            var deltaFile = deltas.Item1;//bottom left to bottom right
            var deltaRank = deltas.Item2;
            var nrOfTimes = isForKing ? 1 : 7;
            for (int i = 0; i < deltaFile.Length; i++)
            {
                for (int j = 1; j <= nrOfTimes; j++)
                {
                    int targetFile = currentFile + j * deltaFile[i];
                    int targetRank = currentRank + j * deltaRank[i];
                    if (!IsOnBoard(targetFile, targetRank)) continue;
                    Square targetSquare = board.Squares[targetFile, targetRank];

                    if(targetSquare.Piece != null && targetSquare.Piece.Color == piece.Color)
                    {
                        break;
                    }
                    if (targetSquare.Piece == null)
                    {
                        moves.Add(new Move(currentLocation, targetSquare));
                    }
                    else if (targetSquare.Piece.Color != currentLocation.Piece!.Color)
                    {
                        moves.Add(new Move(currentLocation, targetSquare));
                        break; // Stop moving in this direction after "capture"
                    }
                }
            }
            return moves;
        }
        public static List<Move> GetDiagonalMoves(this Piece piece, Board board, bool isForKing = false)
        {
            return GetMoves(piece, board, _diagonalDeltas, isForKing);
        }

        public static List<Move> GetVerticalHorizontalMoves(this Piece piece, Board board, bool isForKing = false)
        {
            return GetMoves(piece, board, _horizontalVerticalDeltas, isForKing);
        }
        public static List<Move> GetKnightMoves(this Piece piece,Board board)
        {
            var moves = new List<Move>();
            var currentLocation = piece.CurrentLocation;
            var knightMoves = new List<(int file, int rank)>
            {
                (2, 1), (1, 2), (-1, 2), (-2, 1),
                (-2, -1), (-1, -2), (1, -2), (2, -1)
            };

            foreach (var (deltaFile, deltaRank) in knightMoves)
            {
                int targetFile = currentLocation.File + deltaFile;
                int targetRank = currentLocation.Rank + deltaRank;
                if (IsOnBoard(targetFile, targetRank))
                {
                    var targetSquare = board.Squares[targetFile, targetRank];

                    if (targetSquare.Piece == null || targetSquare.Piece.Color != piece.Color)
                    {
                        moves.Add(new Move(currentLocation, targetSquare));
                    }
                }
            }

            return moves;
        }

        public static bool IsOnBoard(int file, int rank)
        {
            return file >= 0 && file <= 7 && rank >= 0 && rank <= 7;
        }
    }
}
