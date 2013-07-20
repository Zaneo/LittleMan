using LittleMan.IO;

namespace LittleMan.IDE {
    partial class IOForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public IO.InputType Type { get; set; }
        public Emulation.InputFlag Flag { get; set; }

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
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.OutputBox = new System.Windows.Forms.GroupBox();
            this.InputBox = new System.Windows.Forms.GroupBox();
            this.ResetProgramButton = new System.Windows.Forms.Button();
            this.AdvanceProgramButton = new System.Windows.Forms.Button();
            this.OutputBox.SuspendLayout();
            this.InputBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(6, 19);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.Size = new System.Drawing.Size(247, 155);
            this.OutputTextBox.TabIndex = 0;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(12, 19);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(241, 20);
            this.InputTextBox.TabIndex = 1;
            // 
            // OutputBox
            // 
            this.OutputBox.Controls.Add(this.OutputTextBox);
            this.OutputBox.Location = new System.Drawing.Point(13, 12);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(259, 180);
            this.OutputBox.TabIndex = 2;
            this.OutputBox.TabStop = false;
            this.OutputBox.Text = "Output";
            // 
            // InputBox
            // 
            this.InputBox.Controls.Add(this.InputTextBox);
            this.InputBox.Location = new System.Drawing.Point(19, 239);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(247, 52);
            this.InputBox.TabIndex = 3;
            this.InputBox.TabStop = false;
            this.InputBox.Text = "Input";
            // 
            // ResetProgramButton
            // 
            this.ResetProgramButton.Location = new System.Drawing.Point(31, 198);
            this.ResetProgramButton.Name = "ResetProgramButton";
            this.ResetProgramButton.Size = new System.Drawing.Size(75, 23);
            this.ResetProgramButton.TabIndex = 4;
            this.ResetProgramButton.Text = "Reset";
            this.ResetProgramButton.UseVisualStyleBackColor = true;
            // 
            // AdvanceProgramButton
            // 
            this.AdvanceProgramButton.Location = new System.Drawing.Point(201, 198);
            this.AdvanceProgramButton.Name = "AdvanceProgramButton";
            this.AdvanceProgramButton.Size = new System.Drawing.Size(75, 23);
            this.AdvanceProgramButton.TabIndex = 5;
            this.AdvanceProgramButton.Text = "Advance";
            this.AdvanceProgramButton.UseVisualStyleBackColor = true;
            this.AdvanceProgramButton.Click += new System.EventHandler(this.AdvanceProgramButton_Click);
            // 
            // IOForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 303);
            this.Controls.Add(this.AdvanceProgramButton);
            this.Controls.Add(this.ResetProgramButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.OutputBox);
            this.Name = "IOForm";
            this.Text = "Input/Output";
            this.Load += new System.EventHandler(this.IOForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IOForm_FormClosing);
            this.OutputBox.ResumeLayout(false);
            this.OutputBox.PerformLayout();
            this.InputBox.ResumeLayout(false);
            this.InputBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.GroupBox OutputBox;
        private System.Windows.Forms.GroupBox InputBox;
        private System.Windows.Forms.Button ResetProgramButton;
        private System.Windows.Forms.Button AdvanceProgramButton;
    }
}