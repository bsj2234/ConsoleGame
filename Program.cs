using MyBuffer;
using MyData;

namespace ConsoleGameProject
{

    /*
     * 인풋은 비동기로 배열에 받아
     * 하나씩 뽑아 처리
     * 
     */

    internal class Program
    {
        public static readonly Vec2 SCREEN_SIZE = new Vec2(220, 60);
        public static readonly Vec2 SCREEN_CENTER_OFFSET = new Vec2(SCREEN_SIZE.X / 2, SCREEN_SIZE.Y / 2);
        static void Main(string[] args)
        {
            InputManager.Init();
            RenderManager.ScreenInit((int)SCREEN_SIZE.X, (int)SCREEN_SIZE.Y);
            GameManager.Run();

        }//endOfMain
    }
}
