using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class Enemy : Pawn
    {
        DropItemGeneratorComponent dropItemGeneratorComponent;
        public Enemy(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, hp, position, size, overlap)
        {
            dropItemGeneratorComponent = new DropItemGeneratorComponent(this);
            dropItemGeneratorComponent.AddItemDropTable(new ItemConsumable(inventoryComponent ,"Apple"));

            foreach (var item in dropItemGeneratorComponent.GetRandomItems(5))
            {
                inventoryComponent.Add(item);
            }

            fightComponent.OnDelayedDead += DropItem;
        }
        public Enemy(string name, int hp):this(name, hp, new Vec2(0, 0), new Vec2(0, 0), true) 
        {
        }

        public void DropItem()
        {
            UIFightLogManager.Append($"{Name} is dead. \n");
            foreach (var item in inventoryComponent.Items)
            {
                item.ownedInventory = fightComponent.opponentForItem.GetInventory();
                fightComponent.opponentForItem.GetInventory().Add(item);
                UIFightLogManager.Append($"got {item.GetItemUiString()}. \n");
            }
            inventoryComponent.Items.Clear();
        }

    }
}
