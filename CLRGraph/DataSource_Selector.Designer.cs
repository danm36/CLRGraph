namespace CLRGraph
{
    partial class DataSource_Selector
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
            this.listView_DataSourceTypes = new System.Windows.Forms.ListView();
            this.columnHeader_DataSourceType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DataSourceCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SourceName = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_AddSource = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Data Source Type:";
            // 
            // listView_DataSourceTypes
            // 
            this.listView_DataSourceTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_DataSourceType,
            this.columnHeader_DataSourceCategory});
            this.listView_DataSourceTypes.FullRowSelect = true;
            this.listView_DataSourceTypes.Location = new System.Drawing.Point(12, 25);
            this.listView_DataSourceTypes.MultiSelect = false;
            this.listView_DataSourceTypes.Name = "listView_DataSourceTypes";
            this.listView_DataSourceTypes.Size = new System.Drawing.Size(280, 350);
            this.listView_DataSourceTypes.TabIndex = 1;
            this.listView_DataSourceTypes.UseCompatibleStateImageBehavior = false;
            this.listView_DataSourceTypes.View = System.Windows.Forms.View.Details;
            this.listView_DataSourceTypes.DoubleClick += new System.EventHandler(this.listView_DataSourceTypes_DoubleClick);
            // 
            // columnHeader_DataSourceType
            // 
            this.columnHeader_DataSourceType.Text = "Source Type";
            this.columnHeader_DataSourceType.Width = 160;
            // 
            // columnHeader_DataSourceCategory
            // 
            this.columnHeader_DataSourceCategory.Text = "Source Category";
            this.columnHeader_DataSourceCategory.Width = 115;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source Name:";
            // 
            // textBox_SourceName
            // 
            this.textBox_SourceName.Location = new System.Drawing.Point(93, 381);
            this.textBox_SourceName.Name = "textBox_SourceName";
            this.textBox_SourceName.Size = new System.Drawing.Size(199, 20);
            this.textBox_SourceName.TabIndex = 3;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(217, 407);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_AddSource
            // 
            this.button_AddSource.Location = new System.Drawing.Point(136, 407);
            this.button_AddSource.Name = "button_AddSource";
            this.button_AddSource.Size = new System.Drawing.Size(75, 23);
            this.button_AddSource.TabIndex = 5;
            this.button_AddSource.Text = "Add Source";
            this.button_AddSource.UseVisualStyleBackColor = true;
            this.button_AddSource.Click += new System.EventHandler(this.button_AddSource_Click);
            // 
            // DataSource_Selector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 442);
            this.Controls.Add(this.button_AddSource);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.textBox_SourceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView_DataSourceTypes);
            this.Controls.Add(this.label1);
            this.Name = "DataSource_Selector";
            this.Text = "Data Source Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView_DataSourceTypes;
        private System.Windows.Forms.ColumnHeader columnHeader_DataSourceType;
        private System.Windows.Forms.ColumnHeader columnHeader_DataSourceCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SourceName;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_AddSource;
    }
}