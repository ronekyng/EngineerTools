 
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

namespace EngineerTools
{
    public partial class SettingForm : Form
    {

        public SettingForm()
        {
            InitializeComponent();
            showSetting();
        }


        private void chooseProjectRootFolderButton_Click(object sender, EventArgs e)
        {
            
            //projectRootFolderBrowserDlg.ShowDialog();
            if(folderBrowserDlg.ShowDialog()==DialogResult.OK)
            {
                this.projectRootFolderTextBox.Text = folderBrowserDlg.SelectedPath;
                //settingLines[projectRootFolderLineNo] = "ProjectRootFolder=" + projectRootFolderTextBox.Text;
                global::EngineerTools.Properties.Settings.Default.ProjectRootFolder =  projectRootFolderTextBox.Text;
            }
        }

        private void chooseRtxFileFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtxFileFolderTextBox.Text = folderBrowserDlg.SelectedPath;
                //settingLines[rtxFolderLineNo] = "RTXFileFolder=" + rtxFileFolderTextBox.Text;
                global::EngineerTools.Properties.Settings.Default.RTXFileFolder = rtxFileFolderTextBox.Text;
            }
        }

        private void showSetting()
        {
            //MessageBox.Show(Setting.projectRootFolder);
            this.projectRootFolderTextBox.Text = global::EngineerTools.Properties.Settings.Default.ProjectRootFolder;
            this.rtxFileFolderTextBox.Text = global::EngineerTools.Properties.Settings.Default.RTXFileFolder;
        }


        private void saveSettingButton_Click(object sender, EventArgs e)
        {
            SettingFormController.SaveSettings();
        }

        private void OKSettingButton_Click(object sender, EventArgs e)
        {
            SettingFormController.SaveSettings();
            Close();
        }

        private void cancelSettingButton_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
