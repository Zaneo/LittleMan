// The MIT License (MIT)
//
// Copyright (c) 2013 Gareth Higgins
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using LittleMan.Compilation;
using LittleMan.Emulation;
using LittleMan.IO;


namespace LittleMan.IDE {
    public partial class MainWindow : Form {

        enum FormTypes{
            Main,
            InputOutput,
            Build
        }
        public string projectName;

        Compiler compiler;
        InputHandler comHandler = new InputHandler(ProgramType.Compiler);
        //  Create method here that properly creates vm on another threadm I'll have to invoke all items after this. Thread vmThread = new Thread(() => 

        VirtualMachine vm;
        Thread vmThread;
        InputHandler virtHandler = new InputHandler(ProgramType.Computer);
        IOForm ioform;
        BuildForm buildform;

        public bool CodeHit { get; set; }

        public MainWindow() {
            InitializeComponent();
            dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewColumn {
                Name = "0x0",
                Width = 75,
                Visible = true,
                AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells,
                SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable,
                ValueType = typeof(ushort),
                CellTemplate = new DataGridViewTextBoxCell(),
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.DimGray }
            });
            for (int i = 0; i < 16; i++) {
                dataGridView1.Columns.Add(new System.Windows.Forms.DataGridViewColumn {
                    Name = i.ToString("X"),
                    Width = 75,
                    Visible = true,
                    AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells,
                    SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable,
                    ValueType = typeof(ushort),
                    CellTemplate = new DataGridViewTextBoxCell()
                });
                dataGridView1.Rows.Add(16);
                dataGridView1[0, i].Value = i.ToString("X");
            }

            buildform = new BuildForm();
            ioform = new IOForm();

            buildform.Name = "MainBuildForm";
            ioform.Name = "MainIOForm";

            buildform.Owner = this;
            ioform.Owner = this;


            compiler = new Compiler(buildform);
            vm = new VirtualMachine(ioform);
            Thread.CurrentThread.Name = "MainwindowThread";

            ioform.FormClosing += new FormClosingEventHandler(HideIOForm);
            buildform.FormClosing += new FormClosingEventHandler(HideBuildForm);
            buildform.VisibleChanged += new EventHandler(UpdateCheckedStateBuildForm);


            if (!Directory.Exists(Paths.CompiledDirectory))
                Directory.CreateDirectory(Paths.CompiledDirectory);

            EnvGrid.SelectedObject = vm;
            //dataGridView1.DataSource = vm.Memory;
            /*
            InputHandler test = new InputHandler(ProgramType.Compiler);
            test.HandleArgs(new string[] { "code.txt" });
            vm.OutputPath = "code.txt";
            compiler.SetProperties(ref test); */

            buildform.CreateControl();
            ioform.CreateControl();
            CodeHit = true;
        }
       
        public static void HideIOForm(object sender, FormClosingEventArgs e) {
            ((MainWindow)((IOForm)sender).Owner).ViewInputOutput.Checked = false;
        }
        public static void HideBuildForm(object sender, FormClosingEventArgs e) {
            ((MainWindow)((BuildForm)sender).Owner).ViewBuild.Checked = false;
        }
        public static void UpdateCheckedStateBuildForm(object sender, EventArgs e) {
            if (((BuildForm)sender).Visible) {
                ((MainWindow)((BuildForm)sender).Owner).ViewBuild.Checked = false;
            }
        }

        /*
        private void Form1_Load(object sender, EventArgs e) {
            buildform = new BuildForm();
            ioform = new IOForm();

            buildform.Name = "MainBuildForm";
            ioform.Name = "MainIOForm";            

            buildform.Owner = this;
            ioform.Owner = this;

            
            compiler = new Compiler(buildform);
            vm = new VirtualMachine(ioform);
            Thread.CurrentThread.Name = "MainwindowThread";

            ioform.FormClosing += new FormClosingEventHandler(HideIOForm);
            buildform.FormClosing += new FormClosingEventHandler(HideBuildForm);
            buildform.VisibleChanged += new EventHandler(UpdateCheckedStateBuildForm);


            if (!Directory.Exists(Paths.CompiledDirectory))
                Directory.CreateDirectory(Paths.CompiledDirectory);

            EnvGrid.SelectedObject = vm;
            //dataGridView1.DataSource = vm.Memory;
            InputHandler test = new InputHandler(ProgramType.Compiler);
            test.HandleArgs(new string[] {"I File code.txt"});
            vm.OutputPath = "code.txt";
            compiler.SetProperties(ref test);

            buildform.CreateControl();
            ioform.CreateControl();
            CodeHit = true;
        }*/

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e) {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                return;
            CodeBox.Text = string.Format("{0} : {1}", e.OldValue, e.NewValue);
            if (e.Type == ScrollEventType.LargeDecrement || e.Type == ScrollEventType.LargeIncrement) {
                dataGridView1.SuspendLayout();
                dataGridView1.ResumeLayout(true);
            }
        }        

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            dataGridView1.DataSource = null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        /// <summary>
        /// Build the program from input
        /// </summary>
        /// <param name="sender"> Object calling this method </param>
        /// <param name="e"> Event Parameters being passed in </param>
        private void buildProgramToolStripMenuItem_Click(object sender, EventArgs e) {
            buildform.ClearBuildText();
            if(File.Exists(Path.Combine(Paths.CompiledDirectory,"obj.stp")))
                File.Delete(Path.Combine(Paths.CompiledDirectory,"obj.stp"));

            if (File.Exists(Path.Combine(Paths.CompiledDirectory, Paths.DefaultCompiledFileName)))
                File.Delete(Path.Combine(Paths.CompiledDirectory, Paths.DefaultCompiledFileName));
            
            File.WriteAllText(Path.Combine(Paths.CompiledDirectory,"obj.stp"), CodeBox.Text);
            comHandler.HandleArgs(new string[] { Path.Combine(Paths.CompiledDirectory, "obj.stp") });
            comHandler.outputName = Path.Combine(Paths.CompiledDirectory, Paths.DefaultCompiledFileName);
            compiler.SetProperties(ref comHandler);
            compiler.CompileAll();
            File.WriteAllBytes(compiler.OutputPath, compiler.GetSourceAsRaw());

            //compiler.SetSource(CodeBox.Text.Split(new string[]{});
        }

        private void buildToolStripMenuItem1_CheckedChanged(object sender, EventArgs e) {
            if (ViewBuild.Checked || buildform.Visible == false) {
                ViewBuild.Checked = true;
                buildform.Show(this);
            }
            else {
                buildform.Hide();
            }
        }

        private void ViewInputOutput_CheckedChanged(object sender, EventArgs e) {
            if (ViewInputOutput.Checked || viewToolStripMenuItem.Visible == false) {
                ViewInputOutput.Checked = true;
                ioform.Show(this);
            }
            else {
                ioform.Hide();
            }
        }

        private void startWithoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!compiler.IsComplete){
                vm.RequestOUT("Source not compiled yet");
                return;
            }
            vm.SetProgram(compiler.GetSource());
            vmThread = new Thread(() => vm.ExecuteAll(false)) {Name = "VirtualMachineThread"};
            vmThread.Start();
            //vm.ExecuteAll();
        }

        private void startDebuggingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!compiler.IsComplete) {
                vm.RequestOUT("Source not compiled yet");
                return;
            }
            vm.SetProgram(compiler.GetSource());
            vmThread = new Thread(() => vm.ExecuteAll(true)) {Name = "VirtualMachineThread"};
            vmThread.Start();
        }
    }
}
 