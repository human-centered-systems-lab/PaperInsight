using PaperInsight.Logging.Abstraction;
using PaperInsight.Logging.LoggingData;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace PaperInsight.Logging.Loggers.MouseLoggers
{
    internal class MouseMovementLogger : AbstractLeafLogger<MouseMovementLoggingData>
    {
        protected override string CSV_Header => new string[]
        {
            "SystemTime",
            "X",
            "Y"
        }.ToCSVString();

        #region WindowsCrap
        // Windows API hook constants
        private const int WH_MOUSE_LL = 14;

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
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT? hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                _lastMouseX = hookStruct?.pt.x ?? -1;
                _lastMouseY = hookStruct?.pt.y ?? -1;
            }
            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }
        #endregion

        private int _lastMouseX = -1;
        private int _lastMouseY = -1;

        internal const int SAMPLING_INTERVAL = 100;
        private readonly Timer _reapetSampling;

        public MouseMovementLogger()
        {
            _hookCallback = HookCallback;
            _hookHandle = SetWindowsHookEx(WH_MOUSE_LL, _hookCallback, IntPtr.Zero, 0);
            _reapetSampling = new Timer
            {
                AutoReset = true,
                Interval = SAMPLING_INTERVAL
            };
            _reapetSampling.Elapsed += SampleMouse;
        }

        public override void Start(DateTime currentTime, string userID, string path, string fileEnding)
        {
            base.Start(currentTime, userID, path, fileEnding);
            _reapetSampling.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _reapetSampling.Stop();
            UnhookWindowsHookEx(_hookHandle);
        }

        private void SampleMouse(object? sender, ElapsedEventArgs e)
        {
            LoggingData.Enqueue(new()
            {
                SystemTime = DateTime.Now,
                X = _lastMouseX,
                Y = _lastMouseY,
            });
        }
    }
}
