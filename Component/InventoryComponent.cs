using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class InventoryComponent
    {
        public List<Item> Items;
        public InventoryComponent() 
        {
            Items = new List<Item>();
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

        public List<Item> GetList() 
        {
            return Items;
        }


    }
}
