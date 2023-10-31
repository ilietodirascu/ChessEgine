using ChessEngine.Enums;
using ChessEngine.Game;
using ChessEngine.Utility;
using System.Security.Cryptography;

namespace ChessEngine.AI
{
    public class EngineUCI
    {
        private Bot _player;
        public EngineUCI()
        {
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
            var words = message.Split(' ');
            if (words.Length == 2 && words[1] == "startpos")
            {
                _player = new Bot(Color.White);
                _player.SetBoardAsWhite();
                return;
            }
            else if (words.Length == 4 && words[2] == "moves" && _player is null)
            {
                _player = new Bot(Color.Black);
            }
            _player.UpdateBoard(words[^1]);
        }
        void ProcessGoCommand()
        {
            Respond($"bestmove {_player.GetMove()}");
        }
    }
}
