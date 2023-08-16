using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CopyDataStruct;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace SmartFile
{
    public partial class SmartFileForm : Form
    {
        const int WM_COPYDATA = 0x004A;
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, IntPtr wParam, ref CPOYDATASTRUCT IParm);
       
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool ChangeWindowMessageFilter(uint msg, int flags);
        public SmartFileForm()
        {
            //this.Visible = false;
            //this.ShowInTaskbar = false;
            //this.Opacity = 0;
            InitializeComponent();

            ChangeWindowMessageFilter(WM_COPYDATA, 1);
            string[] args1 = new string[3];
            args1[0] = "send";
            args1[1] = "a.xls";
            args1[2] = "EL";
            //MessageBox.Show("");
            int findMainWindow = FindWindow(null, @"EngineerToolsForm-Rone");
            //MessageBox.Show(findMainWindow.ToString());
            if (findMainWindow == 0)
            {
                Process pr = new Process();
                pr.StartInfo.FileName = @"D:\Develop\EngineerTools\EngineerTools\bin\Debug\EngineerTools.exe";
                pr.Start();
                findMainWindow = (int)pr.Handle;

            }

            string messagetex = "";
            if (args1 != null)
            {
                foreach (string arg in args1)
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
                    int result = SendMessage(findMainWindow, WM_COPYDATA, this.Handle, ref cds);
                    MessageBox.Show(result.ToString());
                        
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                    



            }
        }
    }
}
