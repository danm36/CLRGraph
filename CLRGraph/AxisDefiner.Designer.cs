namespace CLRGraph
{
    partial class AxisDefiner
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
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.listBox_XAxis = new System.Windows.Forms.ListBox();
            this.listBox_ZAxis = new System.Windows.Forms.ListBox();
            this.listBox_YAxis = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(377, 287);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 0;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(296, 287);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 23);
            this.button_Apply.TabIndex = 1;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // listBox_XAxis
            // 
            this.listBox_XAxis.FormattingEnabled = true;
            this.listBox_XAxis.Location = new System.Drawing.Point(12, 57);
            this.listBox_XAxis.Name = "listBox_XAxis";
            this.listBox_XAxis.Size = new System.Drawing.Size(133, 225);
            this.listBox_XAxis.TabIndex = 2;
            // 
            // listBox_ZAxis
            // 
            this.listBox_ZAxis.FormattingEnabled = true;
            this.listBox_ZAxis.Location = new System.Drawing.Point(319, 57);
            this.listBox_ZAxis.Name = "listBox_ZAxis";
            this.listBox_ZAxis.Size = new System.Drawing.Size(133, 225);
            this.listBox_ZAxis.TabIndex = 4;
            // 
            // listBox_YAxis
            // 
            this.listBox_YAxis.FormattingEnabled = true;
            this.listBox_YAxis.Location = new System.Drawing.Point(165, 57);
            this.listBox_YAxis.Name = "listBox_YAxis";
            this.listBox_YAxis.Size = new System.Drawing.Size(133, 225);
            this.listBox_YAxis.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "X-Axis:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(270, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Please select the data columns to be used for each axis";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Y-Axis:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(316, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Z-Axis:";
            // 
            // AxisDefiner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 322);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_YAxis);
            this.Controls.Add(this.listBox_ZAxis);
            this.Controls.Add(this.listBox_XAxis);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.button_Cancel);
            this.Name = "AxisDefiner";
            this.Text = "Axis Definer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.ListBox listBox_XAxis;
        private System.Windows.Forms.ListBox listBox_ZAxis;
        private System.Windows.Forms.ListBox listBox_YAxis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}