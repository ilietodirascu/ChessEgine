using ChessEngine.Enums;
using ChessEngine.Extensions;
using ChessEngine.Game;
using ChessEngine.Utility;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;

namespace ChessEngine.AI
{
    public class EngineUCI
    {
        private Bot _player;
        static readonly string[] positionLabels = new[] { "position", "fen", "moves" };

        public EngineUCI()
        {
            _player = new(Color.White);
        }
        public void ReceiveCommand(string message)
        {
            //Console.WriteLine(message);
            try
            {
                message = message.Trim();
                string messageType = message.Split(' ')[0].ToLower();

                switch (messageType)
                {
                    case "uci":
                        Respond("uciok");
                        break;
                    case "isready":
                        Respond("readyok");
                        break;
                    case "ucinewgame":
                        break;
                    case "position":
                        ProcessPositionCommand(message);
                        break;
                    case "go":
                        ProcessGoCommand();
                        break;
                    case "stop":
                        break;
                    case "quit":
                        break;
                    case "d":
                        break;
                    default:
                        break;
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + $"Player is null ?????:{_player is null} \n {message}");
                //Console.WriteLine(_player.Color);
            }
        }
        void Respond(string reponse)
        {
            Console.WriteLine(reponse);
        }
        void ProcessPositionCommand(string message)
        {
            if (message.ToLower().Contains("startpos"))
            {
                _player.SetBoardAsWhite();
            }
            else if (message.ToLower().Contains("fen"))
            {
                string customFen = TryGetLabelledValue(message, "fen", positionLabels);
                var fenNotation = FenNotation.GetNotation(customFen);
                _player.SetBoardGivenFen(fenNotation.PiecePlacement);
            }
            string allMoves = TryGetLabelledValue(message, "moves", positionLabels);
            if (!string.IsNullOrEmpty(allMoves))
            {
                var counter = 0;
                var color = _player.Color;
                string[] moveList = allMoves.Split(' ');
                foreach (string move in moveList)
                {
                    color = counter % 2 == 0 ? Color.White : Color.Black;
                    _player._state.MakeMove(move, color);
                    counter++;
                }
                _player.Color = color.GetOpposingColor();
            }
        }
        void ProcessGoCommand()
        {
            Respond($"bestmove {_player.GetMove()}");
        }
        static string TryGetLabelledValue(string text, string label, string[] allLabels, string defaultValue = "")
        {
            text = text.Trim();
            if (text.Contains(label))
            {
                int valueStart = text.IndexOf(label) + label.Length;
                int valueEnd = text.Length;
                foreach (string otherID in allLabels)
                {
                    if (otherID != label && text.Contains(otherID))
                    {
                        int otherIDStartIndex = text.IndexOf(otherID);
                        if (otherIDStartIndex > valueStart && otherIDStartIndex < valueEnd)
                        {
                            valueEnd = otherIDStartIndex;
                        }
                    }
                }

                return text.Substring(valueStart, valueEnd - valueStart).Trim();
            }
            return defaultValue;
        }

    }
}
