namespace CLRGraph
{
    partial class DataSource_ClojureFunction_Config
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
            this.textBox_plot1 = new System.Windows.Forms.TextBox();
            this.label_primaryPlot = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.textBox_plot2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numericUpDown_precision = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_maxX = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown_minX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_precision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minX)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(624, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter clojure function[s] here that will be evaluated";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(624, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Valid variables to use in function: \'x\' (Current x coord), \'y\' (Current y coord i" +
    "f plotting a surface) and \'t\' (Current time if polling)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBox_plot1
            // 
            this.textBox_plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_plot1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_plot1.Location = new System.Drawing.Point(0, 23);
            this.textBox_plot1.Multiline = true;
            this.textBox_plot1.Name = "textBox_plot1";
            this.textBox_plot1.Size = new System.Drawing.Size(624, 141);
            this.textBox_plot1.TabIndex = 2;
            this.textBox_plot1.Text = "(CMath/Sin (+ x t))";
            // 
            // label_primaryPlot
            // 
            this.label_primaryPlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_primaryPlot.Location = new System.Drawing.Point(0, 0);
            this.label_primaryPlot.Name = "label_primaryPlot";
            this.label_primaryPlot.Size = new System.Drawing.Size(624, 23);
            this.label_primaryPlot.TabIndex = 3;
            this.label_primaryPlot.Text = "Primary plot: y = ";
            this.label_primaryPlot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 61);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.textBox_plot1);
            this.splitContainer.Panel1.Controls.Add(this.label_primaryPlot);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.textBox_plot2);
            this.splitContainer.Panel2.Controls.Add(this.label3);
            this.splitContainer.Size = new System.Drawing.Size(624, 350);
            this.splitContainer.SplitterDistance = 164;
            this.splitContainer.TabIndex = 4;
            // 
            // textBox_plot2
            // 
            this.textBox_plot2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_plot2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_plot2.Location = new System.Drawing.Point(0, 23);
            this.textBox_plot2.Multiline = true;
            this.textBox_plot2.Name = "textBox_plot2";
            this.textBox_plot2.Size = new System.Drawing.Size(624, 159);
            this.textBox_plot2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(624, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Secondary plot: z = ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_apply);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 411);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 31);
            this.panel1.TabIndex = 5;
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(456, 3);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 0;
            this.button_apply.Text = "Apply...";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(537, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numericUpDown_precision);
            this.panel2.Controls.Add(this.numericUpDown_maxX);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.numericUpDown_minX);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(624, 27);
            this.panel2.TabIndex = 6;
            // 
            // numericUpDown_precision
            // 
            this.numericUpDown_precision.DecimalPlaces = 4;
            this.numericUpDown_precision.Location = new System.Drawing.Point(412, 3);
            this.numericUpDown_precision.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDown_precision.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            720896});
            this.numericUpDown_precision.Name = "numericUpDown_precision";
            this.numericUpDown_precision.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_precision.TabIndex = 1;
            this.numericUpDown_precision.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // numericUpDown_maxX
            // 
            this.numericUpDown_maxX.DecimalPlaces = 4;
            this.numericUpDown_maxX.Location = new System.Drawing.Point(227, 3);
            this.numericUpDown_maxX.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDown_maxX.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDown_maxX.Name = "numericUpDown_maxX";
            this.numericUpDown_maxX.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_maxX.TabIndex = 1;
            this.numericUpDown_maxX.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(353, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Precision:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Max X:";
            // 
            // numericUpDown_minX
            // 
            this.numericUpDown_minX.DecimalPlaces = 4;
            this.numericUpDown_minX.Location = new System.Drawing.Point(55, 3);
            this.numericUpDown_minX.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.numericUpDown_minX.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.numericUpDown_minX.Name = "numericUpDown_minX";
            this.numericUpDown_minX.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_minX.TabIndex = 1;
            this.numericUpDown_minX.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Min X:";
            // 
            // DataSource_ClojureFunction_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DataSource_ClojureFunction_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clojure Function";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_precision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_plot1;
        private System.Windows.Forms.Label label_primaryPlot;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox textBox_plot2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numericUpDown_precision;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown_minX;
        private System.Windows.Forms.Label label4;
    }
}