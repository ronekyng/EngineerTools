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
    public partial class TestRenameFiles : Form
    {
        public TestRenameFiles()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "请选择文件夹";
            openFileDialog.Filter = "所有文件(*.*)|*.*";
            string fileDirectory=null;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filepath in openFileDialog.FileNames)
                {
                    FileInfo fi = new FileInfo(filepath); 
                    string oldName = fi.Name;
                    int index =oldName.IndexOf("-WS", StringComparison.OrdinalIgnoreCase);
                    string oldCode = oldName.Substring(0, index);
                    string newCode = this.TestNewCodeTextBox.Text.Trim();
                    string newName = newCode + oldName.Substring(index , oldName.Length- index );
                    fi.MoveTo(fi.Directory + @"\" + newName);
                    fileDirectory = fi.Directory.ToString();
                }
            }
            System.Diagnostics.Process.Start("explorer.exe", fileDirectory);
            this.Hide();
        }
    }
}
