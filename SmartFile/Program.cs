using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CopyDataStruct;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SmartFile
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        const int WM_COPYDATA = 0x004A;
        [STAThread]
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, ref CPOYDATASTRUCT IParm);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool ChangeWindowMessageFilter(uint msg, int flags);

        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new SmartFileForm());
            MessageBox.Show(args[args.Length-2]);
            MessageBox.Show(args[args.Length-1]);
            ChangeWindowMessageFilter(WM_COPYDATA, 1);

            int findMainWindow = FindWindow(null, @"EngineerToolsForm-Rone");
            //MessageBox.Show(findMainWindow.ToString());
            if (findMainWindow == 0)
            {
                Process pr = new Process();
                pr.StartInfo.FileName = @"EngineerTools.exe";
                pr.Start();
                findMainWindow = (int)pr.Handle;

            }

            string messagetex = "";
            if (args != null)
            {
                foreach (string arg in args)
                {
                    messagetex = messagetex + arg + @"||";
                }
                byte[] sarr = System.Text.Encoding.Default.GetBytes(messagetex);
                int len = sarr.Length;
                CPOYDATASTRUCT cds;
                cds.dwData = (IntPtr)Convert.ToInt16("1");
                cds.cbData = len + 1;
                cds.lpData = messagetex;
                try
                {
                    int result = SendMessage(findMainWindow, WM_COPYDATA, 0, ref cds);
                    //MessageBox.Show(result.ToString());

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }




            }

        }
    }
}
