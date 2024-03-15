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
        private static UIContainer currentContainer { get; set; }
        public static int CurrentIndex = 0;
        //첫음 커서 필요한곳에서 onfocust호출해야할지도
        //커런트 컨테이너도 세팅해야함

        public static void Click()
        {
            if(currentContainer.GetContent(CurrentIndex) is UIContainerGridContent)
            {

                (currentContainer.GetContent(CurrentIndex)as UIContainerGridContent).OnClick() ;
            }
            else
            {
                if (currentContainer.GetContent(CurrentIndex) is UIContainer)
                {
                    currentContainer = currentContainer.GetContent(CurrentIndex) as UIContainer;
                    currentContainer.OnLoseFocus();
                    currentContainer.OnClick();
                    CurrentIndex = 0;
                    currentContainer.GetContent(CurrentIndex).OnFocus();
                }
                else
                {
                    currentContainer.OnClick();
                }
            }
        }
        public static void Escape()
        {
            currentContainer.GetContent(CurrentIndex).OnLoseFocus();
            currentContainer = currentContainer.GetOwner() as UIContainer;
            currentContainer.OnClick();
            CurrentIndex = 0;
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }

        public static void Move(Direction dir)
        {
            if(currentContainer == null) return;
            currentContainer.GetContent(CurrentIndex).OnLoseFocus();
            
            CurrentIndex = currentContainer.GetMovedIndex(CurrentIndex, dir);
            currentContainer.GetContent(CurrentIndex).OnFocus();
        }

        public static void InitialCursor(UIContainer uIContainer)
        {
            currentContainer = uIContainer;
            uIContainer.GetContent(0).OnFocus();
        }
    }
}
