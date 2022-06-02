using Games.Domain;

namespace Games.Data
{
    public class CheckerRepository : ICheckerRepository, IRemoveChecerRepository
    {
        public int ScoreBlack = 0;
        public int ScoreWhite = 0;
        public List<Checker>? checkersList { get; set; }
        public List<Checker>? RemoveColorChecker { get; set; }
        int k = 0;
        int d = 0;
        public Checker GetChecker(int row, int column) => checkersList.FirstOrDefault(x => x.Row == row && x.Column == column);
        public void Initialization()
        {

            checkersList = new List<Checker>();
            RemoveColorChecker = new List<Checker>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = (i + 1) % 2; j < 8; j += 2)
                {
                    checkersList.Add(new Checker
                    {
                        Row = i,
                        Column = j,
                        Color = "black",
                        Direction = MoveDirection.black

                    });
                }
            }

            for (int i = 5; i < 8; i++)
            {
                for (int j = (i + 1) % 2; j < 8; j += 2)
                {
                    checkersList.Add(new Checker
                    {
                        Row = i,
                        Column = j,
                        Color = "white",
                        Direction = MoveDirection.white


                    });
                }
            }
        }
        public Checker DeleteCheckers(int i, int j)
        {
            var biba = GetChecker(i, j);
            if (biba != null)
            {
                checkersList?.Remove(biba);
            }
            return biba;
        }
        public Checker GetChecker(Checker checker) => checkersList.FirstOrDefault(checker);

        public List<Checker> GetAllCheckersBlack() => checkersList.FindAll(n => n.Color == "black");
        public List<Checker> GetAllCheckersWhite() => checkersList.FindAll(n => n.Color == "white");

        public void IntilizationRemoveChecker(string removeChecker)
        {
            if (d == 4)
            {
                d = default;
                k++;
            }
            RemoveColorChecker?.Add(new Checker
            {
                Color = removeChecker,
                Column = d,
                Row = k,

            });
            d++;
        }
        public void Queen(Checker checkerQueen)
        {

            checkersList?.FindAll(s => s.Column == checkerQueen.Column && s.Row == checkerQueen.Row).ForEach(s => s.Queen = true);
        }
        public Checker GetRemoveChecker(int row, int column) => RemoveColorChecker?.FirstOrDefault(x => x.Row == row && x.Column == column);

        public void GameOver()
        {
            if (!checkersList.FindAll(n => n.Color == "black").Any())
            {
                ScoreWhite++;
                RemoveColorChecker.Clear();
                checkersList.Clear();
                Initialization();
               

            }
            if (!checkersList.FindAll(n => n.Color == "white").Any())
            {
                ScoreBlack++;
                RemoveColorChecker.Clear();
                checkersList.Clear();
                Initialization();
                
            }
        }

    }
}
