namespace ConsoleGameProject
{
    public class ItemConsumable : Item
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

        //아이템 사용과 인벤토리에서 삭제
        public void Consume()
        {
            if (ownedInventory.Owner is Player pl)
            {
                ownedInventory.Remove(this);
                pl.Heal(healAmount);
            }

        }
    }
}
