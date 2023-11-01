using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Pieces;
using ChessEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.BoardRepresentation
{
    public class Board : ICloneable
    {
        private static readonly string _startPosFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        public readonly Square[,] Squares = new Square[8, 8];
        public Board()
        {
            InitializeBoardFromFen(_startPosFen);
        }
        public Board(string fen)
        {
            InitializeBoardFromFen(fen);
        }
        public Piece? GetPiece(ushort file, ushort rank)
        {
            return Squares[file, rank].Piece;
        }
        private void InitializeBoardFromFen(string fen)
        {
            var ranksPieces = fen.Split('/');
            ushort rankValue = 7;
            foreach (var rank in ranksPieces)
            {
                ushort fileValue = 0;
                foreach (var piece in rank)
                {
                    if (char.IsDigit(piece))
                    {
                        var emptySquareCount = (ushort)char.GetNumericValue(piece);
                        InitializeEmptySquares(fileValue, rankValue, emptySquareCount);
                        fileValue += emptySquareCount;
                    }
                    else
                    {
                        InitializePiece(fileValue, rankValue, piece);
                        fileValue++;
                    }
                }
                rankValue--;
            }
        }
        private void InitializePiece(ushort fileValue, ushort rankValue, char pieceNotation)
        {
            var square = new Square(fileValue, rankValue);
            Squares[fileValue, rankValue] = square;
            var pieceData = FenNotation.GetPieceByChar(pieceNotation);
            var piece = PieceFactory.CreatePiece(pieceData.Item1, pieceData.Item2, square);
            if (piece is not null)
            {
                square.Piece = piece;
            }
        }
        private void InitializeEmptySquares(ushort fileValue, ushort rankValue, int number)
        {
            if (number == 0) return;
            Squares[fileValue, rankValue] = new Square(fileValue, rankValue);
            InitializeEmptySquares(++fileValue, rankValue, --number);
        }

        public object Clone()
        {
            return new Board(GetFenOfBoardState());
        }
        public string GetFenOfBoardState()
        {
            var fen = @"";

            for (short rank = 7; rank >= 0; rank--)
            {
                int emptySquares = 0;
                for (ushort file = 0; file < 8; file++)
                {
                    var square = Squares[file, rank];
                    if (square.Piece is null)
                    {
                        emptySquares++;
                        continue;
                    }
                    else
                    {
                        if (emptySquares > 0)
                            fen += emptySquares;
                        fen += square.Piece.ToString();
                        emptySquares = 0;
                    }
                }
                if (emptySquares > 0)
                {
                    fen += emptySquares;
                }
                if (rank > 0)
                {
                    fen += '/';
                }
            }
            return fen;
        }
        public List<Move> GetPseudoLegalMoves(Color color, GameState state)
        {
            var pseudoLegalMoves = new List<Move>();
            var squares = state.Board.Squares;

            GetAllPieces(state)
                .Where(x => x.Color == color)
                .ToList()
                .ForEach(x => pseudoLegalMoves.AddRange(x.GetMoves(state)));
            return pseudoLegalMoves;
        }
        public List<Piece> GetAllPieces(GameState state)
        {
            var squares = state.Board.Squares;
            var listOfPieces = new List<Piece>();
            foreach (var square in squares)
            {
                if(square.Piece != null) listOfPieces.Add(square.Piece);
            }
            return listOfPieces;
        }
        public List<Piece> GetPiecesOfColor(GameState state, Color color)
        {
            var squares = state.Board.Squares;
            var listOfPieces = new List<Piece>();
            foreach (var square in squares)
            {
                if (square.Piece != null && square.Piece.Color ==  color) listOfPieces.Add(square.Piece);
            }
            return listOfPieces;
        }
    }
}
