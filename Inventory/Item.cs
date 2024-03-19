using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class Item
    {
        protected string name;
        protected string description;
        protected int value;
        public InventoryComponent ownedInventory;

        public Item(InventoryComponent ownedInventory, string name)
        {
            this.name = name;
            this.ownedInventory = ownedInventory;
        }
        public virtual string GetItemUiString()
        {
            return $"{name}";
        }
        public int GetValue()
        {
            return value;
        }
    }
}
