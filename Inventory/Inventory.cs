using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject.Inventory
{
    public class Inventory
    {
        public List<Item> Items;
        private int yieldGetIndex;
        public Inventory() 
        {
            
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public void Remove(Item item)
        {
            Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(Item item)
        {
            return Items.Contains(item);
        }

        public Item YieldGetItem() { yield return Items[yieldGetIndex++]; }
        public void YieldGetItemInit() { yieldGetIndex = 0; }


    }
}
