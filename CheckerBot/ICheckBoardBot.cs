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
        void GetAllCheckers();
    }
    public interface IGetNextMove
    {
    }
    public interface IGetPossibleBeat
    {
        void GetPossibleBeat();
    }
    public sealed class BotClass : ICheckBoardBot,IGetNextMove,IGetPossibleBeat
    {
        private Checker CheckBotChecker { get; set; }
        private CheckerRepository CheckerRepository { get; set; }
        private MoveChecker MoveChecker { get; set; }
        public List<(int row,int column)> cellsPossibleMoveBot { get; set; }
        public List<(int row, int column)> cellsPossibleBeatBot { get; set; }

        public BotClass(MoveChecker moveChecker,CheckerRepository checkerRepository)
        {
            MoveChecker = moveChecker;
            CheckerRepository = checkerRepository;
        }
        public void GetAllCheckers ()
        {
            var biba = CheckerRepository?.checkersList?.FindAll(n => n.Color == "black");
            foreach (var checker in biba)
            {
                GetNextMove(checker);
            }
            GetPossibleBeat();
        }

        private void GetNextMove(Checker activeChecker)
        {
            if (activeChecker != null)
            {

                CheckBotChecker = activeChecker;
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
                    if (activeChecker.Column != -1 && activeChecker.Column !=8)
                    {
                        //CanMoveHereCell(row, activeChecker.Column + 1);
                        //CanMoveHereCell(row, activeChecker.Column - 1);
                    }
                    
                }
            }
        }
        private void CanMoveHereCell(int row, int column)
        {
            var checker = CheckerRepository.GetChecker(row, column);
            if (checker?.Color == null && checker?.Color == null)
            {
                cellsPossibleMoveBot.Add(MoveChecker.SetPossible(row, column));
            } 
            if (( checker?.Color == "white") || ( checker?.Color == "black"))
            {
                BeatChecker(row, column);
            }
        }
        private void BeatChecker(int row, int column)
        {
            int rowsDifficl = row - CheckBotChecker.Row;
            int columnDifficl = column - CheckBotChecker.Column;
            CanBeatChecker(row + rowsDifficl, column + columnDifficl);
        }
        private void CanBeatChecker(int row, int column)
        {
            var checker = CheckerRepository.GetChecker(row, column);
            if (checker?.Color == null)
            {
                cellsPossibleMoveBot.Add(MoveChecker.SetPossible(row, column));
            }
        }

        public void GetPossibleBeat()
        {
            Random _rnd = new Random();
            if ( cellsPossibleMoveBot != null)
            {
                int biba = cellsPossibleMoveBot.Count();
                MoveCheckers(_rnd.Next(0, biba));
            }
            //if (cellsPossibleBeatBot != null)
            //{
            //    BeatChecker(_rnd.Next(0, cellsPossibleBeatBot.Count()));
            //}
        }

        private void MoveCheckers(int checkMove)
        {
            int row = cellsPossibleMoveBot[checkMove].row;
            int column = cellsPossibleMoveBot[checkMove].column;
            MoveLogic(row, column);
        }
        private void BeatChecker(int checkBeat)
        {
            int row = cellsPossibleBeatBot[checkBeat].row;
            int column = cellsPossibleBeatBot[checkBeat].column;
            MoveLogic(row, column);
           
        }
        private void MoveLogic(int row, int column)
        {
            bool canMove = cellsPossibleMoveBot.Contains((row, column));

            if (!canMove)
                return;
            if (Math.Abs(CheckBotChecker.Column - column) == 2)
            {
                int jumpedColumn = (CheckBotChecker.Column + column) / 2;
                int jumpedRow = (CheckBotChecker.Row + row) / 2;
                var checkerBeat = CheckerRepository.GetChecker(jumpedRow, jumpedColumn);
                if (checkerBeat.Color != CheckBotChecker.Color)
                {
                    CheckerRepository.IntilizationRemoveChecker(checkerBeat.Color);
                    CheckerRepository.checkersList?.Remove(checkerBeat);
                }
            }
            CheckBotChecker.Row = row;
            CheckBotChecker.Column = column;

            if (CheckBotChecker.Row == 0 && CheckBotChecker.Color == "white")
            {
                CheckerRepository.Queen(CheckBotChecker);
            }
            if (CheckBotChecker.Row == 7 && CheckBotChecker.Color == "black")
            {
                CheckerRepository.Queen(CheckBotChecker);
            }

            GetNextMove(CheckBotChecker);

            cellsPossibleMoveBot.Clear();

        }
    }
}
