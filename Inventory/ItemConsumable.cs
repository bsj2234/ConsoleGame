using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class ItemConsumable:Item
    {
        protected int healAmount = 10;
        protected int quantity;
        public ItemConsumable(InventoryComponent ownedInventory, string name) : base(ownedInventory, name)
        {
        }
        public void OnConsumeClick(object s, EventArgs args)
        {
            Consume();
        }

        public void Consume()
        {
            if (ownedInventory.Owner is  Player pl) 
            {
                ownedInventory.Remove(this);
                pl.Heal(healAmount);
            }

        }
    }
}
