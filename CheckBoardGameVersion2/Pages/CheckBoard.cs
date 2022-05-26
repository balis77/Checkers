
using CheckerBot;
using CheckGame.Data.LogicGame;
using Games.Data;

namespace CheckBoardGameVersion2.Pages
{
    public partial class CheckBoard
    {
        CheckerRepository checkerRepository { get; set; }
        MoveChecker moveChecker { get; set; }
        BotClass CheckerBots { get; set; }
        protected override void OnInitialized()
        {
            checkerRepository = new CheckerRepository();
            checkerRepository.Initialization();
            moveChecker = new MoveChecker(checkerRepository);
            CheckerBots = new BotClass(moveChecker, checkerRepository);
        }
    }
}
