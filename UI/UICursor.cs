using MyData;

namespace ConsoleGameProject
{
    public static class UiCursor
    {
        private static UiContainer? currentContainer { get; set; }
        private static UiContainer? mainContatiner { get; set; }
        public static int CurrentIndex = 0;
        //첫음 커서 필요한곳에서 onfocust호출해야할지도
        //커런트 컨테이너도 세팅해야함

        public static void Click()
        {

            currentContainer.GetContent(CurrentIndex).OnClick(null, EventArgs.Empty);
            //그림그려져있는건 안으로 안들어가고
            if (currentContainer.GetContent(CurrentIndex) is UiContainerGridContent)
            {
                return;
            }
            //그리드 컨테이너는 안으로 들어가고
            //다른데로 점프 뛸수도 있고
            if (currentContainer.GetContent(CurrentIndex) is UiContainerGrid focusedGridContainer)
            {
                UiContainerGrid? cursorParentTo = focusedGridContainer.GetCustomUiToFocusParent();

                if (cursorParentTo != null)
                {
                    FocusTo(cursorParentTo);
                }
                else
                {
                    ToInner();
                }
            }
            else
            {
                throw new Exception("처리가 존재하지 않는 UI");
            }
        }

        private static void ToInner()
        {
            currentContainer = currentContainer.GetContent(CurrentIndex) as UiContainer;
            currentContainer.SetFocus(false);
            CurrentIndex = 0;
            Ui? toFocus = currentContainer.GetContent(CurrentIndex);
            if (toFocus == null)
            {
                ToOuter();
            }
            else
            {
                toFocus.SetFocus(true);
            }
        }
        private static void ToOuter()
        {
            UiContainer? focused = currentContainer.GetContent(CurrentIndex) as UiContainer;
            if (focused != null)
            {
                currentContainer.SetFocus(false);
            }
            currentContainer = currentContainer.GetOwner() as UiContainer;
            CurrentIndex = 0;
            currentContainer.GetContent(CurrentIndex).SetFocus(true);
        }

        public static void Escape()
        {
            currentContainer.GetContent(CurrentIndex).SetFocus(false);
            CurrentIndex = 0;
            if (currentContainer == mainContatiner)
            {
                GameManager.gameState = GameState.ADVENTURE;
                return;
            }
            currentContainer = currentContainer.GetOwner() as UiContainer;
            currentContainer.GetContent(CurrentIndex).SetFocus(true);

        }

        public static void Move(EDirection dir)
        {
            if (currentContainer == null) return;
            currentContainer.GetContent(CurrentIndex).SetFocus(false);

            CurrentIndex = currentContainer.GetMovedIndex(CurrentIndex, dir);
            currentContainer.GetContent(CurrentIndex).SetFocus(true);
        }

        public static void InitialCursor(UiContainer uIContainer)
        {
            mainContatiner = uIContainer;
            currentContainer = uIContainer;
            uIContainer.GetContent(0).SetFocus(true);
        }

        public static void ReFocus()
        {
            while (true)
            {
                if (currentContainer.GetContent(CurrentIndex) == null)
                {
                    CurrentIndex--;
                }
                if (CurrentIndex < 0)
                {
                    currentContainer = mainContatiner;
                    CurrentIndex = 0;
                    break;
                }
                else
                {
                    break;
                }
            }
            currentContainer.GetContent(CurrentIndex).SetFocus(true);
        }


        public static void FocusTo(UiContainer focusUi)
        {
            currentContainer.GetContent(CurrentIndex).SetFocus(false);
            currentContainer = focusUi;
            CurrentIndex = 0;
            Ui uiToFocus = currentContainer.GetContent(CurrentIndex);
            if (uiToFocus == null)
            {
                ToOuter();
            }
            else
            {
                uiToFocus.SetFocus(true);
            }
        }

        public static void ReturnToAdventure()
        {
            while (true)
            {
                if (GameManager.gameState != GameState.ADVENTURE)
                {
                    Escape();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
