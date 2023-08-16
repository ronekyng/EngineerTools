using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Web;
using Excel=Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.IO;
using System.Collections;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Threading;

namespace EngineerTools
{
    public partial class NewProjectForm : Form
    {
        [ DllImport ("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        private String specialPlanDir = @"\\xxx.com\public\项目收发文件";
        //private String specialPlanDir = @"C:\Users\zhangsan\Desktop";
        private string specialPlanName = "年某专业设计任务一览表";
        private string projectTablePassword = "8320";
        List<Project> projectList = new List<Project>();
        public NewProjectForm()
        {
            InitializeComponent();
            StageCodeComboBox.Items.Add("B");
            StageCodeComboBox.Items.Add("D");
            StageCodeComboBox.Items.Add("F");
        }
        /// <summary>
        /// 查询专业计划，获取本地没有准备好的新项目信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAvailibleProjectButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(specialPlanDir);
            FileInfo[] fiA = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            string specialPlanFullPath="";

            int ProgressValue=0;
            this.Enabled = false;       //进度条开始
            SearchProjectListProgressBar.Value = ProgressValue;
            SearchProjectListProgressBar.Minimum = 0;
            SearchProjectListProgressBar.Maximum = 100;
            SearchProjectListProgressBar.Visible = true;

            ProgressLabel.Text = "正在读取文件...";
            ProgressLabel.Visible = true;
            

            int i=0;
            foreach (FileInfo fi in fiA)
            {
                
                ProgressValue = i * 18 / fiA.Length;     //进度设置
                SearchProjectListProgressBar.Value = ProgressValue;
                i++;
                if (fi.Name.Contains(specialPlanName) & fi.Name.Contains("xl") & !(fi.Name.Contains("~$")))
                {
                    specialPlanFullPath = specialPlanDir + @"\" + fi.Name;
                }
                
            }
            if (specialPlanFullPath == "")
            {
                return;
            }
 
            Excel.Application xlApp = new Excel.Application();
            //设置是否显示警告窗体
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;      //设置是否显示
            xlApp.ScreenUpdating = false;  //禁止刷新屏幕
            Excel.Workbook xlsWorkBook;
            
           
            SearchProjectListProgressBar.Value = 20;            //设置进度条
            try
            {
                xlsWorkBook = xlApp.Workbooks.Open(specialPlanFullPath, ReadOnly: false, Password: projectTablePassword);                   
                Excel.Worksheet xlsWorkSheet = xlsWorkBook.Sheets["设计任务"];
                Excel.Range myNameRange = xlsWorkSheet.Cells.Find(MainForm.My.FullName);
                Excel.AutoFilter filter = xlsWorkSheet.AutoFilter;
                xlsWorkSheet.AutoFilterMode = false;
                Excel.Range projTableEndRange = xlsWorkSheet.Cells.Find("合计");

                for (int j = 4; j < projTableEndRange.Row; j++)
                {
                    ProgressValue = 20 + j * 80 / projTableEndRange.Row;        //设置进度条
                    //ProgressValue = 99;
                    SearchProjectListProgressBar.Value = ProgressValue;
                    SearchProjectListProgressBar.Update();
                    //先判断空行,空行什么都不做，非空继续
                    if (xlsWorkSheet.get_Range("F" + j.ToString()).Value == null || xlsWorkSheet.get_Range("G" + j.ToString()).Value == null)
                    {
                                
                    }
                    else   //非空行，判断是否已存在项目，若已存在，添加单元，否则添加新项目，并添加单元
                    {
                        String no =xlsWorkSheet.get_Range("F" + j.ToString()).Value.ToString();
                        String stage=xlsWorkSheet.get_Range("G" + j.ToString()).Value.ToString();
                        Project findProj;

                        findProj = projectList.Find(c => c.Code.Equals(no + "-" + stage));      //查找项目是否存在

                        //找不到已存在项目，新建项目并插入，否则什么都不做，继续添加单元
                        if(findProj==null)
                        {
                            Project proj = new Project();
                            proj.No = xlsWorkSheet.get_Range("F" + j.ToString()).Value.ToString();
                            proj.Stage.Code = xlsWorkSheet.get_Range("G" + j.ToString()).Value.ToString();
                            if (xlsWorkSheet.get_Range("C" + j.ToString()).Value != null)
                            {
                                proj.Constructure = xlsWorkSheet.get_Range("C" + j.ToString()).Value.ToString();
                            }
                            else
                            {
                                proj.Constructure = "";
                            }
                            if (xlsWorkSheet.get_Range("D" + j.ToString()).Value != null)
                            {
                                proj.Name = xlsWorkSheet.get_Range("D" + j.ToString()).Value.ToString();
                            }
                            else
                            {
                                proj.Name = "";
                            }
                            projectList.Add(proj);
                        }

                        findProj = projectList.Find(c => c.Code.Equals(no + "-" + stage));      //继续查找该项目，以便添加单元
                        if (findProj != null)
                        //找到该项目，则向其中添加本行单元
                        { 
                            if (xlsWorkSheet.get_Range("H" + j.ToString()).Value != null && xlsWorkSheet.get_Range("E" + j.ToString()).Value != null)
                            {
                                Unit munit = new Unit();
                                munit.Name = xlsWorkSheet.get_Range("E" + j.ToString()).Value.ToString();
                                munit.Code = xlsWorkSheet.get_Range("H" + j.ToString()).Value.ToString();
                                Role role = new Role();
                                if(xlsWorkSheet.get_Range("K" + j.ToString()).Value!=null)
                                {
                                    if (xlsWorkSheet.get_Range("K" + j.ToString()).Value.ToString() == MainForm.My.FullName)
                                    {
                                        role.Worker.FullName = MainForm.My.FullName;
                                        role.RoleWork=(int)Role.roles.项目负责人;
                                        munit.RoleList.Add(role);
                                    }
                                    
                                }
                                if (xlsWorkSheet.get_Range("M" + j.ToString()).Value != null)
                                {
                                    if (xlsWorkSheet.get_Range("M" + j.ToString()).Value.ToString() == MainForm.My.FullName)
                                    {
                                        role.Worker.FullName = MainForm.My.FullName;
                                        role.RoleWork = (int)Role.roles.单元负责人;
                                        munit.RoleList.Add(role);
                                    }
                                }

                                if (xlsWorkSheet.get_Range("N" + j.ToString()).Value != null)
                                {
                                    if (xlsWorkSheet.get_Range("N" + j.ToString()).Value.ToString() == MainForm.My.FullName)
                                    {
                                        role.Worker.FullName = MainForm.My.FullName;
                                        role.RoleWork = (int)Role.roles.设计;
                                        munit.RoleList.Add(role);
                                    }
                                }

                                if (xlsWorkSheet.get_Range("O" + j.ToString()).Value != null)
                                {
                                    if (xlsWorkSheet.get_Range("O" + j.ToString()).Value.ToString() == MainForm.My.FullName)
                                    {
                                        role.Worker.FullName = MainForm.My.FullName;
                                        role.RoleWork = (int)Role.roles.校核;
                                        munit.RoleList.Add(role);
                                    }
                                }

                                if (xlsWorkSheet.get_Range("P" + j.ToString()).Value != null)
                                {
                                    if (xlsWorkSheet.get_Range("P" + j.ToString()).Value.ToString() == MainForm.My.FullName)
                                    {
                                        role.Worker.FullName = MainForm.My.FullName;
                                        role.RoleWork = (int)Role.roles.审核;
                                        munit.RoleList.Add(role);
                                    }
                                }
                                if (munit.RoleList.Count > 0)
                                    findProj.Units.Add(munit);
                            }
                        
                        }

                    }

                }

                //MessageBox.Show(projectList.Count.ToString());
                xlsWorkBook.Close(false);
                xlApp.DisplayAlerts = false;
                xlApp.Quit();
                KillSpecailExcel(xlApp);
            }
            catch
            {
                MessageBox.Show("打开文件" + specialPlanFullPath + "失败");
                xlApp.DisplayAlerts = false;
                xlApp.Quit();
                KillSpecailExcel(xlApp);
            }

            OleDbConnection localProjectMdbConn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
            localProjectMdbConn.Open();
            try
            {
                string strSQL = "SELECT * FROM [项目表] ";
                OleDbCommand myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                OleDbDataReader reader;
                reader = myCommand.ExecuteReader();
     
                while(reader.Read())
                {
                    foreach(Project proj in projectList)
                    {
                        if(reader["项目号"].ToString()==proj.No&&reader["设计阶段"].ToString()==proj.Stage.Code )
                        {                                
                            projectList.Remove(proj);
                            break;
                        }
                        else
                        {

                        }
                    }

                }
                //倒序排列 projectlist
                projectList.Reverse();
                //如果所有单元的rolelist（仅为本用户的role）均为空，则删除该项目
                for (i = projectList.Count - 1; i >= 0; i--)
                {
                    Project proj = projectList[i];
                    bool hasWork = false;
                    foreach (Unit munit in proj.Units)
                    {
                        if (munit.RoleList.Count > 0)
                        {
                            hasWork = true;
                        }
                    }
                    if (hasWork == false)
                    {
                        projectList.Remove(proj);
                    }

                }
                ProjectNoComboBox.Items.Clear();
                foreach(Project proj in projectList)
                {
                    ProjectNoComboBox.Items.Add(proj.Code);
                }
                ProjectNoComboBox.SelectedIndex = 0;

                reader.Close();
                localProjectMdbConn.Close();
            }
            catch
            {
                MessageBox.Show("数据库打开失败,请手动输入项目信息！");
                localProjectMdbConn.Close();
            }
            SearchProjectListProgressBar.Value = 100;
            SearchProjectListProgressBar.Update();
            ProgressLabel.Text = "查询文件完成！";
            this.Update();
            Thread.Sleep(500);
            this.Enabled = true;
            SearchProjectListProgressBar.Visible = false;
            ProgressLabel.Visible = false;

        }

        private static void KillSpecailExcel(Microsoft.Office.Interop.Excel.Application m_objExcel)
        {
            try
            {
                if(m_objExcel!=null)
                {
                    int lpdwProcessId;
                    GetWindowThreadProcessId(new IntPtr(m_objExcel.Hwnd), out lpdwProcessId);
                    System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 按下“保存项目”按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveProjectButton_Click(object sender, EventArgs e)
        {
            Project proj;
            proj = projectList.Find(c => c.Code.Equals(ProjectNoComboBox.Text));
            if(proj==null)
            {
                return;
            }
            try
            {
                //向“项目表”中插入新项目
                OleDbConnection localProjectMdbConn = MainForm.getAccessConn(global::EngineerTools.Properties.Settings.Default.MdbFileName);
                localProjectMdbConn.Open();
                string strSQL = "INSERT INTO [项目表](项目号,设计阶段,建设单位,项目名称) VALUES('"
                                  +proj.No+"','"
                                  +proj.Stage+"','"
                                  +proj.Constructure+"','"
                                  + proj.Name 
                                  + "');";
                OleDbCommand myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                myCommand.ExecuteNonQuery();

                strSQL = "SELECT MAX(ID) AS LASTID FROM 项目表";
                myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                long projID = Convert.ToInt64(myCommand.ExecuteScalar());

                //获取“person”中当前用户的ID
                long userID = MainForm.My.ID;

                string projShortName = proj.Constructure.Substring(0,4);

                //添加个人项目表
                String projDirShortPath = proj.No + proj.Stage + " " +projShortName;
                strSQL = "INSERT INTO [个人项目表](人员,项目,项目根目录,开始时间) VALUES("
                    + userID + ","
                    + projID + ",'"
                    + projDirShortPath + "',"
                    +  "now())";
                myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                myCommand.ExecuteNonQuery();

                //生成个人项目目录
                String projDirFullPath = SettingFormController.projectRootFolder+ @"\" + projDirShortPath;
                if (!Directory.Exists(projDirFullPath))
                {
                    Directory.CreateDirectory(projDirFullPath);
                }
                //添加单元
                if(UnitsListView.Items.Count>0)
                {
                    foreach(ListViewItem lvi in UnitsListView.Items)
                    {
                        if(lvi.Checked)
                        {
                            Unit munit;
                            munit = proj.Units.Find(c => c.Code.Equals(lvi.SubItems[0]));

                            //单元表，添加记录
                            strSQL = "INSERT INTO [单元表](项目阶段ID,单元号,单元名称) VALUES('" 
                                + projID + "','"
                                + lvi.SubItems[1].Text + "','"
                                + lvi.SubItems[2].Text + "')";                    
                            myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                            myCommand.ExecuteNonQuery();

                            //获取单元表ID
                            strSQL = "SELECT MAX(ID) AS LASTID FROM 单元表";
                            myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                            long unitID = Convert.ToInt64(myCommand.ExecuteScalar());

                            //添加个人单元表
                            String unitDirShortPath = lvi.SubItems[1].Text + " " +lvi.SubItems[2].Text;
                            strSQL = "INSERT INTO [个人单元表](人员,单元,单元目录,开始时间,状态) VALUES('"
                                    + userID + "',"
                                    + unitID + ",'"
                                    + unitDirShortPath + "',"
                                    +  "now(),'working')";
                            myCommand = new OleDbCommand(strSQL, localProjectMdbConn);
                            myCommand.ExecuteNonQuery();
                            
                            //创建各单元目录
                            String unitDirFullPath = projDirFullPath + @"\" +unitDirShortPath;
                            if (!Directory.Exists(unitDirFullPath))
                            {
                                Directory.CreateDirectory(unitDirFullPath);
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
                        }

                    }
                }

                localProjectMdbConn.Close();
            }
            catch
            {
                MessageBox.Show("数据库打开失败,请手动输入项目信息！");
            }
            UnitsListView.Clear();
            ProjectNoComboBox.Items.Remove(ProjectNoComboBox.SelectedItem);
            ProjectNoComboBox.SelectedIndex = -1;
            ProjectNoComboBox.Text = "";
            ProjectAddressTextBox.Text = "";
            ProjectNameTextBox.Text = "";
            ConstructureNameTextBox.Text = "";
            StageCodeComboBox.Text = "";
            
            projectList.Remove(proj);
            ProgressLabel.Text = "项目、单元已添加，目录已生成！";
        }

        /// <summary>
        /// 项目号列表改变下拉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectNoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex =ProjectNoComboBox.SelectedIndex;
            StageCodeComboBox.Text = projectList[selIndex].Stage.Code;
            ConstructureNameTextBox.Text=projectList[selIndex].Constructure;
            ProjectNameTextBox.Text = projectList[selIndex].Name;
            UnitsListView.Clear();
            UnitsListView.Columns.Add("选择", 50, HorizontalAlignment.Left);

            UnitsListView.Columns.Add("单元号", 50, HorizontalAlignment.Left);
            UnitsListView.Columns.Add("单元名称", 100, HorizontalAlignment.Left);
            UnitsListView.Columns.Add("负责人", 100, HorizontalAlignment.Left);

            foreach(Unit munit in projectList[selIndex].Units)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Checked = true;
                lvi.SubItems.Add(munit.Code);
                lvi.SubItems.Add(munit.Name);
                lvi.SubItems.Add("");
                UnitsListView.Items.Add(lvi);
            }

            
            //UnitsListView


        }
    }
}
