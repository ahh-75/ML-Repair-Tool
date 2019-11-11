namespace Consortio_Translation_Repair_Tool
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
            this.openBtn = new System.Windows.Forms.Button();
            this.pathLbl = new System.Windows.Forms.Label();
            this.executeBtn = new System.Windows.Forms.Button();
            this.countLbl = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.copyInfoLbl = new System.Windows.Forms.Label();
            this.TotalElapsedTimeDisplayLbl = new System.Windows.Forms.Label();
            this.ElapseTimerLbl = new System.Windows.Forms.Label();
            this.DisplayProgressOutputCb = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openBtn
            // 
            this.openBtn.Location = new System.Drawing.Point(6, 15);
            this.openBtn.Margin = new System.Windows.Forms.Padding(2);
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(78, 27);
            this.openBtn.TabIndex = 0;
            this.openBtn.Text = "Open";
            this.openBtn.UseVisualStyleBackColor = true;
            this.openBtn.Click += new System.EventHandler(this.OpenBtn_Click);
            // 
            // pathLbl
            // 
            this.pathLbl.AutoSize = true;
            this.pathLbl.Location = new System.Drawing.Point(100, 29);
            this.pathLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pathLbl.Name = "pathLbl";
            this.pathLbl.Size = new System.Drawing.Size(0, 13);
            this.pathLbl.TabIndex = 1;
            // 
            // executeBtn
            // 
            this.executeBtn.Location = new System.Drawing.Point(6, 64);
            this.executeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(78, 26);
            this.executeBtn.TabIndex = 2;
            this.executeBtn.Text = "Execute";
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // countLbl
            // 
            this.countLbl.AutoSize = true;
            this.countLbl.Location = new System.Drawing.Point(341, 208);
            this.countLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(0, 13);
            this.countLbl.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(26, 138);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(866, 377);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // copyInfoLbl
            // 
            this.copyInfoLbl.AutoSize = true;
            this.copyInfoLbl.Location = new System.Drawing.Point(100, 77);
            this.copyInfoLbl.Name = "copyInfoLbl";
            this.copyInfoLbl.Size = new System.Drawing.Size(0, 13);
            this.copyInfoLbl.TabIndex = 5;
            // 
            // TotalElapsedTimeDisplayLbl
            // 
            this.TotalElapsedTimeDisplayLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalElapsedTimeDisplayLbl.AutoSize = true;
            this.TotalElapsedTimeDisplayLbl.Location = new System.Drawing.Point(813, 15);
            this.TotalElapsedTimeDisplayLbl.Name = "TotalElapsedTimeDisplayLbl";
            this.TotalElapsedTimeDisplayLbl.Size = new System.Drawing.Size(31, 13);
            this.TotalElapsedTimeDisplayLbl.TabIndex = 6;
            this.TotalElapsedTimeDisplayLbl.Text = "--:--:--";
            // 
            // ElapseTimerLbl
            // 
            this.ElapseTimerLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ElapseTimerLbl.AutoSize = true;
            this.ElapseTimerLbl.Location = new System.Drawing.Point(706, 15);
            this.ElapseTimerLbl.Name = "ElapseTimerLbl";
            this.ElapseTimerLbl.Size = new System.Drawing.Size(101, 13);
            this.ElapseTimerLbl.TabIndex = 7;
            this.ElapseTimerLbl.Text = "Total ElapsedTime :";
            // 
            // DisplayProgressOutputCb
            // 
            this.DisplayProgressOutputCb.AutoSize = true;
            this.DisplayProgressOutputCb.Location = new System.Drawing.Point(6, 106);
            this.DisplayProgressOutputCb.Name = "DisplayProgressOutputCb";
            this.DisplayProgressOutputCb.Size = new System.Drawing.Size(136, 17);
            this.DisplayProgressOutputCb.TabIndex = 8;
            this.DisplayProgressOutputCb.Text = "Display progress output";
            this.DisplayProgressOutputCb.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 513);
            this.Controls.Add(this.DisplayProgressOutputCb);
            this.Controls.Add(this.ElapseTimerLbl);
            this.Controls.Add(this.TotalElapsedTimeDisplayLbl);
            this.Controls.Add(this.copyInfoLbl);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.countLbl);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.pathLbl);
            this.Controls.Add(this.openBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Consortio ML repair tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openBtn;
        private System.Windows.Forms.Label pathLbl;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.Label countLbl;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label copyInfoLbl;
        private System.Windows.Forms.Label TotalElapsedTimeDisplayLbl;
        private System.Windows.Forms.Label ElapseTimerLbl;
        private System.Windows.Forms.CheckBox DisplayProgressOutputCb;
    }
}

