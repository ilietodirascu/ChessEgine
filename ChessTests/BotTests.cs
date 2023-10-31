using ChessEngine.AI;

namespace ChessTests
{
    public class BotTests
    {
        [Fact]
        public void TestBotColor()
        {
            var bot = new Bot();
            Assert.True(bot.Color == Color.None);
        }
    }
}
