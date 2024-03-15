using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    internal class UIFight : UIContainerGrid
    {
        public UIFight(string content, Action action, int rowCount = 1, int columnCount = 1, bool isMain = false) : base(rowCount, columnCount, isMain)
        {

        }
    }
}
