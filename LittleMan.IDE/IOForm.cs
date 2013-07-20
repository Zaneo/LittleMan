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
using System.Threading;
using System.Windows.Forms;
using LittleMan.IO;

namespace LittleMan.IDE {
    public partial class IOForm : Form, IHumanInterface {
        ManualResetEvent vmMRE;
        bool waiting;
        delegate void ShowCallback();
        delegate void SelectCallback();
        delegate void OutputCallback(string text);

        public IOForm() {
            InitializeComponent();
            waiting = false;
        }
        
        #region HumanInterface Members

        public void Output<T>(T value) {
            if (OutputTextBox.InvokeRequired) {
                this.Invoke(new OutputCallback(OutputTextBox.AppendText), value.ToString() + Environment.NewLine);
            }
            else {
                OutputTextBox.AppendText(value.ToString() + Environment.NewLine);
            }
        }

        public void SetManualResetEvent(ref ManualResetEvent mre) {
            vmMRE = mre;   
        }

        public void PreFetch() {
            System.Diagnostics.Debug.Write("I am on: " + Thread.CurrentThread.Name + Environment.NewLine);
            if (vmMRE == null) {
                throw new ObjectDisposedException("The manual reset event must be initilaised first, call SetManualResetEvent.");
            }
            waiting = true;
            vmMRE.Reset();

            if (this.Visible == false) {
                if (this.InvokeRequired || string.Compare("MainWindowThread", Thread.CurrentThread.Name,true)!=0 ) {
                    this.Invoke(new ShowCallback(this.Show));
                }
                else {
                    this.Show();
                }
            }
            if (this.InvokeRequired || string.Compare("MainWindowThread", Thread.CurrentThread.Name, true) != 0) {
                this.Invoke(new SelectCallback(this.Select));
            }
            else{
                this.Select();
            }
        }
        public short Input() {
            short val;
            if (!short.TryParse(InputBox.Text, out val)) {
                throw new FormatException("Expected parsable short");
            }
            return val;
        }


        public void Advance() {
            return;
        }

        public void Cleanup() {
            throw new NotImplementedException();
        }

        #endregion

        private void IOForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (waiting) {
                vmMRE.Set();
            }
        }

        private void AdvanceProgramButton_Click(object sender, EventArgs e) {
            if (!waiting) {
                vmMRE.Set();
            }
        }

        private void IOForm_Load(object sender, EventArgs e) {
            System.Diagnostics.Debug.Write("I am on: " + Thread.CurrentThread.Name + Environment.NewLine);
        }
    }
}
