using Games.Data;
using Games.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Data
{
    public interface ICheckerRepository
    {
        
        Checker DeleteCheckers(int i, int j);
        Checker GetChecker(int i, int j);
        Checker GetChecker(Checker checker);
        List<Checker> GetAllCheckersBlack();
        void Initialization();
    }
}
