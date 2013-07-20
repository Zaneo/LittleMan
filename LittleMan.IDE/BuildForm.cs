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
using System.Windows.Forms;
using LittleMan.IO;


namespace LittleMan.IDE {
    public partial class BuildForm : Form , IHumanInterface {
        public BuildForm() {
            InitializeComponent();
        }

        public void ClearBuildText() {
            BuildBox.Clear();
        }

        #region IHumanInterface Members
        // event to update main form checked.
        public void Output<T>(T value) {
            if (this.Visible == false) {
                this.Show();
                this.Select();
            }
            BuildBox.Text += value.ToString();
        }

        public void SetManualResetEvent(ref System.Threading.ManualResetEvent mre) {
            throw new NotSupportedException();
        }
        public void PreFetch() {
            throw new NotSupportedException();
        }

        public short Input() {
            throw new NotSupportedException();
        }

        public void Advance() {
            throw new NotSupportedException();
        }

        public void Cleanup() {
            throw new NotSupportedException();
        }

        #endregion

        private void BuildForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void BuildBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Escape) {
                this.Hide();
            }
        }
    }
}
