using ChessEngine.AI;

namespace ChessTests
{
    public class BotTests
    {
        [Fact]
        public void SearchTest()
        {
            var bot = new Bot(Color.White);
            bot.SetBoardGivenFen("1n3r2/1b6/p1kb4/1p2p1pp/2pp1n2/P3qr2/1P1P1P2/RNBQKBNq");
            var bestMove = bot.Search();
            Assert.True(bestMove.Notation == "d2e3");
        }
        [Fact]
        public void SearchTestFailingGame()
        {
            var bot = new Bot(Color.White);
            bot.SetBoardGivenFen("1nbqkbnr/1ppppppp/r7/8/2PP1B2/P3PN2/P4PPP/RN1QKB1R");
            var bestMove = bot.Search();
        }
    }
}
