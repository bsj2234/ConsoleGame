using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

//https://stackoverflow.com/questions/6554536/is-it-possible-to-get-set-the-console-font-size
namespace ConsoleExtender
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConsoleFont
    {
        public uint Index;
        public short SizeX, SizeY;
    }

    public static class ConsoleHelper
    {
        [DllImport("kernel32")]
        public static extern bool SetConsoleIcon(IntPtr hIcon);

        [DllImport("kernel32")]
        private extern static bool SetConsoleFont(IntPtr hOutput, uint index);

        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        public static bool SetConsoleFont(uint index)
        {
            return SetConsoleFont(GetStdHandle(StdHandle.OutputHandle), index);
        }

        [DllImport("kernel32")]
        private static extern bool GetConsoleFontInfo(IntPtr hOutput, [MarshalAs(UnmanagedType.Bool)] bool bMaximize,
            uint count, [MarshalAs(UnmanagedType.LPArray), Out] ConsoleFont[] fonts);

        [DllImport("kernel32")]
        private static extern uint GetNumberOfConsoleFonts();

        public static uint ConsoleFontsCount
        {
            get
            {
                return GetNumberOfConsoleFonts();
            }
        }

        public static ConsoleFont[] ConsoleFonts
        {
            get
            {
                ConsoleFont[] fonts = new ConsoleFont[GetNumberOfConsoleFonts()];
                if (fonts.Length > 0)
                    GetConsoleFontInfo(GetStdHandle(StdHandle.OutputHandle), false, (uint)fonts.Length, fonts);
                return fonts;
            }
        }
        private const int FixedWidthTrueType = 54;
        private const int StandardOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


        private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
            public string FontName;
        }

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
        {
            Console.WriteLine("Set Current Font: " + font);

            FontInfo before = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };

            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {

                FontInfo set = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                {
                    var ex = Marshal.GetLastWin32Error();
                    Console.WriteLine("Set error " + ex);
                    throw new System.ComponentModel.Win32Exception(ex);
                }

                FontInfo after = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, set, after };
            }
            else
            {
                var er = Marshal.GetLastWin32Error();
                Console.WriteLine("Get error " + er);
                throw new System.ComponentModel.Win32Exception(er);
            }
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
            SetWindowPos(hWnd, IntPtr.Zero, -fudgeOffset, -fudgeOffset, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
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
        public static extern bool AdjustWindowRect(ref Rect lpRect, int dwStyle, bool bMenu);
        public static void SetWindowPosition(int offset)
        {
            IntPtr handle = GetConsoleWindow();

            Rect windowSize = new Rect();

            GetWindowRect(handle, ref windowSize);
            //SetWindowPos(handle, IntPtr.Zero, windowSize.Left, windowSize.Right, windowSize.Top, windowSize.Bottom, SWP_NOZORDER | SWP_NOACTIVATE);
            SetWindowPos(handle, IntPtr.Zero, windowSize.Left + offset, windowSize.Top + offset, windowSize.Right - windowSize.Left, windowSize.Bottom - windowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
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
            for (int i = 0; i < count * 2; i++)
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
