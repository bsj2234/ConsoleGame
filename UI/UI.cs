using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public abstract class UI
    {
        protected UI? owner;
        protected string name;
        protected Vec2 UISize;
        protected Vec2 UIPosAbsolute;
        protected int index;
        protected bool focus;
        public UI(string name ,bool isMain = false)
        {
            if(isMain)
                UISize = new Vec2(Console.WindowWidth, Console.WindowHeight-1);
            this.name = name;
        }
        public UI(string name, Vec2 pos, Vec2 size):this(name)
        {
            this.UIPosAbsolute = pos;
            this.UISize = size;
            this.name = name;
        }



        //virtual public Vec2 GetAbsolutePosition(int index)
        //{
        //    if(owner == null)
        //        return UIPosAbsolute;
        //    else
        //        return owner.GetAbsolutePosition(index);
        //}
        public virtual void OnFocus()
        {
            focus = true;
        }
        public virtual void OnLoseFocus()
        {
            focus = false;
        }


        public virtual void Draw() { }

        public void InitOwner(UIContainer owner, int currentIndex)
        {
            this.owner = owner;
            if(owner.UISize == new Vec2())
                throw new NullReferenceException();
            UISize = owner.GetUiSizeOfIndex(currentIndex);
            UIPosAbsolute = owner.GetAbsoluteUiPositionOfIndex(currentIndex);
        }

        public Vec2 GetPos()
        {
            return UIPosAbsolute;
        }
    }
}
