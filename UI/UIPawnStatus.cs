namespace ConsoleGameProject
{
    public class UiPawnStatus : UiContainerGridContent
    {
        public UiPawnStatus(string name, Func<string> contentFunc, Action? action)
            : base(name, contentFunc, null, 1, 1, false)
        {
        }



    }
}
