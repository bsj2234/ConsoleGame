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
        public void Consume()
        {
            ownedInventory.Remove(this);
            ownedInventory.Owner.Heal(healAmount);

        }
    }
}
