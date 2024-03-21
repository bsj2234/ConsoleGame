using MyData;
namespace ConsoleGameProject
{
    public abstract class UiContainer : Ui
    {
        protected List<Ui> ContentUIList = new List<Ui>();

        public UiContainer(string name, bool isMain = false) : base(name, isMain)
        { }

        abstract public int GetRowIndex(int index);
        abstract public int GetColumnIndex(int index);


        abstract public Vec2 GetUiSizeOfIndex(int index);
        abstract public Vec2 GetAbsoluteUiPositionOfIndex(int index);
        abstract public Ui? GetContent(int index);

        abstract public int GetMovedIndex(int cur, EDirection direction);

        public virtual Ui GetOwner()
        {
            return owner;
        }


    }
}
