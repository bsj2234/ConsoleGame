namespace ConsoleGameProject
{
    public class InventoryComponent
    {
        public Actor Owner;
        public List<Item> Items;
        public InventoryComponent(Actor owner)
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
