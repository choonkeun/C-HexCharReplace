namespace SplitToLine
{
    partial class Form1
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSearchHEX = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSource = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnTarget = new System.Windows.Forms.Button();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtReplaceHEX = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoMMF = new System.Windows.Forms.RadioButton();
            this.rdoBypass = new System.Windows.Forms.RadioButton();
            this.rdoBuffer = new System.Windows.Forms.RadioButton();
            this.cbProgress = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSearchHEX);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(116, 52);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search HEX string";
            // 
            // txtSearchHEX
            // 
            this.txtSearchHEX.Location = new System.Drawing.Point(11, 19);
            this.txtSearchHEX.Name = "txtSearchHEX";
            this.txtSearchHEX.Size = new System.Drawing.Size(100, 20);
            this.txtSearchHEX.TabIndex = 0;
            this.txtSearchHEX.Text = "7e";
            this.txtSearchHEX.Leave += new System.EventHandler(this.txtSearchHEX_Leave);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSource);
            this.groupBox3.Controls.Add(this.txtSource);
            this.groupBox3.Location = new System.Drawing.Point(12, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(529, 52);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source File :";
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(7, 18);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(63, 23);
            this.btnSource.TabIndex = 1;
            this.btnSource.Text = "Source";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(76, 20);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(433, 20);
            this.txtSource.TabIndex = 0;
            this.txtSource.Text = "C:\\TEMP0\\DHCS834-DA-20180611-CO-506-001.dat";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnTarget);
            this.groupBox4.Controls.Add(this.txtTarget);
            this.groupBox4.Location = new System.Drawing.Point(12, 155);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(529, 52);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Target File :";
            // 
            // btnTarget
            // 
            this.btnTarget.Location = new System.Drawing.Point(7, 18);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(63, 23);
            this.btnTarget.TabIndex = 1;
            this.btnTarget.Text = "Target";
            this.btnTarget.UseVisualStyleBackColor = true;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(76, 20);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(433, 20);
            this.txtTarget.TabIndex = 0;
            this.txtTarget.Text = "C:\\TEMP0\\12.txt";
            // 
            // btnConvert
            // 
            this.btnConvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.Location = new System.Drawing.Point(461, 12);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(85, 52);
            this.btnConvert.TabIndex = 4;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtReplaceHEX);
            this.groupBox5.Location = new System.Drawing.Point(131, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(119, 52);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Replace HEX string";
            // 
            // txtReplaceHEX
            // 
            this.txtReplaceHEX.Location = new System.Drawing.Point(11, 19);
            this.txtReplaceHEX.Name = "txtReplaceHEX";
            this.txtReplaceHEX.Size = new System.Drawing.Size(100, 20);
            this.txtReplaceHEX.TabIndex = 0;
            this.txtReplaceHEX.Text = "7e 0d 0a";
            this.txtReplaceHEX.Leave += new System.EventHandler(this.txtReplaceHEX_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoMMF);
            this.groupBox1.Controls.Add(this.rdoBypass);
            this.groupBox1.Controls.Add(this.rdoBuffer);
            this.groupBox1.Location = new System.Drawing.Point(256, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 51);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process";
            // 
            // rdoMMF
            // 
            this.rdoMMF.AutoSize = true;
            this.rdoMMF.Checked = true;
            this.rdoMMF.Location = new System.Drawing.Point(132, 18);
            this.rdoMMF.Name = "rdoMMF";
            this.rdoMMF.Size = new System.Drawing.Size(49, 17);
            this.rdoMMF.TabIndex = 2;
            this.rdoMMF.TabStop = true;
            this.rdoMMF.Text = "MMF";
            this.rdoMMF.UseVisualStyleBackColor = true;
            // 
            // rdoBypass
            // 
            this.rdoBypass.AutoSize = true;
            this.rdoBypass.Location = new System.Drawing.Point(66, 18);
            this.rdoBypass.Name = "rdoBypass";
            this.rdoBypass.Size = new System.Drawing.Size(60, 17);
            this.rdoBypass.TabIndex = 1;
            this.rdoBypass.Text = "ByPass";
            this.rdoBypass.UseVisualStyleBackColor = true;
            // 
            // rdoBuffer
            // 
            this.rdoBuffer.AutoSize = true;
            this.rdoBuffer.Location = new System.Drawing.Point(7, 18);
            this.rdoBuffer.Name = "rdoBuffer";
            this.rdoBuffer.Size = new System.Drawing.Size(53, 17);
            this.rdoBuffer.TabIndex = 0;
            this.rdoBuffer.Text = "Buffer";
            this.rdoBuffer.UseVisualStyleBackColor = true;
            // 
            // cbProgress
            // 
            this.cbProgress.AutoSize = true;
            this.cbProgress.Checked = true;
            this.cbProgress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProgress.Location = new System.Drawing.Point(447, 68);
            this.cbProgress.Name = "cbProgress";
            this.cbProgress.Size = new System.Drawing.Size(104, 17);
            this.cbProgress.TabIndex = 6;
            this.cbProgress.Text = "Progress Display";
            this.cbProgress.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 248);
            this.Controls.Add(this.cbProgress);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SplitToLine v4.4-parallel";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearchHEX;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtReplaceHEX;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoBypass;
        private System.Windows.Forms.RadioButton rdoBuffer;
        private System.Windows.Forms.RadioButton rdoMMF;
        private System.Windows.Forms.CheckBox cbProgress;
    }
}
