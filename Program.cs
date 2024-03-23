using MyBuffer;
using MyData;

namespace ConsoleGameProject
{

    /*
     * 인풋은 비동기로 배열에 받아
     * 하나씩 뽑아 처리
     * 
     * 
     */
    internal class Program
    {
        public static readonly Vec2 SCREEN_SIZE = new Vec2(220, 60);
        static void Main(string[] args)
        {
            InputManager.Init();
            RenderManager.ScreenInit(SCREEN_SIZE.X, SCREEN_SIZE.Y);
            GameManager.Run();

        }//endOfMain
    }
}
