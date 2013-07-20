using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
