using Games.Data;
using Games.Domain;

namespace CheckGame.Data.LogicGame
{
    public sealed class MoveChecker
    {
        private const string WHITE = "white";
        private const string BLACK = "black";

        public CheckerRepository checkerRepository { get; set; }
        public List<(int row, int column)> cellsPosibleMove { get; set; } = new List<(int row, int column)>();
        public List<(int row, int column)> cellsPosibleBeat { get; set; } = new List<(int row, int column)>();
        public Checker CheckClick { get; set; }
        private bool ProgressСheck = true;
        private bool beatChecker;

        public MoveChecker()
        {

        }
        public MoveChecker(CheckerRepository repos)
        {
            checkerRepository = repos;
        }
        public void CheckDesk(List<Checker> CheckerAll)
        {
            foreach (var Checker in CheckerAll)
            {
                CheckMove(Checker);
            }
        }
        private void CheckMove(Checker activeChecker)
        {
            cellsPosibleMove.Clear();
            if (activeChecker != null)
            {
                CheckClick = activeChecker;

                List<int> rowsPossible = new List<int>();
                List<int> columnPossible = new List<int>();

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
        public (int row, int column) SetPossible(int rows, int column)
        {
            return (rows, column);
        }
        private void BeatChecker(int row, int column)
        {
            int rowsDifficl = row - CheckClick.Row;
            int columnDifficl = column - CheckClick.Column;
            CanBeatChecker(row + rowsDifficl, column + columnDifficl);
        }
        private void CanBeatChecker(int row, int column)
        {
            var checker = checkerRepository.GetChecker(row, column);
            if (checker?.Color == null)
            {
                
                cellsPosibleMove.Add(SetPossible(row, column));

            }
        }
        private void CanMoveHereCell(int row, int column)
        {
            var checker = checkerRepository.GetChecker(row, column);
            if (checker?.Color == null && checker?.Color == null)
            {
                cellsPosibleMove.Add(SetPossible(row, column));
            }
            if ((!ProgressСheck && checker?.Color == WHITE) || (ProgressСheck && checker?.Color == BLACK))
            {
                BeatChecker(row, column);
            }
        }
        public void MoveCheckers(Checker activeChecker)
        {
            if (ProgressСheck && activeChecker.Color == BLACK)
                return;
            if (!ProgressСheck && activeChecker.Color == WHITE)
                return;
            CheckMove(activeChecker);

        }
        public void MoveLogic(int row, int column)
        {
            bool canMove = cellsPosibleMove.Contains((row, column));

            if (!canMove)
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

            CheckMove(CheckClick);
            ProgressСheck = !ProgressСheck;
            cellsPosibleMove.Clear();

        }
    }
}
