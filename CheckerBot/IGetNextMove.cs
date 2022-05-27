using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerBot
{
    //провіряє куда можна піти
    public interface IGetNextMove
    {
        void GetPossibleMove(Random random);
    }
}
