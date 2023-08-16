namespace EngineerTools
{
    partial class NewProjectForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConstructureNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ProjectNameTextBox = new System.Windows.Forms.TextBox();
            this.UnitsListView = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.SaveProjectButton = new System.Windows.Forms.Button();
            this.GetAvailibleProjectButton = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ProjectAddressTextBox = new System.Windows.Forms.TextBox();
            this.StageCodeComboBox = new System.Windows.Forms.ComboBox();
            this.ProjectNoComboBox = new System.Windows.Forms.ComboBox();
            this.SearchProjectListProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目阶段号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "阶段号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "建设单位";
            // 
            // ConstructureNameTextBox
            // 
            this.ConstructureNameTextBox.Location = new System.Drawing.Point(82, 52);
            this.ConstructureNameTextBox.Name = "ConstructureNameTextBox";
            this.ConstructureNameTextBox.Size = new System.Drawing.Size(240, 21);
            this.ConstructureNameTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "项目名称";
            // 
            // ProjectNameTextBox
            // 
            this.ProjectNameTextBox.Location = new System.Drawing.Point(82, 83);
            this.ProjectNameTextBox.Name = "ProjectNameTextBox";
            this.ProjectNameTextBox.Size = new System.Drawing.Size(240, 21);
            this.ProjectNameTextBox.TabIndex = 6;
            // 
            // UnitsListView
            // 
            this.UnitsListView.CheckBoxes = true;
            this.UnitsListView.FullRowSelect = true;
            this.UnitsListView.Location = new System.Drawing.Point(5, 235);
            this.UnitsListView.Name = "UnitsListView";
            this.UnitsListView.Size = new System.Drawing.Size(320, 134);
            this.UnitsListView.TabIndex = 8;
            this.UnitsListView.UseCompatibleStateImageBehavior = false;
            this.UnitsListView.View = System.Windows.Forms.View.Details;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(65, 25);
            this.button1.TabIndex = 13;
            this.button1.Text = "添加单元";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SaveProjectButton
            // 
            this.SaveProjectButton.Location = new System.Drawing.Point(257, 147);
            this.SaveProjectButton.Name = "SaveProjectButton";
            this.SaveProjectButton.Size = new System.Drawing.Size(65, 25);
            this.SaveProjectButton.TabIndex = 15;
            this.SaveProjectButton.Text = "保存项目";
            this.SaveProjectButton.UseVisualStyleBackColor = true;
            this.SaveProjectButton.Click += new System.EventHandler(this.SaveProjectButton_Click);
            // 
            // GetAvailibleProjectButton
            // 
            this.GetAvailibleProjectButton.Location = new System.Drawing.Point(11, 147);
            this.GetAvailibleProjectButton.Name = "GetAvailibleProjectButton";
            this.GetAvailibleProjectButton.Size = new System.Drawing.Size(130, 25);
            this.GetAvailibleProjectButton.TabIndex = 16;
            this.GetAvailibleProjectButton.Text = "尝试检索可用的项目";
            this.GetAvailibleProjectButton.UseVisualStyleBackColor = true;
            this.GetAvailibleProjectButton.Click += new System.EventHandler(this.GetAvailibleProjectButton_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 191);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 25);
            this.button5.TabIndex = 14;
            this.button5.Text = "更新可用的单元";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "项目地址";
            // 
            // ProjectAddressTextBox
            // 
            this.ProjectAddressTextBox.Location = new System.Drawing.Point(82, 119);
            this.ProjectAddressTextBox.Name = "ProjectAddressTextBox";
            this.ProjectAddressTextBox.Size = new System.Drawing.Size(240, 21);
            this.ProjectAddressTextBox.TabIndex = 18;
            // 
            // StageCodeComboBox
            // 
            this.StageCodeComboBox.FormattingEnabled = true;
            this.StageCodeComboBox.Location = new System.Drawing.Point(241, 21);
            this.StageCodeComboBox.Name = "StageCodeComboBox";
            this.StageCodeComboBox.Size = new System.Drawing.Size(67, 20);
            this.StageCodeComboBox.TabIndex = 21;
            // 
            // ProjectNoComboBox
            // 
            this.ProjectNoComboBox.FormattingEnabled = true;
            this.ProjectNoComboBox.Location = new System.Drawing.Point(82, 21);
            this.ProjectNoComboBox.Name = "ProjectNoComboBox";
            this.ProjectNoComboBox.Size = new System.Drawing.Size(100, 20);
            this.ProjectNoComboBox.TabIndex = 22;
            this.ProjectNoComboBox.SelectedIndexChanged += new System.EventHandler(this.ProjectNoComboBox_SelectedIndexChanged);
            // 
            // SearchProjectListProgressBar
            // 
            this.SearchProjectListProgressBar.Location = new System.Drawing.Point(3, 400);
            this.SearchProjectListProgressBar.Name = "SearchProjectListProgressBar";
            this.SearchProjectListProgressBar.Size = new System.Drawing.Size(330, 20);
            this.SearchProjectListProgressBar.Step = 1;
            this.SearchProjectListProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.SearchProjectListProgressBar.TabIndex = 23;
            this.SearchProjectListProgressBar.Visible = false;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressLabel.Location = new System.Drawing.Point(111, 382);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(95, 12);
            this.ProgressLabel.TabIndex = 24;
            this.ProgressLabel.Text = "正在读取文件...";
            this.ProgressLabel.Visible = false;
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 422);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.SearchProjectListProgressBar);
            this.Controls.Add(this.ProjectNoComboBox);
            this.Controls.Add(this.StageCodeComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ProjectAddressTextBox);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.GetAvailibleProjectButton);
            this.Controls.Add(this.SaveProjectButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UnitsListView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ProjectNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConstructureNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "NewProjectForm";
            this.Text = "NewProject";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ConstructureNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ProjectNameTextBox;
        private System.Windows.Forms.ListView UnitsListView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button SaveProjectButton;
        private System.Windows.Forms.Button GetAvailibleProjectButton;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ProjectAddressTextBox;
        private System.Windows.Forms.ComboBox StageCodeComboBox;
        private System.Windows.Forms.ComboBox ProjectNoComboBox;
        private System.Windows.Forms.ProgressBar SearchProjectListProgressBar;
        private System.Windows.Forms.Label ProgressLabel;
    }
}