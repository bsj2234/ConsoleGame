using ConsoleExtenderNs;
using ConsoleGameProject;
using MyData;
using System.Diagnostics;
using System.Text;
//https://blog.naver.com/PostView.naver?blogId=qw4898&logNo=222331001254&redirect=Dlog&widgetTypeCall=true&topReferer=https%3A%2F%2Fwww.google.com%2F&directAccess=false
//참고했습니다.
//여기 winapi 서명 있다 맛있다
// https://www.codeproject.com/script/Content/ViewAssociatedFile.aspx?rzp=%2FKB%2Ftrace%2Fwinconsole%2Fwinconsole_src.zip&zep=WinConsole%2FWinConsole.cs&obid=4426&obtid=2&ovid=1
namespace MyBuffer
{
    interface IRenderable
    {
        //렌더링될 문자 출력
        public char? GetRenderChar(int x, int y);
    }
    public static class RenderManager
    {
        private static int width;
        private static int height;
        private static char[,] backBuffer;
        private static char[,] frontBuffer;






        public static void ScreenInit(int X, int Y)
        {
            ConsoleBufferInit(X, Y);
        }
        public static void RenderUIContainer(UiContainerGrid mainUiContainer)
        {
            mainUiContainer.Draw();
            Show();
        }

        public static void RenderAllActorRelative(List<Actor> AllActors, Vec2 origin)
        {
            foreach (Actor actor in AllActors)
            {
                RenderManager.DrawRelative(actor, origin);
            }
            Show();
        }
        public static void RenderAllActorRelativeOffset(List<Actor> AllActors, Vec2 origin, Vec2 offset)
        {
            foreach (Actor actor in AllActors)
            {
                RenderManager.DrawRelative(actor, origin, offset);
            }
            //이게 여기있는게 맞니?
            DestroyActors();
            Show();
        }






        private static void ConsoleBufferInit(int inWidth, int inHeight)
        {

            ScreenClear();
            width = inWidth;
            height = inHeight;
            backBuffer = new char[height, width];
            frontBuffer = new char[height, width];
            Console.SetWindowSize(width, height+2);//콘솔에 출력된 물자들이 밀리는 현상 방지를 위해 height + 1 왜 발생할까// 실행파일은 2여야함
            ConsoleExtender.SetWindowSize();
            Console.CursorVisible = false;
        }
        private static bool IsRangeOut(int inX, int inY)
        {
            if (inX >= width
              || inY >= height
              || inX < 0
              || inY < 0)
            {
                return true;
            }
            return false;
        }
        private static void Draw(char inChar, int inX, int inY)
        {
            if (!IsRangeOut(inX, inY))
                backBuffer[inY, inX] = inChar;
        }
        private static void Draw(char inChar, Vec2 v)
        {
            Draw(inChar, v.X, v.Y);
        }
        //string을 문자배열로 만들어 해당 위치에 인덱스마다 쓰는 메서드, Vec2오버로딩
        //(석진:문자열 받아서 지정한 x,y부터 y만 옆으로 이동
        public static void Draw(string inStr, int inX, int inY)
        {
            char[] temp = inStr.ToCharArray();
            int newlineCount = 0;
            int newLineXPos = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (!IsRangeOut(inX + i - newLineXPos, inY + newlineCount))
                {
                    if (temp[i] == '\n')
                    {
                        newLineXPos = i - 1;
                        newlineCount++;
                        continue;
                    }
                    backBuffer[inY + newlineCount, inX + i - newLineXPos] = temp[i];
                }
            }
        }

        //string 좌에서 우로 그리고 디음줄로 이어지게
        private static void DrawContinue(string inStr, int inX, int inY)
        {
            char[] temp = inStr.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                int x = (inX + i) % width;
                int y = inY + (inX + i) / width;
                if (!IsRangeOut(x, y))
                {
                    backBuffer[y, x] = temp[i];
                }
            }
        }
        public static void DrawHorizontal(char c, Vec2 pos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!IsRangeOut(pos.X + i, pos.Y))
                {
                    backBuffer[pos.Y, pos.X + i] = c;
                }
            }
        }
        public static void DrawVertical(char c, Vec2 pos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!IsRangeOut(pos.X, pos.Y + i))
                {
                    backBuffer[pos.Y + i, pos.X] = c;
                }
            }
        }
        private static void DrawRelative(Actor actor, Vec2 origin)
        {
            for (int i = 0; i < actor.Size.X; i++)
            {
                for (int k = 0; k < actor.Size.Y; k++)
                {
                    char? renderChar = actor.GetRenderChar(i, k);
                    Vec2 sizeOffset = new Vec2(i, k);
                    if (renderChar != null)
                    {
                        Draw((char)renderChar, actor.GetPosition() - origin + sizeOffset);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        private static void DrawRelative(Actor actor, Vec2 origin, Vec2 offset)
        {
            for (int i = 0; i < actor.Size.X; i++)
            {
                for (int k = 0; k < actor.Size.Y; k++)
                {
                    char? renderChar = actor.GetRenderChar(i, k);
                    //Debug.Assert(!(actor.RenderPriority == -1));
                    Vec2 sizeOffset = new Vec2(i, k);
                    if (renderChar != null)
                    {
                        Draw((char)renderChar, actor.GetPosition() - origin + offset + sizeOffset);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        private static void BufferExtraction()
        {
            Array.Copy(backBuffer, frontBuffer, width * height);
        }
        //백버퍼를 비우는 메서드.
        private static void ScreenClear()
        {
            FillBuffer(' ');
        }
        //백버퍼를 채우는 메서드
        private static void FillBuffer(char c)
        {
            //그냥 2차원 초기화 
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    backBuffer[y, x] = c;
        }
        private static void Print()
        {

            StringBuilder result = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result.Append(frontBuffer[y, x]);
                }
                result.Append('\n');
            }
            Console.Write(result.ToString());
        }
        private static void Show()
        {
            BufferExtraction();
            Console.SetCursorPosition(0, 0);
            Print();
            ScreenClear();
        }

        public static void CustomRanderActor()
        {
            RenderAllActorRelativeOffset(GameManager.AllActors, GameManager.player.GetPosition(), Program.SCREEN_CENTER_OFFSET);
        }

        private static void DestroyActors()
        {
            //리스트로 구현한게 잘못됐다.
            //일단 복사해놓고 제거하는게...
            List<Actor> tempForRemove = GameManager.AllActors.ToList();
            foreach (var actor in tempForRemove)
            {
                if(actor.WaitDestroy == true)
                {
                    GameManager.AllActors.Remove(actor);
                }
            }
        }
    }
}