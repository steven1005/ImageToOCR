using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageToOCR
{
    class HookFunction
    {
        #region Windows API
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool InvalidateRect(IntPtr hWnd, Rectangle lpRect, bool bErase);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }
        #endregion

        #region Keyboard
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        #endregion

        #region Mouse
        private const int WH_MOUSE_LL = 14;

        private const int WM_MOUSEMOVE = 0x0200;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_LBUTTONDBLCLK = 0x0203;

        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;

        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_MBUTTONDBLCLK = 0x0209;
        #endregion
        #region Set Variable
        public static bool _isDrawRect = false;
        public static bool _isRectBitmapCopyToClip = false;
        #endregion
        private static bool _isSelecting = false;
        private static LowLevelMouseProc _procMouse = HookCallMouseback;
        private static LowLevelKeyboardProc _procKeyboard = HookCallKeyboardback;
        private static IntPtr _hookIDMouse = IntPtr.Zero;
        private static IntPtr _hookIDKeyboard = IntPtr.Zero;

        #region Get Variable
        public static Point _MouseLeftStartPoint;
        public static Point _MouseLeftEndPoint;
        public static bool bMouseLeftDown = false;

        public static Point _MouseRightStartPoint;
        public static Point _MouseRightEndPoint;
        public static bool bMouseRightDown = false;

        public static Point _MouseCurrentPoint;

        public static Keys _KeyDown;
        public static Keys _KeyUp;
        public static Bitmap bmpRect;
        #endregion
        private static Rectangle _previousRect = Rectangle.Empty;

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        /// Usage 
        /*
            Set Variable：
            HookFunction hf;
            hf = new HookFunction();
            HookFunction._isDrawRect = true;
            HookFunction._isRectBitmapCopyToClip = true;

            Get Variable：
            tbMousePosition.Text = HookFunction._MouseCurrentPoint.ToString();

            ckbLeftClick.Checked = HookFunction.bMouseLeftDown;

            ckbRightClick.Checked = HookFunction.bMouseRightDown;

            tbLeftDownPosition.Text = HookFunction._MouseLeftStartPoint.ToString();
            tbLeftUpPosition.Text = HookFunction._MouseLeftEndPoint.ToString();
                
            tbRightDownPosition.Text = HookFunction._MouseRightStartPoint.ToString();
            tbRightUpPosition.Text = HookFunction._MouseRightEndPoint.ToString();

            tbKeyDown.Text = HookFunction._KeyDown.ToString();
            tbKeyUp.Text = HookFunction._KeyUp.ToString();
            
            Bitmap bmpReturn = null;
            if (HookFunction._isRectBitmapCopyToClip)
            {
                bmpReturn = HookFunction.bmpRect;
            }
        */
        /// 
        public HookFunction()
        {
            _hookIDMouse = SetHookMouse(_procMouse);
            _hookIDKeyboard = SetHookKeyboard(_procKeyboard);
        }
        ~HookFunction()
        {
            UnhookWindowsHookEx(_hookIDMouse);
            UnhookWindowsHookEx(_hookIDKeyboard);
        }

        private IntPtr SetHookMouse(LowLevelMouseProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr SetHookKeyboard(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static IntPtr HookCallKeyboardback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    //Console.WriteLine((Keys)vkCode);
                    _KeyDown = (Keys)vkCode;
                }
                if (wParam == (IntPtr)WM_KEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    //Console.WriteLine((Keys)vkCode);
                    _KeyUp = (Keys)vkCode;
                }
            }
            return CallNextHookEx(_hookIDKeyboard, nCode, wParam, lParam);
        }

        private static IntPtr HookCallMouseback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                #region left
                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    // 鼠标按下时记录起点
                    _isSelecting = true;
                    bMouseLeftDown = true;
                    _MouseLeftStartPoint = Cursor.Position;
                }
                #region Move
                else if (wParam == (IntPtr)WM_MOUSEMOVE && _isSelecting)
                {
                    _MouseCurrentPoint = Cursor.Position;
                    if (_isDrawRect)
                    {
                        DrawSelectionRectangle();
                    }
                }
                #endregion
                else if (wParam == (IntPtr)WM_LBUTTONUP && _isSelecting)
                {
                    // 鼠标释放时记录终点
                    _isSelecting = false;
                    bMouseLeftDown = false;
                    _MouseCurrentPoint = Cursor.Position;
                    DrawSelectionRectangle(); // 清除最后绘制的矩形框
                    _MouseLeftEndPoint = Cursor.Position;
                    if (_isRectBitmapCopyToClip)
                    {
                        bmpRect = CaptureRectToBitmap(_MouseLeftStartPoint, _MouseLeftEndPoint);
                    }
                }
                #endregion
                #region Right
                if (wParam == (IntPtr)WM_RBUTTONDOWN)
                {
                    // 鼠标按下时记录起点
                    bMouseRightDown = true;
                    _MouseRightStartPoint = Cursor.Position;
                }
                else if (wParam == (IntPtr)WM_RBUTTONUP)
                {
                    // 鼠标释放时记录终点
                    bMouseRightDown = false;
                    _MouseRightEndPoint = Cursor.Position;
                }
                #endregion
            }

            return CallNextHookEx(_hookIDMouse, nCode, wParam, lParam);
        }

        private static void DrawSelectionRectangle()
        {
            //清除之前的矩形框
            //if (_previousRect != Rectangle.Empty)
            //{
            //    ControlPaint.DrawReversibleFrame(_previousRect, Color.Red, FrameStyle.Thick);
            //}

            int x = Math.Min(_MouseLeftStartPoint.X, _MouseCurrentPoint.X);
            int y = Math.Min(_MouseLeftStartPoint.Y, _MouseCurrentPoint.Y);
            int width = Math.Abs(_MouseLeftStartPoint.X - _MouseCurrentPoint.X);
            int height = Math.Abs(_MouseLeftStartPoint.Y - _MouseCurrentPoint.Y);

            _previousRect = new Rectangle(x, y, width, height);
            if (_previousRect != Rectangle.Empty)
            {
                ControlPaint.DrawReversibleFrame(_previousRect, Color.Red, FrameStyle.Thick);
            }
        }

        private static Bitmap CaptureRectToBitmap(Point startPoint, Point endPoint)
        {
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(startPoint.X - endPoint.X);
            int height = Math.Abs(startPoint.Y - endPoint.Y);

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            Rectangle rect = new Rectangle(x, y, width, height);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size);
            }
            Clipboard.SetImage((Image)bmp);
            return bmp;
            //string filePath = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            //bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            //MessageBox.Show($"Screenshot saved to: {filePath}", "Screenshot Captured", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
