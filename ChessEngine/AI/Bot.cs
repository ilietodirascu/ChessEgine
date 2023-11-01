using ChessEngine.BoardRepresentation;
using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Utility;
using System;
using System.Security.Cryptography;

namespace ChessEngine.AI
{
    public class Bot
    {
        public Color Color { get; set; }
        private Random _random = new();
        public GameState _state { get; set; }

        public Bot(Color color)
        {
            Color = color;
        }
        public void SetBoardAsWhite()
        {
            var board = new Board();
            _state = new(board);
        }
        public void SetBoardGivenFen(string fen) 
        {
            var board = new Board(fen);
            _state = new(board);
        }
        public Move Search()
        {
            return FindBestMove(3); ;
        }
       
        private GameState GetGameStateCopy()
        {
            var tempBoard = _state.Board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
            var copyState = new GameState(tempBoard)
            {
                MoveHistory = new(_state.MoveHistory)
            };
            return copyState;
        }
        public Move FindBestMove(int depth)
        {
            if (depth <= 0)
            {
                throw new ArgumentException("Depth should be greater than zero.");
            }

            Move bestMove = null;
            int bestValue = int.MinValue;
            var legalMoves = _state.GetLegalMoves(Color);
            legalMoves.ForEach(x =>
            {
                var copyState = GetGameStateCopy();
                var color = x.EndSquarePiece.Color;
                copyState.MakeMove(x.Notation, color);
                int value = Minimax(depth - 1, copyState, false);
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = x;
                }
            });

            return bestMove;
        }

        private int Minimax(int depth, GameState state, bool isMaximizingPlayer)
        {
            if (depth == 0)
            {
                return state.GetSumOfMaterial(isMaximizingPlayer ? Color : Color.GetOpposingColor());
            }

            var legalMoves = state.GetLegalMoves(Color);
            if (isMaximizingPlayer)
            {
                int bestValue = int.MinValue; // Negative infinity
                legalMoves.ForEach(x =>
                {
                    var board = state.Board.Clone() as Board;
                    var copyState = new GameState(board);
                    copyState.MakeMove(x.Notation, Color);
                    int value = Minimax(depth - 1, copyState, false);
                    bestValue = Math.Max(bestValue, value);
                });
                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue; // Positive infinity
                var opposingColor = Color.GetOpposingColor();
                var opposingLegalMoves = state.GetLegalMoves(opposingColor);
                opposingLegalMoves.ForEach(x =>
                {
                    var board = state.Board.Clone() as Board;
                    var copyState = new GameState(board);
                    copyState.MakeMove(x.Notation, opposingColor);
                    int value = Minimax(depth - 1, copyState, true);
                    bestValue = Math.Min(bestValue, value);
                });
                return bestValue;
            }
        }
        private Move GetBestMove(List<Move> moves, GameState initialState)
        {
            var board = initialState.Board;
            var moveValueDictionary = new Dictionary<Move,int>();
            moves.ForEach(x =>
            {
                var tempBoard = board.Clone() as Board ?? throw new Exception("Something Wrong with cloning board");
                var copyState = new GameState(tempBoard)
                {
                    MoveHistory = new(initialState.MoveHistory)
                };
                var color = x.EndSquarePiece.Color;
                copyState.MakeMove(x.Notation, color);
                var differenceOfMaterial = copyState.GetSumOfMaterial(color) - copyState.GetSumOfMaterial(color.GetOpposingColor());
                moveValueDictionary.Add(x,differenceOfMaterial);
            });
            if(moveValueDictionary.Values.Distinct().Count() == 1)
            {
                int randomIndex = _random.Next(0, moveValueDictionary.Count);
                return moveValueDictionary.Keys.ToList()[randomIndex];
            }
            return moveValueDictionary.MaxBy(kvp => kvp.Value).Key;
        }
        public void AdjustMoveWeights(Dictionary<Move,int> movesDict)
        {
            foreach (var item in movesDict)
            {
                item.Key.AdjustValueBasedOnKingDistance(_state);
            }
        }

        public string GetMove()
        {
            var move = Search();
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
