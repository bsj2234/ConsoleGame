using System.Reflection.Metadata;

namespace ConsoleGameProject
{
    internal class ItemWeapon:Item
    {
        public int Atk{ get; set; }
        public ItemWeapon(InventoryComponent ownedInventory, string name, int atk) : base(ownedInventory, name)
        {
            Atk = atk;
        }

        public override string GetItemUiString()
        {
            string result = $"{this.name}\n{Atk}";
            return result;
        }
    }
}
