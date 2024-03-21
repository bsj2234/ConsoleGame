using System.Runtime.InteropServices;

//https://stackoverflow.com/questions/6554536/is-it-possible-to-get-set-the-console-font-size
namespace ConsoleExtenderNs
{
    public static class ConsoleExtender
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

        public static void SetWindowPosition(int offset)
        {
            IntPtr handle = GetConsoleWindow();

            Rect windowSize = new Rect();

            GetWindowRect(handle, ref windowSize);
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
            IntPtr handle = GetConsoleWindow();
            SetWindowPos(handle, IntPtr.Zero, InitialWindowSize.Left, InitialWindowSize.Top, InitialWindowSize.Right - InitialWindowSize.Left, InitialWindowSize.Bottom - InitialWindowSize.Top, SWP_NOZORDER | SWP_NOACTIVATE);
        }
        public static void ShakeScreen(int count)
        {
            Task ShakeAsync = Task.Run(() =>
            {
                ConsoleExtender.ShakeWindow(3, 20, 50);
            });
        }

    }
}
