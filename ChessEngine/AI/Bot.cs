using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI
{
    public class Bot
    {
        public Color Color { get; set; }
        private GameState _state;
        
        public void SetBoardAsWhite()
        {
            Color = Color.White;
            var board = new Board();
            _state = new(board);
        }
        public void SetBoardAsBlack(string position)
        {
            Color = Color.Black;
            var board = new Board(position);
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
    }
}
