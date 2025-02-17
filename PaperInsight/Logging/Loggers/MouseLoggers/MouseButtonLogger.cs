using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using System;
using System.Runtime.InteropServices;

namespace PaperInsight.Logging.Loggers.MouseLoggers
{
    internal class MouseButtonLogger : AbstractLeafLogger<MouseButtonLoggingData>
    {
        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "Button",
            "X",
            "Y"
        }.ToCSVString();

        private bool _started = false;

        #region WindowsCrap
        // Windows API hook constants
        private const int WH_MOUSE_LL = 14;
        // Mouse event constants
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_XBUTTONDOWN = 0x020B;

        // Hook delegate and handle
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private readonly LowLevelMouseProc _hookCallback;
        private readonly IntPtr _hookHandle;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private struct POINT
        {
            public int x;
            public int y;
        }

        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (_started
                && nCode >= 0
                && (wParam == (IntPtr)WM_LBUTTONDOWN ||
                    wParam == (IntPtr)WM_RBUTTONDOWN ||
                    wParam == (IntPtr)WM_MBUTTONDOWN ||
                    wParam == (IntPtr)WM_XBUTTONDOWN))
            {
                string buttonEvent = wParam switch
                {
                    (IntPtr)WM_LBUTTONDOWN => "Left Button Down",
                    (IntPtr)WM_RBUTTONDOWN => "Right Button Down",
                    (IntPtr)WM_MBUTTONDOWN => "Middle Button Down",
                    (IntPtr)WM_XBUTTONDOWN => "X Button Down",
                    _ => "Unknown Button"
                };

                MSLLHOOKSTRUCT? hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                int x = hookStruct?.pt.x ?? -1;
                int y = hookStruct?.pt.y ?? -1;

                LoggingData.Enqueue(new()
                {
                    SystemTime = DateTime.Now,
                            Button = buttonEvent,
                            X = x,
                            Y = y
                });
                
            }
            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }

        #endregion

        public MouseButtonLogger()
        {
            _hookCallback = HookCallback;
            _hookHandle = SetWindowsHookEx(WH_MOUSE_LL, _hookCallback, IntPtr.Zero, 0);
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);
            _started = true;
        }
        public override void Stop()
        {
            base.Stop();
            _started = false;
            UnhookWindowsHookEx(_hookHandle);
        }
    }

}
