﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class InventoryComponent
    {
        public Pawn Owner;
        public List<Item> Items;
        public InventoryComponent(Pawn owner) 
        {
            Items = new List<Item>();
            Owner = owner;
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public Item Remove(Item item)
        {
            Items.Remove(item);
            return item;
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
