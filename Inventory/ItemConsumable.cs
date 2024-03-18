using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class ItemConsumable:Item
    {
        private int quantity;
        public ItemConsumable(string name)
        {
            this.name = name;
        }
    }
}
