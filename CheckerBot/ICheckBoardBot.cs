using CheckGame.Data.LogicGame;
using Games.Data;
using Games.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerBot
{
    //провіряє всю доску
    public interface ICheckBoardBot
    {
        void GeneradeBotBlack();
    }


    public sealed class BotClass : ICheckBoardBot, IGetNextMove
    {
        private List<Checker> CheckMove { get; set; } = new List<Checker>();
        private List<Checker> CheckBeat { get; set; } = new List<Checker>();
        private Checker Click { get; set; }
        private List<Checker> CheckersBlack { get; set; }
        private CheckerRepository CheckerRepository { get; set; }
        private MoveChecker Move { get; set; }
        public List<(int row, int column)> cellsPossibleMove { get; set; }
        public List<(int row, int column)> cellsPossibleBeat { get; set; }

        public BotClass(MoveChecker moveChecker, CheckerRepository checkerRepository)
        {
            Move = moveChecker;
            CheckerRepository = checkerRepository;
        }
        public void GeneradeBotBlack()
        {

            Random random = new Random();
            cellsPossibleMove = new List<(int row, int column)>();
            cellsPossibleBeat = new List<(int row, int column)>();
            CheckersBlack = CheckerRepository.GetAllCheckersBlack();
            if (!CheckersBlack.Any())
            {
                CheckerRepository.GameOver();
                return;

            }
            foreach (var checker in CheckersBlack)
            {
                GetNextMove(checker);
            }

            GetPossibleMove(random);

        }



        private void GetNextMove(Checker activeChecker)
        {
            if (activeChecker != null)
            {
                Click = activeChecker;
                List<int> rowsPossible = new List<int>();

                if (activeChecker.Direction == MoveDirection.black)
                {
                    rowsPossible.Add(activeChecker.Row + 1);
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
            var checker = CheckerRepository.GetChecker(row, column);
            if (checker?.Color == null)
            {
                bool presenceCheck = CheckMove.Contains(Click);
                if (!presenceCheck)
                {
                    CheckMove.Add(Click);
                }
                cellsPossibleMove.Add(Move.SetPossible(row, column));
            }
            if (checker?.Color == "white")
            {
                BeatChecker(row, column);
            }


        }
        private void BeatChecker(int row, int column)
        {
            int rowsDifficl = row - Click.Row;
            int columnDifficl = column - Click.Column;
            CanBeatChecker(row + rowsDifficl, column + columnDifficl);
        }
        private void CanBeatChecker(int row, int column)
        {
            if (row >= 8 || row < 0 || column >= 8 || column < 0)
            {
                return;
            }
            var checker = CheckerRepository.GetChecker(row, column);
            if (checker == null)
            {
                bool presenceCheck = CheckBeat.Contains(Click);
                if (!presenceCheck)
                    CheckBeat.Add(Click);
                cellsPossibleBeat.Add(Move.SetPossible(row, column));
            }
        }


        public void GetPossibleMove(Random random)
        {

            if (cellsPossibleBeat.Any())
            {
                Click = CheckBeat[0];
            }
            else
            {
                var amountCheck = CheckMove.Count();
                int rndAmount = random.Next(0, amountCheck);
                if (rndAmount != 0)
                    rndAmount = rndAmount - 1;

                Click = CheckMove[rndAmount];

            }
            cellsPossibleBeat.Clear();
            cellsPossibleMove.Clear();

            GetNextMove(Click);
            Move.CheckClick = Click;
            Move.cellsPosibleMove = cellsPossibleMove;
            Move.cellsPosibleBeat = cellsPossibleBeat;

            if (cellsPossibleBeat.Any())
            {
                int row = cellsPossibleBeat[0].row;
                int column = cellsPossibleBeat[0].column;
                Move.MoveLogic(row, column);
            }
            else
            {
                int rndAmount = random.Next(0, cellsPossibleMove.Count());
                int row = cellsPossibleMove[rndAmount].row;
                int column = cellsPossibleMove[rndAmount].column;
                Move.MoveLogic(row, column);
            }
            CheckBeat.Clear();
            CheckMove.Clear();
        }



    }
}
