
using Games.Domain;

namespace CheckBoardGameVersion2.Pages
{
    public partial class Index
    {
        List<Checker> blackCheckers = new List<Checker>();
        List<Checker> whiteCheckers = new List<Checker>();
        protected override void OnInitialized()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = (i + 1) % 2; j < 8; j += 2)
                {
                    blackCheckers.Add(new Checker
                    {
                        Color = "black",
                        Column = j,
                        Row = i,
                        Direction = MoveDirection.black
                    });
                }
            }

            for (int i = 5; i < 8; i++)
            {
                for (int j = (i + 1) % 2; j < 8; j += 2)
                {
                    whiteCheckers.Add(new Checker
                    {
                        Color = "white",
                        Column = j,
                        Row = i,
                        Direction = MoveDirection.white
                    });
                }
            }
        }
        Checker activeChecker = null;
        List<(int row, int column)> cellsPossible = new();
        void EvaluateCheckerSpots()
        {
            cellsPossible.Clear();
            if (activeChecker != null)
            {
                List<int> rowsPossible = new List<int>();
                if (activeChecker.Direction == MoveDirection.black ||
                    activeChecker.Direction == MoveDirection.king)
                {
                    rowsPossible.Add(activeChecker.Row + 1);
                }
                if (activeChecker.Direction == MoveDirection.white ||
                    activeChecker.Direction == MoveDirection.king)
                {
                    rowsPossible.Add(activeChecker.Row - 1);
                }

                foreach (var row in rowsPossible)
                {
                    EvaluateSpot(row, activeChecker.Column - 1);
                    EvaluateSpot(row, activeChecker.Column + 1);
                }
            }
        }
        void EvaluateSpot(int row, int column, bool firstTime = true)
        {
            var blackChecker = blackCheckers.FirstOrDefault(
                n => n.Column == column && n.Row == row);

            var whiteChecker = whiteCheckers.FirstOrDefault(
                n => n.Column == column && n.Row == row);

            if (blackChecker == null && whiteChecker == null)
            {
                cellsPossible.Add((row, column));
            }
            else if (firstTime)
            {
                // can i jump this checker?
                if ((whiteTurn && blackChecker != null) ||
                    (!whiteTurn && whiteChecker != null))
                {
                    int columnDifference = column - activeChecker.Column;
                    int rowDifference = row - activeChecker.Row;

                    EvaluateSpot(row + rowDifference, column + columnDifference, false);
                }
            }
        }
        void MoveChecker(int row, int column)
        {
            bool canMoveHere = cellsPossible.Contains((row, column));
            if (!canMoveHere)
                return;

            if (Math.Abs(activeChecker.Column - column) == 2)
            {
                // we jumped something
                int jumpedColumn = (activeChecker.Column + column) / 2;
                int jumpedRow = (activeChecker.Row + row) / 2;

                var blackChecker = blackCheckers.FirstOrDefault(
                    n => n.Row == jumpedRow && n.Column == jumpedColumn);

                if (blackChecker != null)
                    blackCheckers.Remove(blackChecker);

                var whiteChecker = whiteCheckers.FirstOrDefault(
                    n => n.Row == jumpedRow && n.Column == jumpedColumn);

                if (whiteChecker != null)
                    whiteCheckers.Remove(whiteChecker);
            }

            activeChecker.Column = column;
            activeChecker.Row = row;

            if (activeChecker.Row == 0 && activeChecker.Color == "white")
            {
                activeChecker.Direction = MoveDirection.king;
            }
            if (activeChecker.Row == 7 && activeChecker.Color == "black")
            {
                activeChecker.Direction = MoveDirection.king;
            }

            activeChecker = null;
            whiteTurn = !whiteTurn;
            EvaluateCheckerSpots();
        }
        void CheckerClicked(Checker checker)
        {
            if (whiteTurn && checker.Color == "black")
                return;
            if (!whiteTurn && checker.Color == "white")
                return;
            activeChecker = checker;
            EvaluateCheckerSpots();
        }

        bool whiteTurn = true;
    }
}
