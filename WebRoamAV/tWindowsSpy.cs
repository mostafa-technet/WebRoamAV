using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebRoamAV
{
   
    public partial class tWindowsSpy : Form
    {
        private System.Windows.Window m_parent;
        public tWindowsSpy(System.Windows.Window parent)
        {
            InitializeComponent();
            m_parent = parent;
        }
        [DllImport("user32.dll", SetLastError = true)]

        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);



        [DllImport("user32.dll")]

        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")] static extern IntPtr SetCapture(long hWnd);



        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void pictureBox1_DragLeave(object sender, EventArgs e)
        {
           // MessageBox.Show(e.ToString());
        }
        private static int mousedown = 0;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {                
                //pictureBox1.DoDragDrop(pictureBox1.Image, DragDropEffects.None);
                Cursor.Current = new Cursor(new Uri(@"Resources/thumb_Drag_Maximized_Window_1.cur", UriKind.RelativeOrAbsolute).ToString());
                //  dele = WinEventProc;
                // IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);                
                MouseHook.MouseAction += MouseHook_MouseAction;
                MouseHook.Start(this);
                mousedown = 1;
            }
        }

        private void MouseHook_MouseAction(object sender, EventArgs e)
        {                       
            Cursor.Current = Cursors.Default;
            if (mousedown == 1)
            {
                MouseHook.MSLLHOOKSTRUCT s = (MouseHook.MSLLHOOKSTRUCT)sender;                               
                mousedown = 0;
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr h = MouseHook.GetWindowHandleFromPoint(s.pt.x, s.pt.y);
                if (GetWindowText(h, Buff, nChars) > 0)
                {
                    int processID = 0;

                    uint threadID = GetWindowThreadProcessId(h, out processID);

                    Process p = Process.GetProcessById(processID);
                    // MessageBox.Show(Buff.ToString() + Environment.NewLine + p.MainModule.FileName.ToString());
                    label5.Text = Buff.ToString();
                    MouseHook.MouseAction -= MouseHook_MouseAction;
                    MouseHook.stop();
                    new WindowsSpyDetails(p).Show(this);
                }
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
           // MessageBox.Show(e.Data.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_parent.Visibility = checkBox1.Checked ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        }

        private void tWindowsSpy_Load(object sender, EventArgs e)
        {
    
        }
    }

    public static class MouseHook

    {
        public static event EventHandler MouseAction = delegate { };

        static Form frm;
        public static void Start(Form f)
        {
            frm = f;
            _hookID = SetHook(_proc);


        }

        private const int WM_SYSCOMMAND = 0x112;
        private const int MOUSE_MOVE = 0xF012;
        public static void stop()
        {
            ReleaseCapture();
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {                
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(hookStruct, new EventArgs());
            }
            int nul = 0;
           // SendMessage(frm.Handle, WM_SYSCOMMAND, MOUSE_MOVE, ref nul);
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static IntPtr GetWindowHandleFromPoint(int x, int y)
        {
            var point = new Point(x, y);
            return WindowFromPoint(point);
        }
        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        static extern IntPtr SetCapture(long hWnd);
        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);


        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point p);


    }
}
