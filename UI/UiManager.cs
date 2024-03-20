
namespace ConsoleGameProject
{
    public static class UiManager
    {
        private static UiContainerGrid? mainUi = null;
        public static UiContainerGrid? GetMain()
        {
            return mainUi;
        }
        public static void SetMain(UiContainerGrid? ui)
        {
            mainUi = ui;
        }
    }
}