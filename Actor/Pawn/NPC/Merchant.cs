using MyData;

namespace ConsoleGameProject
{
    public class Merchant : Actor, IInteractable
    {
        InventoryComponent inventoryComponent;
        RandomItemGeneratorComponent randomItemGeneratorComponent;
        public Merchant(string name, Vec2 position, Vec2 size, bool overlap) : base(name, position, size, overlap)
        {
            randomItemGeneratorComponent = new RandomItemGeneratorComponent(this);
            inventoryComponent = new InventoryComponent(this);
            randomItemGeneratorComponent.AddItemDropTable(new ItemConsumable(inventoryComponent, "AWW"));
            inventoryComponent.Add(randomItemGeneratorComponent.GetRandomItems(1)[0]);
        }
        public void Interact()
        {
            GameManager.StartShop();
        }
        public InventoryComponent GetInventory()
        {
            return inventoryComponent;
        }
    }
}
