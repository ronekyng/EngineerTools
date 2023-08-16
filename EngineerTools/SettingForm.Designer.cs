namespace EngineerTools
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.chooseProjectRootFolderButton = new System.Windows.Forms.Button();
            this.projectRootFolderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveSettingButton = new System.Windows.Forms.Button();
            this.OKSettingButton = new System.Windows.Forms.Button();
            this.cancelSettingButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxFileFolderTextBox = new System.Windows.Forms.TextBox();
            this.chooseRtxFileFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chooseProjectRootFolderButton
            // 
            this.chooseProjectRootFolderButton.Location = new System.Drawing.Point(333, 29);
            this.chooseProjectRootFolderButton.Name = "chooseProjectRootFolderButton";
            this.chooseProjectRootFolderButton.Size = new System.Drawing.Size(60, 25);
            this.chooseProjectRootFolderButton.TabIndex = 0;
            this.chooseProjectRootFolderButton.Text = "浏 览";
            this.chooseProjectRootFolderButton.UseVisualStyleBackColor = true;
            this.chooseProjectRootFolderButton.Click += new System.EventHandler(this.chooseProjectRootFolderButton_Click);
            // 
            // projectRootFolderTextBox
            // 
            this.projectRootFolderTextBox.Location = new System.Drawing.Point(58, 30);
            this.projectRootFolderTextBox.Name = "projectRootFolderTextBox";
            this.projectRootFolderTextBox.ReadOnly = true;
            this.projectRootFolderTextBox.Size = new System.Drawing.Size(269, 21);
            this.projectRootFolderTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目目录";
            // 
            // saveSettingButton
            // 
            this.saveSettingButton.Location = new System.Drawing.Point(151, 215);
            this.saveSettingButton.Name = "saveSettingButton";
            this.saveSettingButton.Size = new System.Drawing.Size(84, 26);
            this.saveSettingButton.TabIndex = 3;
            this.saveSettingButton.Text = "应用";
            this.saveSettingButton.UseVisualStyleBackColor = true;
            this.saveSettingButton.Click += new System.EventHandler(this.saveSettingButton_Click);
            // 
            // OKSettingButton
            // 
            this.OKSettingButton.Location = new System.Drawing.Point(288, 215);
            this.OKSettingButton.Name = "OKSettingButton";
            this.OKSettingButton.Size = new System.Drawing.Size(77, 26);
            this.OKSettingButton.TabIndex = 4;
            this.OKSettingButton.Text = "确定";
            this.OKSettingButton.UseVisualStyleBackColor = true;
            this.OKSettingButton.Click += new System.EventHandler(this.OKSettingButton_Click);
            // 
            // cancelSettingButton
            // 
            this.cancelSettingButton.Location = new System.Drawing.Point(29, 214);
            this.cancelSettingButton.Name = "cancelSettingButton";
            this.cancelSettingButton.Size = new System.Drawing.Size(84, 26);
            this.cancelSettingButton.TabIndex = 5;
            this.cancelSettingButton.Text = "取消";
            this.cancelSettingButton.UseVisualStyleBackColor = true;
            this.cancelSettingButton.Click += new System.EventHandler(this.cancelSettingButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "RTX目录";
            // 
            // rtxFileFolderTextBox
            // 
            this.rtxFileFolderTextBox.Location = new System.Drawing.Point(58, 67);
            this.rtxFileFolderTextBox.Name = "rtxFileFolderTextBox";
            this.rtxFileFolderTextBox.ReadOnly = true;
            this.rtxFileFolderTextBox.Size = new System.Drawing.Size(269, 21);
            this.rtxFileFolderTextBox.TabIndex = 7;
            // 
            // chooseRtxFileFolderButton
            // 
            this.chooseRtxFileFolderButton.Location = new System.Drawing.Point(333, 66);
            this.chooseRtxFileFolderButton.Name = "chooseRtxFileFolderButton";
            this.chooseRtxFileFolderButton.Size = new System.Drawing.Size(60, 25);
            this.chooseRtxFileFolderButton.TabIndex = 6;
            this.chooseRtxFileFolderButton.Text = "浏 览";
            this.chooseRtxFileFolderButton.UseVisualStyleBackColor = true;
            this.chooseRtxFileFolderButton.Click += new System.EventHandler(this.chooseRtxFileFolderButton_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtxFileFolderTextBox);
            this.Controls.Add(this.chooseRtxFileFolderButton);
            this.Controls.Add(this.cancelSettingButton);
            this.Controls.Add(this.OKSettingButton);
            this.Controls.Add(this.saveSettingButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.projectRootFolderTextBox);
            this.Controls.Add(this.chooseProjectRootFolderButton);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDlg;
        private System.Windows.Forms.Button chooseProjectRootFolderButton;
        private System.Windows.Forms.TextBox projectRootFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveSettingButton;
        private System.Windows.Forms.Button OKSettingButton;
        private System.Windows.Forms.Button cancelSettingButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox rtxFileFolderTextBox;
        private System.Windows.Forms.Button chooseRtxFileFolderButton;
    }
}