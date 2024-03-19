using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class UiItem : UiContainerGridContent
    {
        public UiItem(string name, string content, Action? action, int rowCount = 1, int columnCount = 1, bool isMain = false) 
            : base(name, content, action, rowCount, columnCount, isMain)
        {
        }
    }
}
