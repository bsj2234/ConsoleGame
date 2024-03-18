using System.Text;
using System.Runtime.InteropServices;
using MyData;
using ConsoleGameProject;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using MyBuffer;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Drawing;
using ConsoleExtender;
//https://blog.naver.com/PostView.naver?blogId=qw4898&logNo=222331001254&redirect=Dlog&widgetTypeCall=true&topReferer=https%3A%2F%2Fwww.google.com%2F&directAccess=false
//참고했습니다.
//여기 winapi 서명 있다 맛있다
// https://www.codeproject.com/script/Content/ViewAssociatedFile.aspx?rzp=%2FKB%2Ftrace%2Fwinconsole%2Fwinconsole_src.zip&zep=WinConsole%2FWinConsole.cs&obid=4426&obtid=2&ovid=1
namespace MyBuffer
{
    interface IRenderable
    {
        public char? GetRenderChar(int x, int y);
    }
    public static class RenderManager
    {
            private static int width;
        private static int height;
        private static char[,] backBuffer;
        private static char[,] frontBuffer;
        static void ConsoleBufferInit(int inWidth, int inHeight)
        {

            ScreenClear();
            width = inWidth;
            height = inHeight;
            backBuffer = new char[height, width];
            frontBuffer = new char[height, width];
            Console.SetWindowSize(width, height + 1);//콘솔에 출력된 물자들이 밀리는 현상 방지를 위해 height + 1 왜 발생할까
            PositionConsoleWindowDemo.SetWindowSize();
            Console.CursorVisible = false;
        }
        public static void ScreenInit(int X, int Y)
        {
            ConsoleBufferInit(X, Y);
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


        //문자를 백버퍼에 쓰는 메서드, Vec2오버로딩
        public static void Draw(char inChar, int inX, int inY)
        {
            if (!IsRangeOut(inX, inY))
                backBuffer[inY, inX] = inChar;
        }
        public static void Draw(char inChar, Vec2 v)
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
        public static void DrawContinue(string inStr, int inX, int inY)
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
        public static void DrawContinue(string inStr, Vec2 pos)
        {
            DrawContinue(inStr, pos.X, pos.Y);
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
        public static void DrawRelative(Actor actor, Vec2 origin)
        {
            for (int i = 0; i < actor.Size.X; i++)
            {
                for (int k = 0; k < actor.Size.Y; k++)
                {
                    char? renderChar = actor.GetRenderChar(i, k);
                    Vec2 sizeOffset = new Vec2(i, k);
                    if(renderChar != null)
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
        public static void DrawRelative(Actor actor, Vec2 origin, Vec2 offset)
        {
            for (int i = 0; i < actor.Size.X; i++)
            {
                for (int k = 0; k < actor.Size.Y; k++)
                {
                    char? renderChar = actor.GetRenderChar(i, k);
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
        public static void ScreenClear()
        {
            FillBuffer(' ');  
        }
        //백버퍼를 채우는 메서드
        public static void FillBuffer(char c)
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
                bool jump = false;
                for (int x = 0; x < width; x++)
                {
                    result.Append(frontBuffer[y, x]);
                }
                result.Append('\n');
            }
            Console.Write(result.ToString());
        }
        public static void Show()
        {
            BufferExtraction();
            Console.SetCursorPosition(0, 0);
            Print();
            ScreenClear();
        }
        public static void RenderUIContainer(UiContainerGrid mainUiContainer)
        {
            mainUiContainer.Draw();
        }

        internal static void DrawAllActorRelative(List<Actor> AllActors, Vec2 origin)
        {
            foreach (Actor actor in AllActors)
            {
                RenderManager.DrawRelative(actor, origin);
            }
        }
        internal static void DrawAllActorRelative(List<Actor> AllActors, Vec2 origin, Vec2 offset)
        {
            foreach (Actor actor in AllActors)
            {
                RenderManager.DrawRelative(actor, origin, offset);
            }
        }
    }
}