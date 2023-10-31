using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Utility;
using System.Security.Cryptography;

namespace ChessEngine.AI
{
    public class Bot
    {
        public Color Color { get; init; }
        private GameState _state;

        public Bot(Color color)
        {
            Color = color;
        }
        public void SetBoardAsWhite()
        {
            var board = new Board();
            _state = new(board);
        }
        public string GetMove()
        {
            var legalMoves = _state.GetLegalMoves(Color);
            int randomIndex = new Random().Next(0, legalMoves.Count);
            var move = legalMoves[randomIndex];
            move.MakeMove(_state);
            return move.Notation;
        }
        public void UpdateBoard(string moveNotation)
        {
            InitializeState();
            var opposingColor = Color == Color.White ? Color.Black : Color.White;
            _state.MakeMoveByNotation(moveNotation, opposingColor);
        }
        public void UpdateBoardFromFen(string position)
        {
            InitializeState();
            var board = new Board(position);
            var copyMoveHistory = new List<Move>(_state.MoveHistory);
            _state = new(board)
            {
                MoveHistory = copyMoveHistory
            };

        }
        private void InitializeState()
        {
            if(_state == null )
            {
                var board = new Board();
                _state = new(board);
            }
        }
    }
}
