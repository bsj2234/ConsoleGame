using MyBuffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class UiInventoryContainer : UiContainerGrid
    {
        protected InventoryComponent inventoryComponent;
        public UiInventoryContainer(string name, int rowCount, int columCount, InventoryComponent inventory):base(name,rowCount, columCount)
        {
            inventoryComponent = inventory;
        }

        public void RefreshItems()
        {
            Clear();
            for (int i = 0; i < rowCount*columnCount; i++)
            {
                if (inventoryComponent.Items.Count <= i)
                {
                    break;
                }
                Item item = inventoryComponent.Items[i];
                UiItem newItemUi = new UiItem(item.GetItemUiString(), item.GetItemUiString(), (item is ItemConsumable) ? (item as ItemConsumable).OnConsumeClick : null);
                AddNewUI(newItemUi, i);

            }
            UiCursor.ReFocus();
            RenderManager.RenderUIContainer(UiManager.GetMain());
        }
    }
}
