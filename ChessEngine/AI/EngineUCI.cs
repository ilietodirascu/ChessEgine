using ChessEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.AI
{
    public class EngineUCI
    {
        private readonly Bot _player;
        public EngineUCI()
        {
            _player = new Bot();
        }
        public void ReceiveCommand(string message)
        {
            //Console.WriteLine(message);
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
                    ProcessGoCommand(message);
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
                string piecePlacement = FenNotation.GetNotation(message).PiecePlacement;
                _player.SetBoardAsBlack(piecePlacement);
            }
            else
            {
                Console.WriteLine("Invalid position command (expected 'startpos' or 'fen')");
            }
        }
        void ProcessGoCommand(string message)
        {
            Respond(_player.GetMove());
        }
    }
}
