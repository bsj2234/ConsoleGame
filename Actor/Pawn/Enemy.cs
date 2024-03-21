using MyData;

namespace ConsoleGameProject
{
    public class Enemy : Pawn
    {
        RandomItemGeneratorComponent dropItemGeneratorComponent;
        private int rand = Random.Shared.Next();
        public Enemy(string name, int hp, Vec2 position, Vec2 size, bool overlap, ECharacterType characterType) : base(name, hp, position, size, overlap, characterType)
        {
            dropItemGeneratorComponent = new RandomItemGeneratorComponent(this);
            dropItemGeneratorComponent.AddItemDropTable(new ItemConsumable(inventoryComponent, "Apple"));

            foreach (var item in dropItemGeneratorComponent.GetRandomItems(5))
            {
                inventoryComponent.Add(item);
            }

            fightComponent.OnDelayedDead += DropItem;
        }
        public Enemy(string name, int hp, ECharacterType characterType) : this(name, hp, new Vec2(0, 0), new Vec2(0, 0), true, characterType)
        {
        }

        public void DropItem()
        {
            UiFightLogManager.Append($"{Name} is dead. \n");
            foreach (var item in inventoryComponent.Items)
            {
                item.ownedInventory = fightComponent.opponentForItem.GetInventory();
                fightComponent.opponentForItem.GetInventory().Add(item);
                UiFightLogManager.Append($"got {item.GetItemUiString()}. \n");
            }
            inventoryComponent.Items.Clear();
        }



        public override string GetFightCharacterArt()
        {
            if (GameManager.UiFocusedBlink)
            {
                return AsciiArts.All[(int)chracterType][0];
            }
            else
            {
                return AsciiArts.All[(int)chracterType][1];
            }
        }

    }
}
