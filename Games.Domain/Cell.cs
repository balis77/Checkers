using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games.Domain
{
    public sealed class Cell
    {
        //public int Id { get; set; }
        public bool Color { set; get; } = false;
        public int Row { get; set; }
        public int Column { get; set; }
        public Checker Checker { get; set; }
        public bool MovePossible { get; set; }
        public void RemoveChecker()
        {
            Checker = null;
        }
    }
}
