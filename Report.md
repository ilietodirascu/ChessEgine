# Fundamentals Of Artificial Intelligence

## Lab 3 AI Chess Player – the Minimax Algorithm by Ilie Todirascu FAF-203

### Tasks:

#### Task 1: Implement the MiniMax algorithm with the following scoring function:

Score = M aterialV alue + P ositionalV alue
For computing the MaterialValue, each piece is assigned a value (e.g., Pawn = 1, Knight
= 3, Bishop = 3, Rook = 5, Queen = 9). Then you sum these values for your pieces and
substract the value of the pieces of the oponent.
For computing the PositionalValue, you should take into account the position of each
pieces on the board (e.g the more squares a pawn has travelled, the higher their PositionalValue etc.). You should then substract the opponent’s PositionalValue from your
pieces’ PositionalValue.

#### Task 2: Implement Alpha-Beta Prunning.

#### Task 3: Implement an improved scoring (evaluation) method for MiniMax.

For example,
you could add values like KingSafetyValue, MobilityValue (nr of legal moves to each
side), PawnStructureValue (can include penalties for isolated pawns, doubled pawns, and
bonuses for passed pawns or a strong pawn chain), etc. You can also use Heuristic
Evaluation Functions. Be creative! (1p)

#### Task 4: Add at least one improvement to the MiniMax algorithm from the following list:

Progressive Deepening, Transposition Tables, Opening Books, Move Ordering, Aspiration
Window etc.

### Task Implementation

I managed to create the bot on lichess, and unfortunetly not much else.

I started creating the bot from the ground up in C# and sadly wasted too much time on bugs, and UCI communication protocol.

There is an attempt on mini max

```C#
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
```

But it doesn't work.

There are 22 tests if that is worth anything.

The current bot is not much better than random.

It just tries to capture the highest valued piece.

There were some attempts at adding some weights to encourage approaching the king, which worked but the bot would just march in with the 2 pawns closest to the king.

The validation of legal moves is pretty intersting but unoptimized, basically I create a different state of the game(meaning a new board that I can safely make moves on), then I try all the pseudolegal moves
and see if making them would expose the King to a check.

Since everything is a class, there were some issue with this approach since, a lot of the times there was the need to use reference types as if they were value types, for that I used the Prototype pattern to make deep copies.

From what I have tested, everything works, en passant, promotions,etc.

Pieces have positional value, even moves have value, this would have aided in evaluating moves, instead of making random ones if capture isn't possible.

## Conclusion

In conclusion, through this laboratory work, I've gained valuable insights into decision-making algorithms like minimax with alpha-beta pruning. These algorithms are instrumental in the realm of artificial intelligence, particularly for determining optimal moves in two-player games such as chess.

Alpha-beta pruning stands out as a key enhancement to the minimax algorithm. It achieves this by significantly reducing the number of nodes in the search tree that require evaluation. By eliminating branches that are guaranteed to yield inferior outcomes compared to already explored options, alpha-beta pruning enhances the efficiency and speed of the search process.

Programming a chess AI serves as an ideal project for delving into the intricacies of advanced optimization algorithms and system design, offering a rewarding learning experience in the realm of artificial intelligence and game theory.
