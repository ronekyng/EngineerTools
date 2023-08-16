using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Collections;
using System.Net;
using System.Threading;
//using Outlook=Microsoft.Office.Interop.Outlook;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Excel = Microsoft.Office.Interop.Excel;


namespace EngineerTools
{

    delegate void SetTextCallback(string text);
    public partial class MainForm : Form
    {
      
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();


        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd,GetWindowCmd uCmd);


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern IntPtr GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd,int msg,int wParam, StringBuilder iParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wParam, String iParam);
        [DllImport("user32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam); 

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int iParam);
        [DllImport("user32.dll")]
        public static extern bool ChangeWindowMessageFilter(uint msg, int flags);
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        public static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(IntPtr hwnd, string text);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd,
            out uint dwProcessId);

        enum GetWindowCmd :uint
        {
            GW_HWNMFIRST = 0,//为源子窗口寻找第一个兄弟窗口或者第一个顶级窗口
            GW_HWNMLAST = 1,//为源子窗口寻找最后一个兄弟窗口或者最后一个顶级窗口
            GW_HWNMNEXT = 2,//为源子窗口寻找下一个兄弟窗口
            GW_HWNMPREV = 3,//为源子窗口寻找上一个兄弟窗口
            GW_OWNER = 4,//寻找窗口的所有者
            GW_CHILD = 5,//为源子窗口寻找第一个子窗口
            GW_ENABLEDPOPUP = 5,
        }
        enum SendMessageID :uint
        {
            WM_SETTEXT = 0x000C,
            WM_GETTEXT=0x000D,
            WM_COPYDATA=0x004A,//定义消息
            WM_KEYDOWN = 0x100,//定义消息
            WM_KEYUP = 0x101,//定义消息
            WM_CLICK=0x00F5,//定义消息
        }
        enum KeyCode : uint
        {
            ENTER=0x0d,
        }
        enum MouseCode : uint
        {
            MOUSEEVENTF_MOVE=0x0001,
            MOUSEEVENTF_LEFTDOWN=0x0002,
            MOUSEEVENTF_LEFTUP=00004,
            MOUSEEVENTF_RIGHTDOWN=0x0008,
            MOUSEEVENTF_RIGHTUP=0x0010,
            MOUSEEVENTF_MIDDLEDOWN=0x0020,
            MOUSEEVENTF_MIDDLEUP=0x0040,
            MOUSEEVENTF_ABSOLUTE=0x8000,
        }

        const int WM_COPYDATA=0x004A;//定义消息
        const int WM_SETTEXT = 0x000C;
        const int SW_FORCEMINIMIZE = 11;
        const int SW_HIDE = 0;
        const int SW_MAXIMIZE = 3;
        const int SW_MINIMIZE = 6;
        const int SW_RESTORE = 9;
        const int SW_SHOW = 5;
        //……nCmdShow参数的其他值省略，详情请查看百度百科

        public const uint LVM_FIRST = 0x1000;
        public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const uint LVM_GETITEMW = LVM_FIRST + 75;
        public const uint PROCESS_VM_OPERATION = 0x0008;
        public const uint PROCESS_VM_READ = 0x0010;
        public const uint PROCESS_VM_WRITE = 0x0020;
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess,
            bool bInheritHandle, uint dwProcessId);
        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_RELEASE = 0x8000;

        public const uint MEM_RESERVE = 0x2000;
        public const uint PAGE_READWRITE = 4;

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
           uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr pszText; // string 
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;
        }
        public int LVIF_TEXT = 0x0001;

        ContextMenu notifyContextMenu = new ContextMenu();

        FileSystemWatcher fileWatcher = new FileSystemWatcher();    //设置文件监控
        System.Timers.Timer ewTimer = new System.Timers.Timer();    //浏览器监控定时器    
        List<Project> MyProjectList = new List<Project>();
        List<Project> AllProjectList = new List<Project>();
        Project currentProject = new Project();
        Unit currentUnit = new Unit();
        public static Person My;
        List<Person> PersonList = new List<Person>();
        
        private ArrayList openProjectList=new ArrayList();
        public MainForm()
        {
            InitializeComponent();

            ChangeWindowMessageFilter(WM_COPYDATA, 1);
            //设置监控目录
            SetWatcher(@"E:\", "*.*");
            //SetWatcher(@"E:\test", "*.*");

            //Setting.Setting();
            My = new Person(System.Environment.UserName);

            //设置项目列表ListView标题行
            projectListView.Columns.Add("编号", 50, HorizontalAlignment.Left);
            projectListView.Columns.Add("项目号", 80, HorizontalAlignment.Left);
            projectListView.Columns.Add("阶段", 50, HorizontalAlignment.Left);
            projectListView.Columns.Add("项目名称", 200, HorizontalAlignment.Left);
            projectListView.FullRowSelect = true;

            listProjectWindow();
            AllProjectList = Project.GetAllProjectList();
            MyProjectList = Project.GetOnesProjectsList(AllProjectList,  My);
            ShowMyProjectListTreeView();
            ShowMyProjectListView();
            WatchExplorerStart();
            ShowAllPersonList();
            ShowAllProject();
            ShowFileTree();

            MainNotifyIcon.Visible = true;
            this.ShowInTaskbar = true;
        }
        public void setMessgeTextBox(string text)
        {
            if(this.messageTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setMessgeTextBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.messageTextBox.Text = text;
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {

            saveLog();
        }

        // 托盘提示
        private void MainForm_Load(object sender, EventArgs e)
        {
            //设置消息过滤
            ChangeWindowMessageFilter(WM_COPYDATA, 1);
            this.MainNotifyIcon.Text = "文件监控小工具";
        }
 
        // 隐藏任务栏图标、显示托盘图标
        private void MainForm_ReSize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideMainWindow();
            }
        }
       
        private void clearButton_Click(object sender, EventArgs e)
        {
            setMessgeTextBox("");
        }

        private void closeButtom_Click(object sender, EventArgs e)
        {
            SettingForm setForm = new SettingForm();
            setForm.Show();
            //Close();
        }

        //重载 禁用关闭
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND&&(int)m.WParam==SC_CLOSE)
            {
                return;
            }
            base.WndProc(ref m);
        }
        //双击系统托盘
        private void mainNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChangeMainWindowStateFromNotifyIcon();

        }

        private void ChangeMainWindowStateFromNotifyIcon()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowMainWindow();
            }
            else
            {
                HideMainWindow();
            }
        }
        private void ShowMainWindow()
        {

                this.Show();
                this.WindowState = FormWindowState.Normal;
                MainNotifyIcon.Visible = true;
                this.ShowInTaskbar = true;
                ShowMainNotifyIconStripMenuItem.Text = global::EngineerTools.Properties.Resources.HideMainNotifyIconStripMenuItem;
        }
        private void HideMainWindow()
        {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
                MainNotifyIcon.Visible = true;
                this.ShowInTaskbar = true;
                ShowMainNotifyIconStripMenuItem.Text = global::EngineerTools.Properties.Resources.ShowMainNotifyIconStripMenuItem;
        }


        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            closeForm();
        }

        private void saveLog()
        {
            string wStr;
            wStr = readLog();
            wStr = wStr + this.messageTextBox.Text;
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\log.txt", false);
            sw.WriteLine(wStr);
            sw.Close();
            
        }
        private string readLog()
        {
            string rStr;
            StreamReader sr = new StreamReader(Application.StartupPath + "\\log.txt", false);
            rStr = sr.ReadLine().ToString();
            sr.Close();
            return rStr;
        }
        private void closeForm()
        {
            //watcher.Changed -= 
            MainNotifyIcon.Visible = false;
            Close();
        }

        private void test()
        {
            IntPtr desktopPtr,wndPtr;

            desktopPtr = GetDesktopWindow();
            wndPtr = FindWindow(null, "公司名称");
            int rtxProcessID;
            GetWindowThreadProcessId(wndPtr,out rtxProcessID);

            messageTextBox.Text += "\r\n"+ rtxProcessID.ToString();

            StringBuilder wName = new StringBuilder(256);
            GetClassName(wndPtr, wName, 256);
            //MessageBox.Show(wName.GetType().ToString());
            this.messageTextBox.Text += "\r\n" + wName.ToString();
            //wndPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);
            //MessageBox.Show(wndPtr.ToString());
            //wndPtr = FindWindow("#32770", null);
            string clName = "#32770";
            wndPtr = IntPtr.Zero;
            do
            {
                wndPtr = FindWindowEx(IntPtr.Zero, wndPtr, clName, null);
                int wndProcessId;
                GetWindowThreadProcessId(wndPtr, out wndProcessId);
                if (wndProcessId==rtxProcessID)
                {
                    GetWindowText(wndPtr, wName, 256);
                    string wNameStr=wName.ToString();
                    messageTextBox.Text += "\r\n"+ wNameStr;

                    if(wNameStr.IndexOf("RTX 会话")!=-1)
                    {
                        MessageBox.Show(wNameStr);
                        IntPtr editWnd,editWndFather;
                        editWndFather = FindWindowEx(wndPtr, IntPtr.Zero, "Static", null);
                        editWnd = FindWindowEx(editWndFather, IntPtr.Zero, "RichEdit20W", null);
                        StringBuilder chatText=new StringBuilder(1024);
                        SendMessage(editWnd, 0x00D, 2048, chatText);
                        //MessageBox.Show(chatText.ToString());
                        
                    }
                }

            } while (wndPtr != IntPtr.Zero);

            //FindWindowEx(IntPtr.Zero, wndPtr, "RichEdit20W", null);
        }
        /*
        private void MailTo(string name)
        {
            try
            {
                //mailm
            MessageBox.Show(name);
                Outlook.Application outlookApp = new Outlook.Application();
                //Outlook.NameSpace myOutlookNameSpace = outlookApp.GetNamespace("MAPI");
                Outlook.MailItem mailItem = outlookApp.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                mailItem.To = "PSMail1@github.com";
                mailItem.Subject = "测试";
                mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                string content = "请查收";
                //mailItem.Attachments.Add("");
                mailItem.HTMLBody = content;
                //((Outlook._MailItem)mailItem).Send();


            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show(e.Message);
                MessageBox.Show(e.HResult.ToString());
            }
        }
        private void OpenOutlookNewEmailDialog(string name)
        {
            try
            {
                //MessageBox.Show(name);
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.NameSpace myOutlookNameSpace = outlookApp.GetNamespace("MAPI");
                Outlook.MailItem mailItem = outlookApp.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                mailItem.To = name+"@github.com";
                //mailItem.Sent += new System.EventHandler();
                mailItem.Display();
                //((Outlook._MailItem)mailItem).Send();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                MessageBox.Show(e.Message);
                MessageBox.Show(e.HResult.ToString());
            }
        }
        private void OnSendMail()
        {

        }
        */
        private void OpenRtxDialog(string rtxName)
        {
            string rtxMsg="";
            OpenRtxDialog(rtxName, rtxMsg);
        }
        private void OpenRtxDialog(string rtxName,string rtxMsg)
        {
            IntPtr desktopPtr, wndRtxPtr;

            desktopPtr = GetDesktopWindow();
            wndRtxPtr = FindWindow(null, "公司名称");
            if(wndRtxPtr==IntPtr.Zero)
            {
                return;
            }
            ShowWindow(wndRtxPtr,SW_SHOW);
            int rtxProcessID;
            GetWindowThreadProcessId(wndRtxPtr, out rtxProcessID);
             
            IntPtr  rtxFindUserPtrWnd=IntPtr.Zero;
            do
            {
                rtxFindUserPtrWnd = FindWindowEx(wndRtxPtr, rtxFindUserPtrWnd, "#32770", null);
                if (IsWindowVisible(rtxFindUserPtrWnd))
                {
                    IntPtr rtxFindUserEditWnd;
                    //MessageBox.Show(name);
                    rtxFindUserEditWnd = FindWindowEx(rtxFindUserPtrWnd, IntPtr.Zero, "Edit", null);
                    if(rtxFindUserEditWnd!=IntPtr.Zero)
                    {
                        SendMessage(rtxFindUserEditWnd, (int)SendMessageID.WM_SETTEXT, 0, ""); 
                        SendMessage(rtxFindUserEditWnd, (int)SendMessageID.WM_SETTEXT, 0, rtxName);         
                        IntPtr rtxFindUserEditResultWnd = FindWindowEx(rtxFindUserPtrWnd, rtxFindUserEditWnd, "Button", null);
                        SendMessage(rtxFindUserEditResultWnd, (int)SendMessageID.WM_CLICK, 0, 0);
                        SendMessage(rtxFindUserEditResultWnd, (int)SendMessageID.WM_CLICK, 0, 0);

                        SendMessage(rtxFindUserEditWnd, (int)SendMessageID.WM_SETTEXT, 0, ""); 
                        rtxFindUserPtrWnd = IntPtr.Zero;
                    }
                }
            }
            while (rtxFindUserPtrWnd != IntPtr.Zero);

            IntPtr rtxDialogWnd;
            rtxDialogWnd = IntPtr.Zero;
            do
            {
                rtxDialogWnd = FindWindowEx(IntPtr.Zero, rtxDialogWnd, "#32770", null);
                StringBuilder wName = new StringBuilder(256);
                GetWindowText(rtxDialogWnd, wName, 256);
                string wNameStr = wName.ToString();

                if (wNameStr.Contains("RTX 会话"))
                {
                    IntPtr rtxMessageEditWnd;
                    rtxMessageEditWnd = FindWindowEx(rtxDialogWnd, IntPtr.Zero, "RichEdit20W", null);
                    SendMessage(rtxMessageEditWnd, (int)SendMessageID.WM_SETTEXT, 0, rtxMsg);
                    rtxDialogWnd = IntPtr.Zero;
                }

            } while (rtxDialogWnd != IntPtr.Zero);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            test();
        }

        private bool listProjectWindow()
        {
            IntPtr wndPtr, lastWndPtr,subWndPtr;
            wndPtr = IntPtr.Zero;
            lastWndPtr=IntPtr.Zero ;
            subWndPtr = IntPtr.Zero;
            if(MyProjectList!=null)
            {
                foreach(Project proj in MyProjectList)
                {
                    proj.Working = false;
                }
            }
            //projectListView.BeginUpdate();
            do
            {
                try
                {
                    wndPtr = FindWindowEx(IntPtr.Zero, lastWndPtr, "CabinetWClass", null);
                    lastWndPtr = wndPtr;
                    if(wndPtr !=IntPtr.Zero)
                    {
                        
                        subWndPtr = FindWindowEx(wndPtr, IntPtr.Zero, "WorkerW", null);
                        subWndPtr = FindWindowEx(subWndPtr, IntPtr.Zero, "ReBarWindow32", null);
                        subWndPtr = FindWindowEx(subWndPtr, IntPtr.Zero, "Address Band Root", null);
                        subWndPtr = FindWindowEx(subWndPtr, IntPtr.Zero, "msctls_progress32", null);
                        subWndPtr = FindWindowEx(subWndPtr, IntPtr.Zero, "Breadcrumb Parent", null);
                        subWndPtr = FindWindowEx(subWndPtr, IntPtr.Zero, "ToolbarWindow32", null);
                        StringBuilder strAddress = new StringBuilder(1024);
                        SendMessage(subWndPtr, 0x00D, 2048, strAddress);
                        //MessageBox.Show(strAddress.ToString());
                        if (strAddress.ToString().Contains(SettingFormController.projectRootFolder))
                        {
                            string str, projectCode,projNo,projStage;
                            string[] substr;
                            str = strAddress.ToString().Substring(("地址:" + SettingFormController.projectRootFolder).Length + 1);
                            substr = str.Split(new char[]{'\\',' '} );

                            projNo = substr[1].Substring(0, substr[1].Length - 1);
                            projStage = substr[1].Substring(substr[1].Length - 1, 1);
                            projectCode = projNo +"-"+ projStage;
                            Project findProj= MyProjectList.Find(c => c.Code.Equals(projectCode));
                            if (findProj!=null)
                            {
                                String unitcode;
                                findProj.Working = true;
                                //MessageBox.Show(findProj.Name);
                                foreach(Unit munit in findProj.Units)
                                {
                                    munit.Working = "";
                                }
                                
                                if(substr[3]!=null)
                                {
                                    unitcode = substr[3];
                                    Unit findUnit = findProj.Units.Find(c => c.Code.Equals(unitcode));
                                    if(findUnit!=null)
                                    {
                                        findUnit.Working = "working";
                                    }
                                }
                            }
                            //MessageBox.Show(projectCode);
                        }
                    }

                }
                catch
                {
                    return false;
                }

            }
            //while (wndPtr.ToInt32() != 0);
            while (wndPtr!= IntPtr.Zero) ;

            return true;
           
        }

        public static OleDbConnection getAccessConn(String mdbName)
        {
            string connstr = "Provider=Microsoft.Jet.OlEDB.4.0;Data Source="+mdbName+";";
            OleDbConnection tempconn = new OleDbConnection(connstr);
            return (tempconn);
        }
        public void ShowMyProjectListView()
        {
            projectListView.Items.Clear();
            projectListView.BeginUpdate();
            projectListView.EndUpdate();
            int i = 1;
            foreach(Project proj in MyProjectList)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                lvi.Text = i.ToString();
                lvi.SubItems.Add(proj.No);
                lvi.SubItems.Add(proj.Stage.Code);
                lvi.SubItems.Add(proj.Constructure);
                projectListView.Items.Add(lvi);
                i++;
            }
        }
        public void ShowMyProjectListTreeView()
        {
            MyProjectsListTreeView.Nodes.Clear();
            foreach (Project project in MyProjectList)
            {
                TreeNode projTn = new TreeNode();
                projTn.Text = project.Work.Dir+"|"+project.Code + project.Constructure + project.Name;
                projTn.Name = project.Name;
                projTn.Tag = project;

                //MessageBox.Show( project.Units[0].Owner.Code);
                
                List<Unit> OnesUnits = project.GetOnesUnits(My);
                foreach (Unit unit in OnesUnits)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = unit.Code + unit.Name;
                    tn.Name = unit.Code;
                    tn.Tag = unit;
                    //MessageBox.Show(unit.Owner.Work.Dir);
                    projTn.Nodes.Add(tn);
                }
                MyProjectsListTreeView.Nodes.Add(projTn);
            }
            
        }
        private void ProjectRootInitialButton_Click(object sender, EventArgs e)
        //初始化工作目录，将目录内所有文件登入数据库，并为每个文件生成hash值
        {
            long fileid;                 //新插入文件的ID
            string filehash;            //文件哈希值
            string strSQL;
            OleDbCommand myCommand;

            //判断是否初始化过
            if (SettingFormController.ProgramInitialed == "False")
            {
                DirectoryInfo di = new DirectoryInfo(SettingFormController.projectRootFolder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fiA = di.GetFiles("*.*", SearchOption.AllDirectories);
                OleDbConnection conn = getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
                conn.Open();
                foreach (FileInfo fi in fiA)
                {
                    //获取文件哈希
                    filehash = ProjectFileStream.SHA1File(fi.FullName);
                    //文件名查重
                    string fifullname=fi.FullName;
                    string fipath=fifullname.Replace(fi.Name,"");
                    strSQL = "SELECT * FROM FILE WHERE FileName='" + fi.Name + "' AND Path='" + fi.FullName + "'";
                    myCommand = new OleDbCommand(strSQL, conn);
                    long existfileid =Convert.ToInt64( myCommand.ExecuteScalar());
                    if(existfileid>0)
                    {
                        strSQL = "SELECT FileHash.Hash FROM FileHash WHERE FileID=" + existfileid;
                        myCommand = new OleDbCommand(strSQL, conn);
                        string existfilehash = myCommand.ExecuteScalar().ToString();
                        if (existfilehash != filehash)
                        {
                            strSQL = "INSERT INTO  FileHash(FileID,Hash) VALUES (" + existfileid + ",'" + filehash + "')";
                            //setMessgeTextBox(strSQL);
                            OleDbCommand myCommand2 = new OleDbCommand(strSQL, conn);
                            myCommand2.ExecuteNonQuery();
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        //登记文件
                        strSQL = "INSERT INTO  File(FileName,Path,SourceType,Source,State) VALUES ('" + fi.Name + "','" + fi.FullName + "','','','')";
                        //setMessgeTextBox(strSQL);
                        myCommand = new OleDbCommand(strSQL, conn);
                        myCommand.ExecuteNonQuery();

                        //获取文件ID
                        strSQL = "SELECT MAX(ID) AS LASTID FROM FILE";
                        myCommand = new OleDbCommand(strSQL, conn);
                        fileid =Convert.ToInt64( myCommand.ExecuteScalar());


                        //记录文件哈希值
                        strSQL = "INSERT INTO  FileHash(FileID,Hash) VALUES (" + fileid + ",'" + filehash + "')";
                        //setMessgeTextBox(strSQL);
                        OleDbCommand myCommand2 = new OleDbCommand(strSQL, conn);
                        myCommand2.ExecuteNonQuery();
                    }

                }        
                conn.Close();
                //修改配置文件
                //Setting.ProgramInitialed = "True";
                //Setting.Save();
            }
            else
            {

            }

   
        }

        //设置监控的目录、文件类型等参数
        public void SetWatcher(string path, string filter)
        {

            fileWatcher.Path = path;
            fileWatcher.Filter = filter;
            fileWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
            fileWatcher.Created += new FileSystemEventHandler(OnFileChanged);
            fileWatcher.Deleted += new FileSystemEventHandler(OnFileChanged);
            fileWatcher.Renamed += new RenamedEventHandler(OnFileRenamed);
            fileWatcher.EnableRaisingEvents = true;

            fileWatcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Security |
                NotifyFilters.Size;
            fileWatcher.IncludeSubdirectories = true;
        }

        //OnFileChanged函数处理文件的change、creat、delete等文件变化
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {

            try
            {

                if (e.ChangeType == WatcherChangeTypes.Created)
                {
                    string msgTxt;
                    msgTxt = this.messageTextBox.Text + "\r\n" + DateTime.Now.ToString() + "创建了新文件:" + e.FullPath;
                    setMessgeTextBox(msgTxt);

                }
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    string msgTxt;
                    msgTxt = this.messageTextBox.Text + "\r\n" + DateTime.Now.ToString() + "删除了文件:" + e.FullPath;
                    setMessgeTextBox(msgTxt);

                }
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    string msgTxt;
                    msgTxt = this.messageTextBox.Text + "\r\n" + "改动:" + e.FullPath;
                    setMessgeTextBox(msgTxt);

                }
            }
            catch
            {
                // Close();
            }

        }

        //OnFileRenamed函数处理文件的rename等文件变化
        private void OnFileRenamed(object source, RenamedEventArgs e)
        {
            string msgTxt;
            msgTxt = this.messageTextBox.Text + "\r\n" + DateTime.Now.ToString() + e.OldName + "重命名为:" + e.FullPath;
            setMessgeTextBox(msgTxt);
        }


        /// <summary>
        /// 监视当前所有打开的浏览器，启动一个定时器，5秒钟间隔
        /// </summary>
        private void WatchExplorerStart()
        {
            
            ewTimer.Interval = 500;
            ewTimer.Elapsed += new System.Timers.ElapsedEventHandler(ExplorerWatcherElapsed);
            ewTimer.Start();
        }

        /// <summary>
        /// 定时器执行函数，监控系统程序
        /// </summary>
        private void ExplorerWatcherElapsed(object sender,System.Timers.ElapsedEventArgs e)
        {
            listProjectWindow();
            //this.BeginInvoke(new HighLightOpenedProject_dlg(HighLightOpenedProject), null);//有用的，测试后恢复

            //Calldelegate();
           
            //setMessgeTextBox(this.messageTextBox.Text+"1");
        }

        /// <summary>
        /// 高亮显示已经打开的项目，支持多线程操作
        /// </summary>
        private void HighLightOpenedProject()
        {
            
            int i=0;
            foreach (TreeNode rootTN in MyProjectsListTreeView.Nodes )
            {
                //MessageBox.Show(lvi.SubItems[1].Text.ToString());
                //string treenodete = rootTN.SubItems[1].Text.ToString() + rootTN.SubItems[2].Text.ToString();
                if (MyProjectList[i].Working==true)
                {
                    rootTN.BackColor = Color.Red;
                    //setMessgeTextBox(this.messageTextBox.Text + "\n"+ projectcode);
                    if(rootTN.Nodes!=null)
                    {
                        int j = 0;
                        foreach (TreeNode tNode in rootTN.Nodes)
                        {
                            if(MyProjectList[i].Units[j].Working=="working")
                            {
                                tNode.BackColor = Color.Blue;
                            }
                            else
                            {
                                tNode.BackColor = Color.White;
                            }
                            j++;
                        }
                         
                    }
                       
                }
                else
                {
                    rootTN.BackColor = Color.White;
                }
                i++;
            }

        }
        /// <summary>
        /// 声明委托HighLightOpenedProject() 
        /// </summary>
        private delegate void HighLightOpenedProject_dlg();
        //private void    Calldelegate()
        //{
        //    this.BeginInvoke(new HighLightOpenedProject_dlg(HighLightOpenedProject), null);

        //}
        public void NewProjectAdded(object sender,FormClosedEventArgs args)
        {
            ShowMyProjectListTreeView();
        }

        private void SetWorkingDirButton_Click(object sender, EventArgs e)
        {
            Project proj = MyProjectList.Find(c => c.Working.Equals(true));
            if(proj!=null)
            {
                Unit munit = proj.Units.Find(d => d.Working.Equals(true));
                if(munit!=null)
                {
                    MessageBox.Show("当前打开" + proj.Code + " " + proj.Name + " " + munit.Code + munit.Name);  //显示当前打开的单元
                    string unitDirFullPath;
                    unitDirFullPath = SettingFormController.projectRootFolder + @"\" + proj.Work.Dir + @"\" + munit.Work.Dir;
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "01DesignPlan");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "02Cal");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "03Transmittal");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "04Check");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "05Archived");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "06Order");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "07Sign");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "08Server");
                    Directory.CreateDirectory(unitDirFullPath + @"\" + "10Backup");

                }
                else
                {
                    MessageBox.Show("当前打开" + proj.Code + " " + proj.Name);  //显示当前打开的单元
                }
                
            }
            else
            {
                MessageBox.Show("当前没有打开任何项目单元！");
            }
            


        }

        private void SetTableHeadButton_Click(object sender, EventArgs e)
        {
            Project proj = MyProjectList.Find(c => c.Working.Equals(true));
            if (proj != null)
            {
                Unit munit = proj.Units.Find(d => d.Working.Equals(true));
                if (munit != null)
                {
                    MessageBox.Show("当前打开" + proj.Code + " " + proj.Name + " " + munit.Code + munit.Name);  //显示当前打开的单元
                    String curUnitPath = SettingFormController.projectRootFolder + @"\" + proj.Work.Dir + @"\" + munit.Work.Dir;
                    DirectoryInfo curDirInfo = new DirectoryInfo(curUnitPath);
                    FileInfo[] fiA = curDirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);
                    foreach(FileInfo fi in fiA)
                    {
                        string fileName=fi.Name;
                        if (fileName.Contains(".xl") || fileName.Contains(".doc") || fileName.Contains(".dwg"))
                        {
                            fileName.IndexOf("ws-",comparisonType:StringComparison.OrdinalIgnoreCase);
                            string newName = curUnitPath+@"\";
                            if(!File.Exists(newName))
                                File.Move(fileName, newName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("当前打开" + proj.Code + " " + proj.Name);  //显示当前打开的单元
                }

            }
            else
            {
                MessageBox.Show("当前没有打开任何项目单元！");
            }

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            //MailTo("testMail");
        }

        private void AddProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectForm nPF = new NewProjectForm();
            nPF.FormClosed += new FormClosedEventHandler(NewProjectAdded);
            nPF.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutForm = new AboutBox();
            aboutForm.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReadPersonListFormRTX()
        {
            string showtext = "";
            IntPtr vHandle = FindWindow("#32770", "在线人员列表");
            if (vHandle != IntPtr.Zero)
            {
                vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "List1");
            }
            if (vHandle == IntPtr.Zero) return;
            int vItemCount = ListView_GetItemCount(vHandle);
            uint vProcessId;

            GetWindowThreadProcessId(vHandle, out vProcessId);

            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
            try
            {
                for (int i = 0; i < vItemCount; i++)
                {
                    Person per = new Person();
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    string vText = Marshal.PtrToStringUni(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    per.RTXID = Convert.ToInt32(vText);

                    vItem[0].iSubItem = 1;
                    WriteProcessMemory(vProcess, vPointer,
                           Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                           Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    vText = Marshal.PtrToStringUni(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    per.Name = vText;


                    vItem[0].iSubItem = 2;
                    WriteProcessMemory(vProcess, vPointer,
                      Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                      Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    vText = Marshal.PtrToStringUni(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    string[] str = vText.Split('-');
                    per.FullName = str[1];


                    vItem[0].iSubItem = 3;
                    WriteProcessMemory(vProcess, vPointer,
                      Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                      Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);

                    vText = Marshal.PtrToStringUni(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    per.Department = vText;
                    PersonList.Add(per);
                }
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
            OleDbConnection conn = getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            conn.Open();
        }
        private void ShowAllPersonList()
        {
            PersonList=Person.GetAllPersonList();
            foreach(Person per in PersonList)
            {
                FindPersonComboBox.Items.Add(per.FullName + "(" + per.Name + ")");

                TreeNode[] findDepartNodes =AllPersonTreeView.Nodes.Find(per.Department,false);
                if (findDepartNodes.Length>0)
                {
                    TreeNode perNode = new TreeNode();
                    perNode.Text = per.FullName+"("+per.Name+")";
                    perNode.Name = per.Name;
                    findDepartNodes[0].Nodes.Add(perNode);
                }
                else
                {
                    TreeNode departNode = new TreeNode();
                    departNode.Text = per.Department;
                    departNode.Name = per.Department;


                    TreeNode perNode = new TreeNode();
                    perNode.Text = per.FullName + "(" + per.Name + ")";
                    perNode.Name =  per.Name ;
                    departNode.Nodes.Add(perNode);

                    AllPersonTreeView.Nodes.Add(departNode);
                }
            }   
            
        }
        private void ShowAllProject()
        {
            
            foreach(Project pro in AllProjectList)
            {
                TreeNode projTn=new TreeNode();
                projTn.Text=pro.Name;
                projTn.Name=pro.Name;
                projTn.Tag=pro.Code;
                foreach(Unit unit in pro.Units)
                {
                    TreeNode unitTn = new TreeNode();
                    unitTn.Text = unit.Name;
                    unitTn.Name = unit.Name;
                    unitTn.Tag = unit.Code;
                    projTn.Nodes.Add(unitTn);
                }
                AllProjectsTreeView.Nodes.Add(projTn);
            }
        }

        /// <summary>
        /// 利用Win32 API读取其他程序中的SysListView32中的值
        /// </summary>
        /// <param name="AHandle"></param>
        /// <returns></returns>
        public int ListView_GetItemCount(IntPtr AHandle)
        {
            return SendMessage(AHandle, LVM_GETITEMCOUNT, 0, 0);
        }

        private void GetPersonDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadPersonListFormRTX();
        }

        private void MyWorkOpenSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage MainTab = new TabPage();
            TreeNode SelectedNode = MyProjectsListTreeView.SelectedNode;

            if (SelectedNode.Level==0)
            {
                
                MainTab.Text = SelectedNode.Text;
                MainTab.Tag = SelectedNode.Tag;
                ShowMyWorkSourceInListView((Project)MainTab.Tag);
            }      
            else if (SelectedNode.Level == 1)
            {
                string text=((Project)SelectedNode.Parent.Tag).Code + SelectedNode.Text;
                MainTab.Text = text;
                MainTab.Tag = SelectedNode.Tag;
                ShowMyWorkSourceInListView((Unit)MainTab.Tag,MainTab);
            }       
            MainTabControl.TabPages.Add(MainTab);
            MainTabControl.SelectedTab=MainTab; 
        }
        public void ShowMyWorkSourceInListView(Project MyProj)
        {

        }
        /// <summary>
        /// MainTabControl中显示MyWork中的所有资源
        /// </summary>
        /// <param name="MyUnit">要显示的单元</param>
        /// <param name="MainTab">MainTab，在此显示资源</param>
        public void ShowMyWorkSourceInListView(Unit MyUnit, TabPage MainTab)
        {
            ListView MyWorkListView = new ListView();
            MyWorkListView.Dock = DockStyle.Fill;
            //MyWorkListView.View = View.LargeIcon;
            MyWorkListView.View = View.LargeIcon;
            MyWorkListView.FullRowSelect = true;
            MainTab.Controls.Add(MyWorkListView);
            //定义MyWorkListView标题行
            MyWorkListView.Columns.Add("", 50, HorizontalAlignment.Left);
            MyWorkListView.Columns.Add("文件名", 100, HorizontalAlignment.Left);
            MyWorkListView.Columns.Add("文件编号", 100, HorizontalAlignment.Left);
            MyWorkListView.Show();
            MyWorkListView.Groups.Clear();
            MyWorkListView.Clear();
            //定义ListView分组
            ListViewGroup DataSheetGroup = new ListViewGroup("数据表");
            ListViewGroup SummarySheetGroup = new ListViewGroup("汇总表");
            ListViewGroup DrawingGroup = new ListViewGroup("图纸");

            //将分组添加至MyWorkListView
            MyWorkListView.Groups.Add(DataSheetGroup);
            MyWorkListView.Groups.Add(SummarySheetGroup);
            MyWorkListView.Groups.Add(DrawingGroup);
            //遍历单元目录下的文件
            DirectoryInfo unitDirectory = new DirectoryInfo(MyUnit.FilesPath);
            if (!unitDirectory.Exists)
            {
                return;
            }
                
            FileInfo[] fileArray = unitDirectory.GetFiles();

            foreach(FileInfo fileIn in fileArray)
            {
                if(fileIn.Extension.Contains(".xl"))
                {
                    SheetFile sheetFile = new SheetFile(fileIn);
                    sheetFile.Open();

                    ListViewItem lvi = new ListViewItem(sheetFile.Head.Name);
                    switch(sheetFile.Type.TypeCode)
                    {
                        case "D":
                            lvi.Group = DataSheetGroup;
                            break;
                        case "E":
                            lvi.Group = SummarySheetGroup;
                            break;
                    }
                    lvi.Text = (MyWorkListView.Items.Count + 1).ToString() + sheetFile.Head.Name;
                    lvi.SubItems.Add(sheetFile.Head.Name);//数据表标题
                    lvi.SubItems.Add(sheetFile.Head.Code);//文表号
                    
                    MyWorkListView.Items.Add(lvi);  
                    string normalFileName=MyUnit.Owner.No+MyUnit.Owner.Stage.Code+MyUnit.Code+"-"+MyUnit.Name+"-"+sheetFile.Head.Code;
                    if(normalFileName!=sheetFile.Info.Name)
                    {
                        lvi.BackColor = Color.Red;
                    }

                    sheetFile.Close();
                }
                else if (fileIn.Extension.Contains(".dwg"))
                {
                    //调用cad内图框查询
                    //MyWorkListView.ControlAdded();
                    List<DWGInfo> diList = DWGInfo.DwgTitles(fileIn.FullName);
                    this.Show();
                    foreach(DWGInfo di in diList)
                    {
                        ListViewItem lvi = new ListViewItem(di.Name);
                        //lvi.Text = (MyWorkListView.Items.Count + 1).ToString() + sheetFile.Head.Name;
                        lvi.SubItems.Add(di.Name);//数据表标题
                        lvi.SubItems.Add(di.No);//文表号
                        lvi.Group = DrawingGroup;
                        MyWorkListView.Items.Add(lvi); 
                    }

 
                }
            }
        }


        private void PersonItemRtxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Person per =  GetSelectedPerson();
            if(per!=null)
            { 
                OpenRtxDialog(per.Name);
            }
            
        }

        private void PersonItemEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userName = "";
            userName = AllPersonTreeView.SelectedNode.Name;
            //OpenOutlookNewEmailDialog(userName);
        }

        private void PersonItemLyncTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {

        } 

        private Person GetSelectedPerson()
        {
            TreeNode tn = AllPersonTreeView.SelectedNode;
            if (tn.Level == 1)
            {
                Person per = new Person();
                per = PersonList.Find(c => c.Name.Equals(tn.Name));
                if(per!=null)
                {
                    return per;
                }
                else
                {
                    return null;
                }  
            }
            else
                return null;
        }

        private void AllPersonTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                TreeNode tn = AllPersonTreeView.GetNodeAt(e.X, e.Y);
                if(tn!=null)
                {
                    AllPersonTreeView.SelectedNode = tn;
                    if(tn.Level==0)
                    {
                        AllPersonTreeView.ContextMenuStrip= null ;
                    }
                    else
                    {
                        AllPersonTreeView.ContextMenuStrip = PersonItemContextMenuStrip;
                    }
                }
            }
        }

        private void ShowFileTree()
        {
            TreeNode rootDesign = new TreeNode();
            MessageBox.Show(SettingFormController.projectRootFolder);
            GetAllFiles(SettingFormController.projectRootFolder, rootDesign);
            rootDesign.Text = "设计项目目录";
            rootDesign.Name = "设计项目目录";
            rootDesign.Tag = SettingFormController.projectRootFolder;
            rootDesign.Expand();
            AllDesignFilesTreeView.Nodes.Add(rootDesign);

            //获取SharePoint服务器上的文件，由于网速所限，初始化中只显示到单元级别
            TreeNode rootSharePoint = new TreeNode();
            rootSharePoint.Text = "SharePoint";
            rootSharePoint.Name = "SharePoint";
            rootSharePoint.Tag = "SharePoint";
            try
            {
                DirectoryInfo diRootSharePoint = new DirectoryInfo(SettingFormController.SharePointRootFolder);
                DirectoryInfo[] diProjSharePoint = diRootSharePoint.GetDirectories();
                Regex regProjectCode = new Regex(@"\w{0,2}\d{4,5}\w{0,1}");
                foreach(DirectoryInfo diProj in diProjSharePoint)
                {
                    if(regProjectCode.IsMatch(diProj.Name))
                    {
                        string sProjSharePointFolder = diProj.FullName + @"\Shared Documents";
                        DirectoryInfo diProjShareDoc = new DirectoryInfo(sProjSharePointFolder);
                        TreeNode projNodeSP = new TreeNode();
                        projNodeSP.Text = diProj.Name;
                        projNodeSP.Name = diProj.Name;
                    
                        projNodeSP.Tag = sProjSharePointFolder;
                        rootSharePoint.Nodes.Add(projNodeSP);                  
                    }
                }
                AllDesignFilesTreeView.Nodes.Add(rootSharePoint);
            }
            catch
            {

            }

            TreeNode rootStandardCode = new TreeNode();
            rootStandardCode.Text = "规范标准";
            rootStandardCode.Name = "StandardCode";
            rootStandardCode.Tag = "StandardCode";
            AllDesignFilesTreeView.Nodes.Add(rootStandardCode);

            TreeNode MySpeciltyStandardCode = new TreeNode();
            MySpeciltyStandardCode.Text = "本专业规范标准";
            MySpeciltyStandardCode.Name = "本专业规范标准";
            MySpeciltyStandardCode.Tag = "本专业规范标准";
            rootStandardCode.Nodes.Add(MySpeciltyStandardCode);

            TreeNode OtherSpeciltyStandardCode = new TreeNode();
            OtherSpeciltyStandardCode.Text = "其他专业规范标准";
            OtherSpeciltyStandardCode.Name = "其他专业规范标准";
            OtherSpeciltyStandardCode.Tag = "其他专业规范标准";
            rootStandardCode.Nodes.Add(OtherSpeciltyStandardCode);

        }

        private void GetAllFiles(string filePath,TreeNode node)
        {
            if(filePath != null)
            {
                DirectoryInfo di = new DirectoryInfo(filePath);
                node.Text = di.Name;
                node.Name = di.Name;
                node.Tag = di.FullName;
                FileInfo[] childFiles = di.GetFiles("*.*");
                foreach (FileInfo cF in childFiles)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = cF.Name;
                    tn.Name = cF.Name;
                    tn.Tag = cF.FullName;
                    node.Nodes.Add(tn);
                }
                DirectoryInfo[] childDirectories = di.GetDirectories();
                foreach (DirectoryInfo cD in childDirectories)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = cD.Name;
                    tn.Name = cD.Name;
                    tn.Tag = cD.FullName;
                    node.Nodes.Add(tn);
                    GetAllFiles(cD.FullName, tn);
                }

            }
            
        }

        private void MyProjectsListTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode tn = MyProjectsListTreeView.GetNodeAt(e.X, e.Y);
                if (tn != null)
                {
                    MyProjectsListTreeView.SelectedNode = tn;
                    if (tn.Level == 0)
                    {
                        MyProjectsListTreeView.ContextMenuStrip = null;
                    }
                    else
                    {
                        MyProjectsListTreeView.ContextMenuStrip = MyWorkContextMenuStrip;
                    }
                }
            }
        }

        private void AllDesignFilesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            string nodePath = e.Node.FullPath;
            int start = nodePath.IndexOf("SharePoint");
            if (start == 0)
            {
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    DirectoryInfo path = new DirectoryInfo((string)tn.Tag);
                    DirectoryInfo[] pathDirectories = path.GetDirectories();

                    foreach (DirectoryInfo subPath in pathDirectories)
                    {
                        TreeNode subTn = new TreeNode();
                        subTn.Text = subPath.Name;
                        subTn.Name = subPath.Name;
                        subTn.Tag = subPath.FullName;
                        tn.Nodes.Add(subTn);
                    }

                    FileInfo[] pathFiles = path.GetFiles();
                    foreach(FileInfo subFile in pathFiles)
                    {
                        TreeNode subTn = new TreeNode();
                        subTn.Text = subFile.Name;
                        subTn.Name = subFile.Name;
                        subTn.Tag = subFile.FullName;
                        tn.Nodes.Add(subTn);
                    }
                }

            }
        }

        private void MainTabControl_DoubleClick(object sender, EventArgs e)
        {
            MainTabControl.TabPages.RemoveAt(MainTabControl.SelectedIndex);
            //MainTabControl.SelectedTab.Hide();
        }

        private void MyWorkOpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void l获取所有文件模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(@"X:\公司文件设计文件格式模板目录");
            FileInfo[] files = di.GetFiles();

           
            
            foreach(FileInfo file in files)
            {
                string name ;
                string ver;
                string code="";
                string strname;
                ver = file.Name;

                name = file.Name.Substring(0, file.Name.Count() - 5);

                //MessageBox.Show(code + "\r\n" + name + "\r\n" + ver);
                string sql = "INSERT INTO NormalFile (编号,名称,类型,版本,公司模板) VALUES ('" + code + "','" + name + "','E','" + ver + "','" + file.FullName + "')";
                OleDbConnection conn = getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void StandardCodeInitialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fileContext = new FileInfo(@"C:\Users\zhangsan\Desktop\标准目录.xlsx");
            Excel.Application xlApp=new Excel.Application();
            Excel.Workbook xlsWorkBook =null ;
                   
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;      //设置是否显示
            xlApp.ScreenUpdating = false;  //禁止刷新屏幕
            try
            {
                xlsWorkBook = xlApp.Workbooks.Open(
                    fileContext.FullName,
                    ReadOnly: false,
                    Editable: true,
                    IgnoreReadOnlyRecommended: true,
                    UpdateLinks: false,
                    Notify: false,
                    CorruptLoad: Excel.XlCorruptLoad.xlNormalLoad);
            }
            catch
            {
                try
                {
                    xlsWorkBook = xlApp.Workbooks.Open(
                        fileContext.FullName,
                        ReadOnly: false,
                        Editable: true,
                        IgnoreReadOnlyRecommended: true,
                        UpdateLinks: false,
                        Notify: false,
                        CorruptLoad: Excel.XlCorruptLoad.xlExtractData);
                }
                catch
                {

                }
            }
            if(xlsWorkBook!=null)
            {
                foreach (Excel.Worksheet sh in xlsWorkBook.Sheets)
                {
                    if (sh.Name == "国标GB")
                    {
                        int row = 2;
                        while (sh.get_Range("A" + row.ToString()).Value !=null&&sh.get_Range("A" + row.ToString()).Value != "")
                        {
                            string standardCode = sh.get_Range("A" + row.ToString()).Value;
                            string standardCode2 = sh.get_Range("B" + row.ToString()).Value;
                            if(standardCode2!=null)
                                standardCode2.Replace("T", "/T");
                            standardCode = standardCode + standardCode2;
                            //string specialtyCode = sh.get_Range("B" + row.ToString()).Value;
                            string numberCode;
                            if(sh.get_Range("C" + row.ToString()).Value!=null)
                                numberCode = sh.get_Range("C" + row.ToString()).Value.ToString();
                            string ageCode;
                            if (sh.get_Range("E" + row.ToString()).Value!=null)
                                ageCode = sh.get_Range("E" + row.ToString()).Value.ToString();
                            string Name = sh.get_Range("F" + row.ToString()).Value;
                            DateTime date;
                            if (sh.get_Range("G" + row.ToString()).Value!=null)
                                if(sh.get_Range("G" + row.ToString()).Value.GetType() == typeof(DateTime))
                                date = sh.get_Range("G" + row.ToString()).Value;
                            string note = sh.get_Range("I" + row.ToString()).Value;
                            string address = sh.get_Range("K" + row.ToString()).Value;
                            row++;
                            //MessageBox.Show(standardCode + numberCode);
                        }
                    }
                    else if (sh.Name != "法律法规和地方标准" && sh.Name != "图集" && sh.Name != "其他资料（书籍、杂志、行业学习资料等）")
                    {
                        int row = 2;
                        while (sh.get_Range("A" + row.ToString()).Value != null && sh.get_Range("A" + row.ToString()).Value != "")
                        {
                            string standardCode = "";
                            string numberCode = "";
                            string ageCode="";
                            string codeString = sh.get_Range("A" + row.ToString()).Value;
                            string Name = sh.get_Range("B" + row.ToString()).Value;
                            DateTime date =Convert.ToDateTime( sh.get_Range("C" + row.ToString()).Value);
                            string note = sh.get_Range("E" + row.ToString()).Value;
                            string address = sh.get_Range("F" + row.ToString()).Value;
                            string surgestCode;
                            if(codeString.Contains("T")||codeString.Contains("t"))
                            {
                                surgestCode = "T";
                                codeString.Replace("T", "");
                                codeString.Replace("t", "");
                            }
                            string[] Codes = codeString.Split(',');
                            foreach (string code in Codes)
                            {
                                if (code.Contains("~"))
                                {
                                    string[] halfCodes = code.Split('~');
                                    string startCode = halfCodes[0];
                                    string endCode = halfCodes[1];
                                    string startNumberCode;
                                    string endNumberCode;
                                    if (startCode.Contains(" "))
                                    {
                                        standardCode = (startCode.Split(' '))[0];
                                        startNumberCode = (startCode.Split(' '))[1];
                                    }
                                    else
                                    {
                                        standardCode = startCode.Substring(0, 2);
                                        startNumberCode = startCode.Replace(standardCode, "");
                                    }
                                    standardCode = standardCode.Trim();
                                    startNumberCode = startNumberCode.Trim();
                                    ageCode = endCode.Split('-')[1];
                                    endNumberCode = endCode.Split('-')[0];
                                    int startNumberCodeIndex = startNumberCode.IndexOf(".");
                                    int endNumberCodeIndex = endNumberCode.IndexOf(".");
                                    if(startNumberCodeIndex>0&&endNumberCodeIndex>0)
                                    {
                                        for (int i = Convert.ToInt32(startNumberCode.Substring(startNumberCodeIndex, startNumberCode.Length - startNumberCodeIndex));
                                            i <= Convert.ToInt32(endNumberCode.Substring(endNumberCodeIndex, endNumberCode.Length - endNumberCodeIndex));
                                            i++)
                                        {
                                            ageCode = i.ToString();
                                            //insert into SQL;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = Convert.ToInt32(startNumberCode);
                                                i <= Convert.ToInt32(endNumberCode);
                                                i++)
                                        {
                                            ageCode = i.ToString();
                                            //insert into SQL;
                                        }
                                    }

                                }
                                else
                                {
                                    code.Trim();
                                    string[] Code1;
                                    if(code.Contains("-"))
                                        Code1 = code.Split('-');
                                    else
                                        Code1 = code.Split('－');
                                    if (Code1.Length==3)
                                    {
                                        standardCode = Code1[0];
                                        numberCode = Code1[1];
                                        ageCode = Code1[2].Trim();
                                    }
                                    else
                                    {
                                        ageCode = Code1[1].Trim();
                                        string[] Code2 = Code1[0].Trim().Split(' ');
                                        standardCode = Code2[0];
                                        numberCode = Code2[1];
                                    }
                                    //insert into SQL
                                }
                                row++;
                            }
                        }
                    }
                }
            }
            xlsWorkBook.Close(false);
            xlApp.Quit();
            KillSpecailExcel(xlApp);
            

        }
        private static void KillSpecailExcel(Microsoft.Office.Interop.Excel.Application m_objExcel)
        {
            try
            {
                if (m_objExcel != null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(m_objExcel.Hwnd), out lpdwProcessId);
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void cADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dwgPath = @"E:\test\XXXXXXX";
            DWGInfo.DwgTitles(dwgPath);
        }

        private void 重命名文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestRenameFiles renameForm = new TestRenameFiles();
            renameForm.Show();

        }

        private void ShowMainNotifyIconStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeMainWindowStateFromNotifyIcon();
        }

        private void FlangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flange fl = new Flange();
            fl.Show();
        }
    }
}

