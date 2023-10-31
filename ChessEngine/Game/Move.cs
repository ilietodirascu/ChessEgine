using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Pieces;
using ChessEngine.Utility;

namespace ChessEngine.Game
{
    public class Move
    {
        private Board _board;
        public Square StartSquare { get; set; }
        public Square EndSquare { get; set; }
        public Piece EndSquarePiece { get; set; }
        public bool IsEnpassant { get; set; }
        public bool IsPromotion { get; set; }
        public string Notation { get; set; }
        public bool IsCastle { get; set; }

        public Move(Square startSquare, Square endSquare, Piece? endSquarePiece = null, bool isEnpassant = false, bool isCastle = false, bool isPromotion = false)
        {
            StartSquare = startSquare;
            EndSquare = endSquare;
            EndSquarePiece = endSquarePiece is null ? StartSquare.Piece! : endSquarePiece;
            IsEnpassant = isEnpassant;
            IsCastle = isCastle;
            IsPromotion = isPromotion;
        }
        public void MakeMove(GameState state)
        {
            _board = state.Board;
            if(IsCastle)
            {
                CastleSideEffect();
            }
            else if(IsEnpassant)
            {
                EnPassantSideEffect();
            }
            var stateStartSquare = _board.Squares[StartSquare.File, StartSquare.Rank];
            var stateEndSquare = _board.Squares[EndSquare.File,EndSquare.Rank];
            stateStartSquare.Piece = null;
            stateEndSquare.Piece = PieceFactory.CreatePiece(EndSquarePiece.Value,EndSquarePiece.Color,stateEndSquare);
            stateEndSquare.Piece!.CurrentLocation = stateEndSquare;
            this.AddMoveNotation(IsPromotion);
            if(stateEndSquare.Piece is King king && !king.HasMoved)
            {
                king.HasMoved = true;
            }
            else if(stateEndSquare.Piece is Rook rook && !rook.HasMoved)
            {
                rook.HasMoved = true;
            }
            state.MoveHistory.Add(this);
        }
        public void CastleSideEffect()
        {
            var squares = _board.Squares;
            //queen side castling
            if (StartSquare.File > EndSquare.File)
            {
                var queenSideRookSquare = squares[0, StartSquare.Rank];
                var queenSideRook = queenSideRookSquare.Piece;
                var newRookSquare = squares[StartSquare.File - 1, StartSquare.Rank];
                newRookSquare.Piece = PieceFactory.CreatePiece(queenSideRook!.Value, queenSideRook!.Color, newRookSquare);
                if (newRookSquare.Piece is Rook rook) rook.HasMoved = true;
                queenSideRookSquare.Piece = null;
            }
            else
            {
                var kingSideRookSquare = squares[7, StartSquare.Rank];
                var kingSideRookPiece = kingSideRookSquare.Piece;
                var newRookSquare = squares[StartSquare.File + 1, StartSquare.Rank];
                newRookSquare.Piece = PieceFactory.CreatePiece(kingSideRookPiece!.Value,kingSideRookPiece!.Color,newRookSquare);
                if(newRookSquare.Piece is Rook rook) rook.HasMoved = true;
                kingSideRookSquare.Piece = null;
            }
        }
        public void EnPassantSideEffect()
        {
            var piece = StartSquare.Piece;
            var color = piece!.Color;
            int direction = color == Color.White ? 1 : -1;
            var pawnToRemoveSquare = _board.Squares[EndSquare.File,EndSquare.Rank + direction];
            pawnToRemoveSquare.Piece = null;
        }
    }
}
