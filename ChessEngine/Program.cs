using ChessEngine.AI;

EngineUCI engine = new();

string command = String.Empty;
while (command != "quit")
{
    command = Console.ReadLine();
    //engine.ReceiveCommand(command);
}