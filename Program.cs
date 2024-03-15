using MyBuffer;
using MyData;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ConsoleGameProject
{
    internal class Program
    {
        static Stack<ConsoleKey> consoleKeys = new Stack<ConsoleKey>();

        //static readonly Vec2 MapSize = new Vec2(110, 60);
        static readonly Vec2 MapSize = new Vec2(220, 60);
        //static readonly Vec2 MapSize = new Vec2( 55, 30 );
        public readonly static Vec2 CENTER_OFFSET = new Vec2(MapSize.X/2, MapSize.Y/2);

        public static List<Actor> AllActors = new List<Actor>();

        static InputState inputState = InputState.ADVENTURE;
        public static GameState gameState = GameState.ADVENTURE;

        [StructLayout(LayoutKind.Sequential)]
        public struct _COORD
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _CONSOLE_FONT_INFOEX
        {
            public ulong cbSize;
            public int nFont;
            public _COORD dwFontSize;
            public uint FontFamily;
            public uint FontWeight;
            public char[] FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SECURITY_ATTRIBUTES
        {
            int nLength;
            IntPtr lpSecurityDescriptor;
            bool bInheritHandle;
        }

        [DllImport("kernel32.dll")]
        public extern static int CreateConsoleScreenBuffer(int dwDesiredAccess, int dwShareMode, _SECURITY_ATTRIBUTES[] lpSecurityAttributes, int dwFlags, IntPtr[] lpScreenBufferData);

        [DllImport("kernel32.dll")]
        public extern static bool SetCurrentConsoleFontEx(int hConsoleOutput, bool bMaximumWindow, _COORD dwFontSize, uint FontFamily,uint FontWeight, char[] FaceName);

        [DllImport("kernel32.dll")]
        public extern static bool GetCurrentConsoleFontEx(int hConsoleOutput,bool bMaximumWindow,_CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);


        //static UIContainerGrid FightUIContainer = new UIContainerGrid(0,30);
        public static bool Blink = false;
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            RenderManager.ScreenInit(MapSize.X,MapSize.Y);
            //FightUIContatinerSetter.SetFightUI(FightUIContainer);
            Task inpuAsync = Task.Run(() => {
                while (true)
                {
                    if (Console.KeyAvailable == false)
                        continue;
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    consoleKeys.Push(keyInfo.Key);
                }
            });
            Task focusAsync = Task.Run(() => {
                while (true)
                {
                    Thread.Sleep(300);
                    Blink = !Blink;
                }
            });
            //https://gist.github.com/Shensd/01342e2f399de4dca2ca87b36059ba0a
            Task musicAsync = Task.Run(() => {
                while (true)
                {
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(293, 200);
                    Console.Beep(246, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(415, 200);
                    Console.Beep(415, 200);
                    Console.Beep(440, 200);
                    Console.Beep(493, 200);
                    Console.Beep(440, 200);
                    Console.Beep(440, 200);
                    Console.Beep(440, 200);
                    Console.Beep(329, 200);
                    Console.Beep(293, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(369, 200);
                    Console.Beep(329, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(293, 200);
                    Console.Beep(246, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(415, 200);
                    Console.Beep(415, 200);
                    Console.Beep(440, 200);
                    Console.Beep(493, 200);
                    Console.Beep(440, 200);
                    Console.Beep(440, 200);
                    Console.Beep(440, 200);
                    Console.Beep(329, 200);
                    Console.Beep(293, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(369, 200);
                    Console.Beep(329, 200);
                    Console.Beep(329, 200);
                    Console.Beep(369, 200);
                    Console.Beep(329, 200);
                }
            });

            Player player = new Player("siko",100, CENTER_OFFSET, new Vec2(1,1),false);
            //Wall wall1 = new Wall("wall", new Vec2(20, 20), new Vec2(10, 2), false);
            HuntArea huntarea = new HuntArea("huntarea", new Vec2(100, 33), new Vec2(10, 2));

            /////////////RenderManager.ConsoleBufferInit(MapSize);
            //CreateConsoleScreenBuffer(1<<8| 1<<7, 0, null, 1, null);
            //잠깐만 이거 한번만 실행되어야 하는데
            //UICursor UICursor = new UICursor(FightUIContainer);



            UIContainerGrid MainUiContainer = new UIContainerGrid(2, 1, true);
            //MainUiContainer.AddNewUI(null);

            var FightScene = new UIContainerGridContent( """
                 ...                                             .*(,, */#.       .(/,*,*#.         
              /**// (##(&      ****/,                           *///**((%%*     *////**/*/((@        
                  ((((#   (,(##((%/(@                         (//////(#*      */////////(&          
                 *% ((&  /##((((#&@                           (///////(#*      /*///////(#*          
               % (**,,,/*/(((&                                #///////((#      (////////(%           
     .((.   .(/(//*,,#@&///(*                                 #///////(#.     (///////(#            
     #**///(////////////////(/                                 #////((#(*/,,,*(//////((@            
       #(((/ .//////////////(@                                 .#(((*///,/*,/*(/////((@             
          . //// ,/   /////##@                                  ///**/////////////((((#((           
          ////// ,*///, *#/*,,//#@     *///(                   .////////////////////((((#,          
          ,.     (, *((///(((%(###%%   (*,//((#                .(////////////////////(((((&          
           ,/, ...,///##,,*/&#(* *(    ,#(((#*                /#////////////////////(((((((&         
               ((*//##@@@     ##  @@ #%              /*,*////(@(//////////  /*///((((///%#@         
                   #(((((                          (/.  .*////(#, ///////    /((((&((///*///#%%#    
                   %@@                             #////,,////(##((,.,,,,#(((,,,,,((@(((/////////((#
""",
                () => { },3,3
                );
            var FightGrid = new UIContainerGrid(1, 3);
            var FightGrid2 = new UIContainerGrid(3, 1);
            var FightGrid3 = new UIContainerGrid(3, 1);
            var FightGrid4 = new UIContainerGrid(3, 1);
            var FightGrid5 = new UIContainerGridContent(@"DiE
Ha
HaHa",()=> { });
            var FightGrid6 = new UIContainerGridContent("DiE", () => { });
            var FightGrid7 = new UIContainerGridContent("DiE", () => { });

            var StaticEnemy = new UIContainerGridContent("HP = 485", () => { });
            var StaticPlayer = new UIContainerGridContent(player.Hp, () => { });

            //Debug Hp Display In UI
            Task task = new Task(() => {
            player.Hp = player.Hp + 1;
                Thread.Sleep(00);
            });

            MainUiContainer.AddNewUI(FightScene);
            MainUiContainer.AddNewUI(FightGrid);
            FightGrid.AddNewUI(FightGrid2);
            FightGrid.AddNewUI(FightGrid3);
            FightGrid.AddNewUI(FightGrid4);
            FightGrid4.AddNewUI(FightGrid5);
            FightGrid4.AddNewUI(FightGrid6);
            FightGrid4.AddNewUI(FightGrid7);
            FightScene.AddNewUI(StaticEnemy);
            FightScene.AddNewUI(StaticPlayer);
            UICursor.InitialCursor(MainUiContainer);


            PositionConsoleWindowDemo.SetWindowSize();
            //ConsoleExtender.ConsoleHelper.SetCurrentFont("Consolas", 15);

            //game loop
            while (true)
            {
                switch (gameState)
                {
                    case GameState.ADVENTURE:
                        Adventure();
                        break;
                    case GameState.FIGHT:
                        Fight();
                        break;
                }
                RenderManager.Show();
            }



            void Adventure()
            {
                //render
                //RenderManager.ScreenFillWideChar('ㅇ');

                //RenderManager.OldTime = stopwatch.ElapsedMilliseconds; // 시간을 측정한다. 1초마다 갱신한다.
                //RenderManager.CurTime = stopwatch.ElapsedMilliseconds;

                foreach (Actor actor in AllActors)
                {
                    RenderManager.DrawRelative(actor, player.GetPosition());
                }
                AdventureInput();
            }
            void AdventureInput()
            {
                //input
                while (consoleKeys.Count != 0)
                {
                    ConsoleKey input = consoleKeys.Pop();
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            player.Move(Direction.UP);
                            break;
                        case ConsoleKey.DownArrow:
                            player.Move(Direction.DOWN);
                            break;
                        case ConsoleKey.LeftArrow:
                            player.Move(Direction.LEFT);
                            break;
                        case ConsoleKey.RightArrow:
                            player.Move(Direction.RIGHT);
                            break;
                    }
                }
                consoleKeys.Clear();
            }




            void Fight()
            {
                FightInput();
                RenderManager.RenderUIContainer(MainUiContainer);
                //UI[,] allUI = FightUIContainer.GetAll();


            }
            void FightInput()
            {
                //input
                while (consoleKeys.Count != 0)
                {
                    ConsoleKey input = consoleKeys.Pop();
                    switch (input)
                    {
                        case ConsoleKey.UpArrow:
                            UICursor.Move(Direction.UP);
                            break;
                        case ConsoleKey.DownArrow:
                            UICursor.Move(Direction.DOWN);
                            break;
                        case ConsoleKey.LeftArrow:
                            UICursor.Move(Direction.LEFT);
                            break;
                        case ConsoleKey.RightArrow:
                            UICursor.Move(Direction.RIGHT);
                            break;
                        case ConsoleKey.Enter:
                            UICursor.Click();
                            break;
                        case ConsoleKey.Escape:
                            UICursor.Escape();
                            break;
                    }
                }
                consoleKeys.Clear();
            }
        }//endOfMain
        public static void ShakeScreen(int count)
        {
            Task ShakeAsync = Task.Run(() => {
                    PositionConsoleWindowDemo.ShakeWindow(3, 20, 50);
            });
        }

        
    }
    public static class PositionConsoleWindowDemo
    {
        //https://stackoverflow.com/questions/27715004/position-a-small-console-window-to-the-bottom-left-of-the-screen
        // P/Invoke declarations.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        const int MONITOR_DEFAULTTOPRIMARY = 1;

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [StructLayout(LayoutKind.Sequential)]
        struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            public static MONITORINFO Default
            {
                get { var inst = new MONITORINFO(); inst.cbSize = (uint)Marshal.SizeOf(inst); return inst; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int x, y;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        const uint SW_RESTORE = 9;

        [StructLayout(LayoutKind.Sequential)]
        struct WINDOWPLACEMENT
        {
            public uint Length;
            public uint Flags;
            public uint ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    var instance = new WINDOWPLACEMENT();
                    instance.Length = (uint)Marshal.SizeOf(instance);
                    return instance;
                }
            }
        }


        public static void SetWindowSize()
        {
            // Get this console window's hWnd (window handle).
            IntPtr hWnd = GetConsoleWindow();

            // Get information about the monitor (display) that the window is (mostly) displayed on.
            // The .rcWork field contains the monitor's work area, i.e., the usable space excluding
            // the taskbar (and "application desktop toolbars" - see https://msdn.microsoft.com/en-us/library/windows/desktop/ms724947(v=vs.85).aspx)
            var mi = MONITORINFO.Default;
            GetMonitorInfo(MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY), ref mi);

            // Get information about this window's current placement.
            var wp = WINDOWPLACEMENT.Default;
            GetWindowPlacement(hWnd, ref wp);
            var newRect = new Rect();
            GetWindowRect(hWnd, ref newRect);
            // Calculate the window's new position: lower left corner.
            // !! Inexplicably, on W10, work-area coordinates (0,0) appear to be (7,7) pixels 
            // !! away from the true edge of the screen / taskbar.
            int fudgeOffset = 7;
            /*wp.NormalPosition = new RECT()
            {
                Left = -fudgeOffset,
                Top = mi.rcWork.Bottom - (wp.NormalPosition.Bottom - wp.NormalPosition.Top),
                Right = (wp.NormalPosition.Right - wp.NormalPosition.Left),
                Bottom = fudgeOffset + mi.rcWork.Bottom
            };
            
            // Place the window at the new position.
            SetWindowPlacement(hWnd, ref wp);*/

            SetInitailWindowSize();
            SetWindowPos(hWnd, IntPtr.Zero,-fudgeOffset, -fudgeOffset, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
            SetInitailWindowSize();

        }

        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;


        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        /// 

        [DllImport("user32.dll")]
        public static extern bool AdjustWindowRect(ref Rect lpRect,int dwStyle,bool bMenu);
        public static void SetWindowPosition(int offset)
        {
            IntPtr handle = GetConsoleWindow();

            Rect windowSize = new Rect();

            GetWindowRect(handle, ref windowSize);
            //SetWindowPos(handle, IntPtr.Zero, windowSize.Left, windowSize.Right, windowSize.Top, windowSize.Bottom, SWP_NOZORDER | SWP_NOACTIVATE);
            SetWindowPos(handle, IntPtr.Zero, windowSize.Left + offset, windowSize.Top + offset, windowSize.Right - windowSize.Left , windowSize.Bottom - windowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        private static Rect InitialWindowSize;
        public static void SetInitailWindowSize()
        {
            IntPtr handle = GetConsoleWindow();
            GetWindowRect(handle, ref InitialWindowSize);
        }
        static bool shake = false;
        public static void ShakeWindow(int count, int offset, int time)
        {
            IntPtr handle = GetConsoleWindow();
            for (int i = 0; i < count*2; i++)
            {
                if (shake)
                {
                    SetWindowPos(handle, IntPtr.Zero, InitialWindowSize.Left + offset, InitialWindowSize.Top - offset, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
                }
                else
                {
                    SetWindowPos(handle, IntPtr.Zero, InitialWindowSize.Left, InitialWindowSize.Top, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
                }
                shake = !shake;
                Thread.Sleep(time);
            }
            ResetWindow();
        }
        public static void ResetWindow()
        {
            int offset = 10;
            IntPtr handle = GetConsoleWindow();
            SetWindowPos(handle, IntPtr.Zero, InitialWindowSize.Left, InitialWindowSize.Top, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
        }

    }
}
