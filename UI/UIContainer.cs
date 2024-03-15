using MyData;
namespace ConsoleGameProject
{
    public abstract class UIContainer:UI
    {
        protected List<UI> ContentUIList = new List<UI>();

        public UIContainer(bool isMain = false) :base(isMain)
        { }

        abstract public int GetRowIndex(int index);
        abstract public int GetColumnIndex(int index);

        
        abstract public Vec2 GetUiSizeOfIndex(int index);
        abstract public Vec2 GetAbsoluteUiPositionOfIndex(int index);
        abstract public UI GetContent(int index);

        abstract public int GetMovedIndex(int cur, Direction direction);

        public virtual void OnClick()
        {
        }
        public virtual UI GetOwner()
        {
            return owner;
        }


    }
}
