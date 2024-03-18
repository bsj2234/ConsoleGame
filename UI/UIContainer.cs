﻿using MyData;
namespace ConsoleGameProject
{
    public abstract class UIContainer:UI
    {
        protected List<UI> ContentUIList = new List<UI>();

        public UIContainer(string name, bool isMain = false) :base(name, isMain)
        { }

        abstract public int GetRowIndex(int index);
        abstract public int GetColumnIndex(int index);

        
        abstract public Vec2 GetUiSizeOfIndex(int index);
        abstract public Vec2 GetAbsoluteUiPositionOfIndex(int index);
        abstract public UI GetContent(int index);

        abstract public int GetMovedIndex(int cur, EDirection direction);

        public virtual void OnClick()
        {
        }
        public virtual UI GetOwner()
        {
            return owner;
        }


    }
}
