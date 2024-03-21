namespace ConsoleGameProject
{
    public class RandomItemGeneratorComponent
    {
        protected Actor owner;
        protected List<Item> dropTable;
        public RandomItemGeneratorComponent(Actor owner)
        {
            this.owner = owner;
            dropTable = new List<Item>();
        }

        public List<Item> GetRandomItems(int count)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < count; i++)
            {
                items.Add(dropTable[Random.Shared.Next() % dropTable.Count]);
            }
            return items;
        }

        public void AddItemDropTable(Item item)
        {
            dropTable.Add(item);
        }
    }
}
