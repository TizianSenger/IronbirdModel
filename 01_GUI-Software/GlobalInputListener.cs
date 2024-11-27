using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EurofighterInformationCenter
{
    public class GlobalInputListener : IDisposable
    {
        InformationCenterMainPage eurofighterInformationCenterInstance;

        public delegate void InputDetectedHandler();
        public event InputDetectedHandler InputDetected;
        public void Dispose()
        {
            Stop(); // Oder was auch immer notwendig ist, um den Listener ordnungsgemäß zu schließen
        }

        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_TOUCH = 0x0241;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public GlobalInputListener()
        {
            _proc = HookCallback;
        }

        public void Start()
        {
            _hookID = SetHook(_proc);
        }

        public void Stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    // Hier wurde ein Mausklick erkannt
                    //MessageBox.Show("asdfasdf");
                    InputDetected?.Invoke();
                    //eurofighterInformationCenterInstance.machdas();
                }
                else if (wParam == (IntPtr)WM_TOUCH)
                {
                    // Hier wurde eine Toucheingabe erkannt
                    //MessageBox.Show("asdfasdf");
                    InputDetected?.Invoke();
                    //eurofighterInformationCenterInstance.machdas();
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
