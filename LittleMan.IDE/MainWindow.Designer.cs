namespace LittleMan.IDE {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.CodeBox = new System.Windows.Forms.TextBox();
            this.EnvGrid = new System.Windows.Forms.PropertyGrid();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.savelAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewInputOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.startDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startWithoutDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exceptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // CodeBox
            // 
            this.CodeBox.BackColor = System.Drawing.SystemColors.Window;
            this.CodeBox.Location = new System.Drawing.Point(345, 27);
            this.CodeBox.Multiline = true;
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(246, 281);
            this.CodeBox.TabIndex = 0;
            // 
            // EnvGrid
            // 
            this.EnvGrid.Location = new System.Drawing.Point(345, 314);
            this.EnvGrid.Name = "EnvGrid";
            this.EnvGrid.Size = new System.Drawing.Size(246, 156);
            this.EnvGrid.TabIndex = 6;
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(603, 24);
            this.MainMenuStrip.TabIndex = 7;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.savelAllToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(115, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(115, 6);
            // 
            // savelAllToolStripMenuItem
            // 
            this.savelAllToolStripMenuItem.Name = "savelAllToolStripMenuItem";
            this.savelAllToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.savelAllToolStripMenuItem.Text = "Savel A&ll";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewInputOutput,
            this.ViewBuild});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // ViewInputOutput
            // 
            this.ViewInputOutput.CheckOnClick = true;
            this.ViewInputOutput.Name = "ViewInputOutput";
            this.ViewInputOutput.Size = new System.Drawing.Size(145, 22);
            this.ViewInputOutput.Text = "Input/Output";
            this.ViewInputOutput.CheckedChanged += new System.EventHandler(this.ViewInputOutput_CheckedChanged);
            // 
            // ViewBuild
            // 
            this.ViewBuild.CheckOnClick = true;
            this.ViewBuild.Name = "ViewBuild";
            this.ViewBuild.Size = new System.Drawing.Size(145, 22);
            this.ViewBuild.Text = "Build";
            this.ViewBuild.CheckedChanged += new System.EventHandler(this.buildToolStripMenuItem1_CheckedChanged);
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildProgramToolStripMenuItem});
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.buildToolStripMenuItem.Text = "&Build";
            // 
            // buildProgramToolStripMenuItem
            // 
            this.buildProgramToolStripMenuItem.Name = "buildProgramToolStripMenuItem";
            this.buildProgramToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.buildProgramToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.buildProgramToolStripMenuItem.Text = "Build Program";
            this.buildProgramToolStripMenuItem.Click += new System.EventHandler(this.buildProgramToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.toolStripSeparator3,
            this.startDebuggingToolStripMenuItem,
            this.startWithoutDebuggingToolStripMenuItem,
            this.toolStripSeparator4,
            this.exceptionsToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(249, 6);
            // 
            // startDebuggingToolStripMenuItem
            // 
            this.startDebuggingToolStripMenuItem.Name = "startDebuggingToolStripMenuItem";
            this.startDebuggingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startDebuggingToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.startDebuggingToolStripMenuItem.Text = "&Start Debugging";
            this.startDebuggingToolStripMenuItem.Click += new System.EventHandler(this.startDebuggingToolStripMenuItem_Click);
            // 
            // startWithoutDebuggingToolStripMenuItem
            // 
            this.startWithoutDebuggingToolStripMenuItem.Name = "startWithoutDebuggingToolStripMenuItem";
            this.startWithoutDebuggingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.startWithoutDebuggingToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.startWithoutDebuggingToolStripMenuItem.Text = "Start Without Debugging";
            this.startWithoutDebuggingToolStripMenuItem.Click += new System.EventHandler(this.startWithoutDebuggingToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(249, 6);
            // 
            // exceptionsToolStripMenuItem
            // 
            this.exceptionsToolStripMenuItem.Name = "exceptionsToolStripMenuItem";
            this.exceptionsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.exceptionsToolStripMenuItem.Text = "Exceptions";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(327, 442);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 482);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.EnvGrid);
            this.Controls.Add(this.CodeBox);
            this.Controls.Add(this.MainMenuStrip);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.Text = "Little Man IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CodeBox;
        private System.Windows.Forms.PropertyGrid EnvGrid;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savelAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem startDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startWithoutDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exceptionsToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewInputOutput;
        private System.Windows.Forms.ToolStripMenuItem ViewBuild;
    }
}

