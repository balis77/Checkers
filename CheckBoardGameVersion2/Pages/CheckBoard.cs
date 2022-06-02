
using CheckGame.Data.LogicGame;
using Games.Data;
using Games.Domain;

namespace CheckBoardGameVersion2.Pages
{
    public partial class CheckBoard
    {
        CheckerRepository checkerRepository { get; set; }
        MoveChecker moveChecker { get; set; }
        protected override void OnInitialized()
        {
            checkerRepository = new CheckerRepository();
            checkerRepository.Initialization();
            moveChecker = new MoveChecker(checkerRepository);
        }
        public void SomeMethod(Checker checker)
        { 
            moveChecker.Checkers(checker);
            moveChecker.ChecksBeat();
        }
        
    }
}
