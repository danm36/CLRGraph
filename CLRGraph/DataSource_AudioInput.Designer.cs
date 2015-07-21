namespace CLRGraph
{
    partial class DataSource_AudioInput_Config
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
            this.listView_Devices = new System.Windows.Forms.ListView();
            this.columnHeader_DeviceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ChannelCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_Devices
            // 
            this.listView_Devices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_Devices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_DeviceID,
            this.columnHeader_DeviceName,
            this.columnHeader_ChannelCount});
            this.listView_Devices.FullRowSelect = true;
            this.listView_Devices.Location = new System.Drawing.Point(12, 25);
            this.listView_Devices.MultiSelect = false;
            this.listView_Devices.Name = "listView_Devices";
            this.listView_Devices.Size = new System.Drawing.Size(440, 219);
            this.listView_Devices.TabIndex = 0;
            this.listView_Devices.UseCompatibleStateImageBehavior = false;
            this.listView_Devices.View = System.Windows.Forms.View.Details;
            this.listView_Devices.DoubleClick += new System.EventHandler(this.listView_Devices_DoubleClick);
            // 
            // columnHeader_DeviceID
            // 
            this.columnHeader_DeviceID.Text = "ID";
            this.columnHeader_DeviceID.Width = 40;
            // 
            // columnHeader_DeviceName
            // 
            this.columnHeader_DeviceName.Text = "Name";
            this.columnHeader_DeviceName.Width = 340;
            // 
            // columnHeader_ChannelCount
            // 
            this.columnHeader_ChannelCount.Text = "Channels";
            this.columnHeader_ChannelCount.Width = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected Audio Input:";
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.Location = new System.Drawing.Point(377, 250);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_Apply
            // 
            this.button_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Apply.Location = new System.Drawing.Point(296, 250);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 23);
            this.button_Apply.TabIndex = 2;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // DataSource_AudioInput_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 282);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_Devices);
            this.Name = "DataSource_AudioInput_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Input Source";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_Devices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.ColumnHeader columnHeader_DeviceID;
        private System.Windows.Forms.ColumnHeader columnHeader_DeviceName;
        private System.Windows.Forms.ColumnHeader columnHeader_ChannelCount;
    }
}