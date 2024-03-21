namespace ConsoleGameProject
{
    public class UiItem : UiContainerGridContent
    {
        public UiItem(string name, string content, EventHandler? action, int rowCount = 1, int columnCount = 1, bool isMain = false)
            : base(name, content, action, rowCount, columnCount, isMain)
        {
            AddEvenetOnClick(UpdateInventoryUi);
        }

        public void UpdateInventoryUi(object s, EventArgs args)
        {
            if (owner is UiInventoryContainer u)
            {
                u.RefreshItems();
            }
            else
            {
                throw new Exception("Not a Ui That can have Items");
            }
        }
    }
}
