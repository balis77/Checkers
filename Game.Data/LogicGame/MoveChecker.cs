using Games.Data;
using Games.Domain;

namespace CheckGame.Data.LogicGame
{
    public interface IMoveChecker
    {
        void MoveCheckers(Checker activeChecker);
    }
    public static class SetInList
    {
        public static (int row, int column) SetPossible(int rows, int column)
        {
            return (rows, column);
        }
    }
    public sealed class MoveChecker
    {
        private const string WHITE = "white";
        private const string BLACK = "black";

        public CheckerRepository checkerRepository { get; set; }
        public List<(int row, int column)> cellsPosibleMove { get; set; } = new List<(int row, int column)>();
        public List<(int row, int column)> cellsPosibleBeat { get; set; } = new List<(int row, int column)>();
        public List<Checker> CheckBotMove { get; set; } = new List<Checker>();
        public List<Checker> CheckBotBeat { get; set; } = new List<Checker>();
        public Checker CheckClick { get; set; }
        public bool ProgressСheck = true;


        public MoveChecker(CheckerRepository repos)
        {
            checkerRepository = repos;
        }

        public void GeneradeBotBlack()
        {

            Random random = new Random();

            var CheckersBlack = checkerRepository.GetAllCheckersBlack();
            if (!CheckersBlack.Any())
            {
                checkerRepository.GameOver();
                return;

            }
            foreach (var checker in CheckersBlack)
            {
                Checkers(checker);
            }

            GetPossibleMove(random);

        }
        public void GetPossibleMove(Random random)
        {

            if (CheckBotBeat.Any())
            {
                CheckClick = CheckBotBeat[0];
            }
            else
            {
                var amountCheck = CheckBotMove.Count();
                int rndAmount = random.Next(0, amountCheck);
                if (rndAmount != 0)
                    rndAmount = rndAmount - 1;

                CheckClick = CheckBotMove[rndAmount];
            }
            cellsPosibleBeat.Clear();
            cellsPosibleMove.Clear();

            Checkers(CheckClick);

            if (CheckBotBeat.Any())
            {
                int row = cellsPosibleBeat[0].row;
                int column = cellsPosibleBeat[0].column;
                MoveLogic(row, column);
            }
            else
            {
                int rndAmount = random.Next(0, cellsPosibleMove.Count());
                int row = cellsPosibleMove[rndAmount].row;
                int column = cellsPosibleMove[rndAmount].column;
                MoveLogic(row, column);
            }
            CheckBotBeat.Clear();
            CheckBotMove.Clear();
        }
        public void ChecksBeat()
        {
            var White = checkerRepository.GetAllCheckersWhite();

            bool IsBatte = false;

            if (!IsBatte || cellsPosibleBeat.Count <= 0)
            {
                foreach (var checker in White)
                {
                    SomeMethod(checker);
                }

            }

            if (cellsPosibleBeat.Any())
            {
                cellsPosibleMove.Clear();
            }
        }
        /// <summary>
        /// Way 
        /// </summary>
        /// <param name="activeChecker"></param>
        public void SomeMethod(Checker activeChecker)
        {
            List<int> rowsPossible = new List<int>();
            if (activeChecker != null)
            {

                if (activeChecker.Direction == MoveDirection.white)
                {
                    rowsPossible.Add(activeChecker.Row - 1);
                }

            }
            foreach (var row in rowsPossible)
            {
                CanMoveHereCell(row, activeChecker.Column + 1);
                CanMoveHereCell(row, activeChecker.Column - 1);
            }
        }
        public void Checkers(Checker activeChecker)
        {


            if (ProgressСheck && activeChecker.Color == BLACK)
                return;
            if (!ProgressСheck && activeChecker.Color == WHITE)
                return;
            CheckMove(activeChecker);


        }

        private void CheckMove(Checker activeChecker)
        {

            if (activeChecker.Color == WHITE)
            {

                cellsPosibleMove.Clear();
                cellsPosibleBeat.Clear();

            }


            if (activeChecker != null)
            {
                CheckClick = activeChecker;

                List<int> rowsPossible = new List<int>();

                if (activeChecker.Direction == MoveDirection.black)
                {
                    rowsPossible.Add(activeChecker.Row + 1);
                }
                if (activeChecker.Direction == MoveDirection.white)
                {
                    rowsPossible.Add(activeChecker.Row - 1);
                }
                if (activeChecker.Queen)
                {
                    rowsPossible.Add(activeChecker.Row - 1);
                    rowsPossible.Add(activeChecker.Row + 1);

                }

                foreach (var row in rowsPossible)
                {
                    CanMoveHereCell(row, activeChecker.Column + 1);
                    CanMoveHereCell(row, activeChecker.Column - 1);
                }

            }
        }

        private void CanMoveHereCell(int row, int column)
        {
            if (row >= 8 || row < 0 || column >= 8 || column < 0)
            {
                return;
            }
            var checker = checkerRepository.GetChecker(row, column);
            if (checker?.Color == null && checker?.Color == null)
            {
                bool presenceCheck = CheckBotMove.Contains(CheckClick);
                if (!presenceCheck && CheckClick?.Color == BLACK)
                {
                    CheckBotMove.Add(CheckClick);
                }
                cellsPosibleMove.Add(SetInList.SetPossible(row, column));
            }
            if ((!ProgressСheck && checker?.Color == WHITE) || (ProgressСheck && checker?.Color == BLACK))
            {
                BeatChecker(row, column);
            }
        }

        private void BeatChecker(int row, int column)
        {
            int rowsDifficl = row - CheckClick.Row;
            int columnDifficl = column - CheckClick.Column;
            CanBeatChecker(row + rowsDifficl, column + columnDifficl);
        }

        private void CanBeatChecker(int row, int column)
        {
            if (row >= 8 || row < 0 || column >= 8 || column < 0)
                return;

            var checker = checkerRepository.GetChecker(row, column);
            if (checker?.Color == null)
            {
                bool presenceCheck = CheckBotBeat.Contains(CheckClick);
                if (!presenceCheck && CheckClick.Color == BLACK)
                    CheckBotBeat.Add(CheckClick);

                cellsPosibleBeat.Add(SetInList.SetPossible(row, column));

            }
        }


        public void MoveLogic(int row, int column)
        {

            bool canMove = cellsPosibleMove.Contains((row, column));
            bool canBeat = cellsPosibleBeat.Contains((row, column));
            if (!canMove && !canBeat)
                return;
            if (Math.Abs(CheckClick.Column - column) == 2)
            {
                int jumpedColumn = (CheckClick.Column + column) / 2;
                int jumpedRow = (CheckClick.Row + row) / 2;
                var checkerBeat = checkerRepository.GetChecker(jumpedRow, jumpedColumn);
                if (checkerBeat.Color != CheckClick.Color)
                {
                    checkerRepository.IntilizationRemoveChecker(checkerBeat.Color);
                    checkerRepository.checkersList?.Remove(checkerBeat);
                    CheckClick.Row = row;
                    CheckClick.Column = column;
                    Thread.Sleep(100);
                    cellsPosibleBeat.Clear();
                    Checkers(CheckClick);
                    if (cellsPosibleBeat.Any())
                    {
                        ProgressСheck = !ProgressСheck;
                    }
                }

            }
            CheckClick.Row = row;
            CheckClick.Column = column;

            if (CheckClick.Row == 0 && CheckClick.Color == WHITE)
            {
                checkerRepository.Queen(CheckClick);
            }
            if (CheckClick.Row == 7 && CheckClick.Color == BLACK)
            {
                checkerRepository.Queen(CheckClick);
            }
            ProgressСheck = !ProgressСheck;
            checkerRepository.GameOver();
            cellsPosibleMove.Clear();
            cellsPosibleBeat.Clear();

            if (!ProgressСheck)
            {
                GeneradeBotBlack();

            }




        }
    }
}
