using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public static class UICursor
    {
        private static UIContainer? currentContainer { get; set; }
        private static UIContainer? mainContatiner { get; set; }
        public static int CurrentIndex = 0;
        //첫음 커서 필요한곳에서 onfocust호출해야할지도
        //커런트 컨테이너도 세팅해야함

        public static void Click()
        {
            if(currentContainer.GetContent(CurrentIndex) is UiContainerGridContent current)
            {

                current.OnClick() ;
            }
            else
            {
                if (currentContainer.GetContent(CurrentIndex) is UIContainer currentCon)
                {
                    ToInner();
                }
                else
                {
                    currentContainer.OnClick();
                }
            }
        }

        private static void ToInner()
        {
            currentContainer = currentContainer.GetContent(CurrentIndex) as UIContainer;
            currentContainer.OnLoseFocus();
            currentContainer.OnClick();
            CurrentIndex = 0;
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }
        private static void ToOuter()
        {
            UIContainer? focused = currentContainer.GetContent(CurrentIndex) as UIContainer;
            if (focused != null)
            {
                currentContainer.OnLoseFocus();
            }
            currentContainer = currentContainer.GetOwner() as UIContainer;
            CurrentIndex = 0;
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }

        public static void Escape()
        {
            if (currentContainer == mainContatiner)
            {
                GameManager.gameState = GameState.ADVENTURE;
                return;
            }
            currentContainer.GetContent(CurrentIndex).OnLoseFocus();
            currentContainer = currentContainer.GetOwner() as UIContainer;
            CurrentIndex = 0;
            currentContainer.GetContent(CurrentIndex).OnFocus();
            
        }

        public static void Move(EDirection dir)
        {
            if(currentContainer == null) return;
            currentContainer.GetContent(CurrentIndex).OnLoseFocus();
            
            CurrentIndex = currentContainer.GetMovedIndex(CurrentIndex, dir);
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }

        public static void InitialCursor(UIContainer uIContainer)
        {
            mainContatiner = uIContainer;
            currentContainer = uIContainer;
            uIContainer.GetContent(0).OnFocus();
        }

        public static void ReFocus()
        {
            while(true)
            {
                if (currentContainer.GetContent(CurrentIndex) == null)
                {
                    CurrentIndex--;
                }
                if(CurrentIndex < 0)
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
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }
    }
}
