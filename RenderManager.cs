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
//https://blog.naver.com/PostView.naver?blogId=qw4898&logNo=222331001254&redirect=Dlog&widgetTypeCall=true&topReferer=https%3A%2F%2Fwww.google.com%2F&directAccess=false
//참고했습니다.
//여기 winapi 서명 있다 맛있다
// https://www.codeproject.com/script/Content/ViewAssociatedFile.aspx?rzp=%2FKB%2Ftrace%2Fwinconsole%2Fwinconsole_src.zip&zep=WinConsole%2FWinConsole.cs&obid=4426&obtid=2&ovid=1
namespace MyBuffer
{

    interface IRenderable
    {
        public char GetRenderChar(int x, int y);
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct _SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
    [StructLayoutAttribute(LayoutKind.Explicit)]
    public unsafe struct UnionStruct
    {
        [FieldOffsetAttribute(0)]
        public char UnicodeChar;
        [FieldOffsetAttribute(0)]
        byte AsciiChar;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct _CONSOLE_CURSOR_INFO
    {
        public uint dwSize;
        public bool bVisible;
    }


    struct _CHAR_INFO
    {
        ushort Attributes;
    }

    public static class RenderManager
    {
        /*
        const int GENERIC_READ = unchecked((int)0x80000000);
        const int GENERIC_WRITE = 0x40000000; 
        public const UInt32 INVALID_HANDLE_VALUE = 0xffffffff;
        private const int STD_OUTPUT_HANDLE = -11;

        const int EMPTY = 32;
  		const int CONSOLE_TEXTMODE_BUFFER = 1;

        [DllImport("kernel32")]
   		static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32")]
   		static extern IntPtr CreateConsoleScreenBuffer(int access, int share, IntPtr security, int flags, IntPtr reserved);

        [DllImport("kernel32")]
        static extern int SetConsoleCursorPosition(IntPtr buffer, int position, int y);
        [DllImport("kernel32")]
        static extern int SetConsoleCursorInfo(IntPtr buffer, ref _CONSOLE_CURSOR_INFO lpConsoleCursorInfo);
        
   		[DllImport("kernel32")]
   		static extern bool SetConsoleActiveScreenBuffer(IntPtr handle); 
        [DllImport("kernel32")]
   		static extern int FillConsoleOutputCharacter(IntPtr buffer, char character, int length, Vec2 position, out int written);

        [DllImport("kernel32")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Ansi)]
        public static extern bool WriteFile(
    IntPtr hFile,
    //System.Text.StringBuilder lpBuffer,
    char[] lpBuffer,
    int nNumberOfBytesToWrite,
    out uint lpNumberOfBytesWritten,
    [In] ref System.Threading.NativeOverlapped lpOverlapped);


        [DllImport("kernel32")]
        static extern int SetConsoleCP(uint CodePageId);
        [DllImport("kernel32")]
        static extern int SetConsoleOutputCP(uint CodePageId);





        private static Stopwatch stopwatch = new Stopwatch();
        private static int width;//가로크기
        private static int height;//세로크기
        //win api ver handle 
        private static int g_nScreenIndex;
        private static IntPtr[] g_hScreen = new IntPtr[2];
        private static int g_numOfFps;
        public static long CurTime;
        public static long OldTime;
        */



        //public static void ScreenInit(int in_width, int in_height)
        //{

            //Win Api 잠시 포기
            //    //if (SetConsoleCP(949) == 0)
            //    //{
            //    //    throw new Exception();
            //    //}
            //    //if (SetConsoleOutputCP(949) == 0)
            //    //{
            //    //    throw new Exception();
            //    //}
            //    _CONSOLE_CURSOR_INFO cci;

            //    g_hScreen[0] = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            //    g_hScreen[1] = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);

            //    cci.dwSize = 1;
            //    cci.bVisible = false;
            //    SetConsoleCursorInfo(g_hScreen[0], ref cci);
            //    SetConsoleCursorInfo(g_hScreen[1], ref cci);

            //    width = in_width; 
            //    height = in_height;
            //}

            //private static void ScreenFlipping()
            //{
            //    SetConsoleActiveScreenBuffer(g_hScreen[g_nScreenIndex]);
            //    g_nScreenIndex = (g_nScreenIndex == 0)? 1: 0;
            //}
            //public static void ScreenClear()
            //{
            //    Vec2 Coord = new Vec2(0, 0);
            //    int dw;
            //    FillConsoleOutputCharacter(g_hScreen[g_nScreenIndex], ' ', width * height, Coord, out dw);
            //}
            //public static void ScreenFill(char c)
            //{
            //    Vec2 Coord = new Vec2(0, 0);
            //    int dw;
            //    FillConsoleOutputCharacter(g_hScreen[g_nScreenIndex], c, width * height, Coord, out dw);
            //}
            //public static void ScreenFillWideChar(char c)
            //{
            //    //ScreenClear();
            //    Vec2 Coord = new Vec2(0, 0);
            //    int dw;

            //    for (int i = 0; i < width * height; i++)
            //    {
            //        Draw(c, i/width, i%width);
            //        Show();
            //        Show();
            //    }

            //    //FillConsoleOutputCharacter(g_hScreen[g_nScreenIndex], c, width * height/2, Coord, out dw);
            //    //Draw("짜짜짜짜파게티 진짜 돼ㅆ써", 0, 0); 디버그용
            //    Show();
            //}
            //public static void ScreenRelease()
            //{
            //    CloseHandle(g_hScreen[0]);
            //    CloseHandle(g_hScreen[1]);
            //}

            //private static void BackBufferDraw(char[] str, int x, int y)
            //{
            //    uint dw;
            //    Vec2 CursorPosition = new Vec2(x, y);
            //    SetConsoleCursorPosition(g_hScreen[g_nScreenIndex], x, y);
            //    NativeOverlapped temp = new NativeOverlapped();
            //    WriteFile(g_hScreen[g_nScreenIndex], str, str.Length*2, out dw, ref temp );
            //}

            //public static void Render()
            //{
            //    ScreenClear();

            //    if(CurTime - OldTime >= 1000)
            //    {
            //        OldTime = CurTime;
            //        g_numOfFps = 0;

            //    }

            //    g_numOfFps++;
            //    ScreenFlipping();
            //}

            //public static void Release()
            //{
            //}
            //public static void DrawHorizontal(char c, Vec2 pos, int count)
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (!IsRangeOut(pos.X + i, pos.Y))
            //        {
            //            Draw(c, pos.X + i, pos.Y);
            //        }
            //    }
            //}
            //public static void DrawVertical(char c, Vec2 pos, int count)
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (!IsRangeOut(pos.X, pos.Y + i))
            //        {
            //            Draw(c, pos.X, pos.Y + i);
            //        }
            //    }
            //}
            //public static void Show()
            //{
            //    ScreenFlipping();
            //}
            /*
            public static void ColorHorizontal(ConsoleColor c, Vec2 pos, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!IsRangeOut(pos.X + i, pos.Y))
                    {
                        backColorBuffer[pos.Y, pos.X + i] = c;
                    }
                }
            }
            public static void ColorVertical(ConsoleColor c, Vec2 pos, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!IsRangeOut(pos.X, pos.Y + i))
                    {
                        backColorBuffer[pos.Y + i, pos.X] = c;
                    }
                }
            }

            public static void DrawRelative(Actor actor, Vec2 playerPos)
            {
                for (int i = 0; i < actor.Size.X; i++)
                {
                    for (int k = 0; k < actor.Size.Y; k++)
                    {
                        Vec2 T = actor.GetPosition() - playerPos + Program.CENTER_OFFSET + new Vec2(i, k);
                        //플레이어 중앙에 배치
                        Draw(actor.GetRenderChar(i, k), T.X, T.Y);
                    }
                }
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
            public static void Draw(char inChar, int inX, int inY)
            {
                //if (!IsRangeOut(inX, inY))
                    BackBufferDraw(new char[1] { inChar }, inX, inY);
            }
            public static void Draw(string inStr, int inX, int inY)
            {
                if (!IsRangeOut(inX, inY))
                    BackBufferDraw(inStr.ToCharArray(), inX, inY);
            }
            public static void RenderUIContainer(UIContainerGrid mainUiContainer)
            {
                mainUiContainer.Draw();
            }
            */

            private static int width;//가로크기
        private static int height;//세로크기
        private static char[,]? backBuffer = null;//각변경사항, 정보를 입력받는 백버퍼
        private static char[,]? frontBuffer = null;//백버퍼를 받아 화면에 보여지는 프론트 버퍼
        private static ConsoleColor[,]? backColorBuffer = null;//
        private static ConsoleColor[,]? frontColorBuffer = null;//



        //가로, 세로값을  받아 저장하고 백버퍼와 프론트 버퍼를 인스턴스하한다.
        //콘솔창의 크기도 버퍼크기에 맞게 설정

        static Dictionary<char, bool> oneWidthChars = new Dictionary<char, bool>();
        static void ConsoleBufferInit(int inWidth, int inHeight)
        {

            ScreenClear();
            width = inWidth;
            height = inHeight;
            backBuffer = new char[height, width];
            frontBuffer = new char[height, width];
            backColorBuffer = new ConsoleColor[height, width];
            frontColorBuffer = new ConsoleColor[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    backColorBuffer[y, x] = ConsoleColor.White;
                    frontColorBuffer[y, x] = ConsoleColor.White;
                }
            }
            Console.SetWindowSize(width, height + 1);//콘솔에 출력된 물자들이 밀리는 현상 방지를 위해 height + 1이라는데 한번 테스트겸 지워보는것도 이해를 아직 못함
            Console.CursorVisible = false;
            oneWidthChars['|'] = true;
            oneWidthChars['='] = true;
            oneWidthChars[' '] = true;
            oneWidthChars['`'] = true;
            oneWidthChars['~'] = true;
            oneWidthChars['!'] = true;
            oneWidthChars['@'] = true;
            oneWidthChars['#'] = true;
            oneWidthChars['$'] = true;
            oneWidthChars['%'] = true;
            oneWidthChars['^'] = true;
            oneWidthChars['&'] = true;
            oneWidthChars['*'] = true;
            oneWidthChars['('] = true;
            oneWidthChars[')'] = true;
            oneWidthChars['-'] = true;
            oneWidthChars['='] = true;
            oneWidthChars['\\'] = true;
            oneWidthChars['_'] = true;
            oneWidthChars['+'] = true;
            oneWidthChars['|'] = true;
            oneWidthChars['|'] = true;
            oneWidthChars['.'] = true;
            oneWidthChars[','] = true;
            oneWidthChars['/'] = true;
            oneWidthChars['\n'] = true;
            oneWidthChars['\r'] = true;
            oneWidthChars[' '] = true;
            oneWidthChars['A'] = true;
            oneWidthChars['A'] = true;
            for(int i = 0; i < 26; i++)
            {
                oneWidthChars[(char)('A' + (char)i)] = true;
                oneWidthChars[(char)('a' + (char)i)] = true;
            }

        }
        public static void ScreenInit(int X, int Y)
        {
            ConsoleBufferInit(X, Y);
        }

        // 백버퍼를 읽기전용으로 제공.
        public static char[,] Buffer => backBuffer;

        //콘솔 화면 범위 검사
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



        //문자를 백버퍼에 쓰는 메서드
        public static void Draw(char inChar, int inX, int inY)
        {
            if (!IsRangeOut(inX, inY))
                backBuffer[inY, inX] = inChar;
        }
        public static void Draw(char inChar, Vec2 v)
        {
            Draw(inChar, v.X, v.Y);
        }

        //문자열을 문자배열로 만들어 해당 위치에 인덱스마다 쓰는 메서드
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
                int y = inY + (inX + i) / width; //아무생각없이 inY랑 나머지연산으로 col을 구하려함 실수다 실수
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
        public static void ColorHorizontal(ConsoleColor c, Vec2 pos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!IsRangeOut(pos.X + i, pos.Y))
                {
                    backColorBuffer[pos.Y, pos.X + i] = c;
                }
            }
        }
        public static void ColorVertical(ConsoleColor c, Vec2 pos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!IsRangeOut(pos.X, pos.Y + i))
                {
                    backColorBuffer[pos.Y + i, pos.X] = c;
                }
            }
        }


        public static void DrawRelative(Actor actor, Vec2 playerPos)
        {
            for (int i = 0; i < actor.Size.X; i++)
            {
                for (int k = 0; k < actor.Size.Y; k++)
                {
                    //플레이어 중앙에 배치
                    Draw(actor.GetRenderChar(i, k), actor.GetPosition() - playerPos + Program.CENTER_OFFSET + new Vec2(i, k));
                }
            }
        }





        //백버퍼를 프론트 버퍼에 복사함
        //Array.Copy를통해 같은 타임의 배열이기 때문에 boxing이 이러어날 걱정없이 복사도니다.
        //잠깐 boxing이 뭐더라 값형식이 참조형식으로 박싱 변환되는것?
        private static void BufferExtraction()
        {
            Array.Copy(backBuffer, frontBuffer, width * height);//2차원배열을 1차원처럼?길이를?넘기면?전부된다?
        }
        //백버퍼를 비우는 메서드.
        public static void ScreenClear()
        {
            FillBuffer(' ');  // 선택한 위치부터 길이까지 클리어
        }
        //백버퍼를 채우는 메서드
        public static void FillBuffer(char c)
        {
            //그냥 2차원 초기화 
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    backBuffer[y, x] = c;
            //https://stackoverflow.com/questions/11672149/fill-a-multidimensional-array-with-same-values-c-sharp
            //안전하지 않지만 속도가 빠른 2차원 초기화 너무어렵다 이건 나중에
        }

        //프론트버퍼를 출력하는 메서드.
        //?하나씩?출력해도?되는건가? // 커서가 마구마구 움직임
        //오늘배운 stringBuilder 사용 훌륭하다

        
        private static void Print()
        {

            //두개짜리를 위해 하나짜리 두번출력했었는데 아시키아트떄매 두개짜리는 한칸을 않읽는방법으로 변경
            //버그발생 연달아 넓은게 넓은걸 안읽음
            //두칸짜리 출력시 예외 처리 해야할것 일단 출력을 하고 뒤의 버퍼들을 넓은 문자의 수들만큼 출력을 하지 않는다.
            //색 설정을 위해서는 입력 전 후에서 색 지정, 초기화가 필요해서 못씀 하지만 win api를 사용하면 해결가능
            StringBuilder result = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                bool jump = false;
                for (int x = 0; x < width; x++)
                {
                    //머리아픈 문제발생
                    //if (oneWidthChars.ContainsKey(frontBuffer[y,x]))
                    //{
                    //    //한칸짜린 출력
                    //    result.Append(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
                    //}
                    //else
                    //{
                    //    //두칸짜리일경우 뒤의한칸을 비워두고그 다음칸에 값을 넣었었지만 그럴경우
                    //    //연속된 두칸짜리일경우몇개가 출력이 안될 수 있음
                    //    if (jump)
                    //    {
                    //        jump = false;
                    //        continue;
                    //    }
                    //    result.Append(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
                    //    jump = true;
                    //    //jumponce
                    //}
                    result.Append(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
                }
                result.Append('\n');
            }
            Console.Write(result.ToString());

            //그냥 WINAPI써야겠다 컬러 설정을 못해버리겠네
            //이것도 느려!
            //foreach(char ch in result.ToString())
            //{
            //    Console.Write(ch);
            //}



            //넘모느리다 처음엔 빨랐는데 이상하네
            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        //if (frontColorBuffer[y, x] == null)
            //        //{
            //        //    //Console.ResetColor();
            //        //}
            //        //else
            //        //{
            //        //    Console.ForegroundColor = frontColorBuffer[y,x];
            //        //}
            //        if (frontBuffer[y, x] == ' ' || frontBuffer[y, x] == '-' || frontBuffer[y, x] == '|')
            //        {
            //            //한칸짜린 두번
            //            Console.Write(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
            //            Console.Write(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
            //        }
            //        else
            //        {
            //            Console.Write(frontBuffer[y, x]);//Console.Write(frontBuffer[y, x]);
            //        }
            //    }
            //    Console.Write('\n');
            //}
            ////Console.Write(result.ToString());

        }

        //화면에 보여주기 위한 메서드
        public static void Show()
        {
            BufferExtraction();
            Console.SetCursorPosition(0, 0);
            Print();
            ScreenClear();
        }

        public static void DrawFight()
        {
            FillBuffer(' ');
            DrawContinue("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ", 0, 20);
            Show();
        }

        public static void RenderUIContainer(UIContainerGrid mainUiContainer)
        {
            mainUiContainer.Draw();

            //foreach (var item in mainUiContainer.GetAllContent())
            //{
            //    if(item != null)
            //        item.Draw();
            //}
        }

        public static void SetColor(ConsoleColor consoleColor, int x, int y)
        {
            backColorBuffer[y, x] = consoleColor;
        }
        public static void ResetColor(int x, int y)
        {
            backColorBuffer[y, x] = ConsoleColor.White;
        }
    }
}