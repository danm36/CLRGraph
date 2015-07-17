namespace CLRGraph
{
    partial class CLRGraph_MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CLRGraph_MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.FPSCounterLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.glGraph = new CLRGraph.GLGraph();
            this.splitContainer_Right = new System.Windows.Forms.SplitContainer();
            this.tabControl_sidebar = new System.Windows.Forms.TabControl();
            this.tabPage_GraphScript = new System.Windows.Forms.TabPage();
            this.textBox_GraphScript = new System.Windows.Forms.TextBox();
            this.toolStrip_GraphScript = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_NewGraphScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_OpenGraphScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SaveGraphScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SaveAsGraphScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_CompileGraphScript = new System.Windows.Forms.ToolStripButton();
            this.tabPage_Series = new System.Windows.Forms.TabPage();
            this.listView_series = new System.Windows.Forms.ListView();
            this.columnHeader_Visible = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Current = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Color = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_PointCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_DataSource = new System.Windows.Forms.TabPage();
            this.listView_DataSources = new System.Windows.Forms.ListView();
            this.columnHeader_DataSourceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DataSourceType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DataSourceLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_AddDataSource = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_RemoveDataSource = new System.Windows.Forms.ToolStripButton();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.splitter_Log = new System.Windows.Forms.Splitter();
            this.textBox_RuntimeREPL = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ClearLog = new System.Windows.Forms.Button();
            this.button_ExecuteRuntimeInput = new System.Windows.Forms.Button();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip_GraphControl = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton_LineThickness = new System.Windows.Forms.ToolStripDropDownButton();
            this.lineWidth1pxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineWidth2pxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lineWidth3pxToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.lineWidth4pxToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.lineWidth5pxToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton_GraphType = new System.Windows.Forms.ToolStripDropDownButton();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.linesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trianglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.connectedLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton_ShaderMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.solidLightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceFogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.worldSpaceCoordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundingCoordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton_RenderTransparency = new System.Windows.Forms.ToolStripDropDownButton();
            this.solidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stippleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ShowAxes = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ShowAxesInColor = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_RenderCameraControl = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_RenderMode2D = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_RenderMode3DOrth = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_RenderMode3DPersp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ResetCameraFocus = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ResetCamera = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Right)).BeginInit();
            this.splitContainer_Right.Panel1.SuspendLayout();
            this.splitContainer_Right.Panel2.SuspendLayout();
            this.splitContainer_Right.SuspendLayout();
            this.tabControl_sidebar.SuspendLayout();
            this.tabPage_GraphScript.SuspendLayout();
            this.toolStrip_GraphScript.SuspendLayout();
            this.tabPage_Series.SuspendLayout();
            this.tabPage_DataSource.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStrip_GraphControl.SuspendLayout();
            this.toolStrip_RenderCameraControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FPSCounterLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // FPSCounterLabel
            // 
            this.FPSCounterLabel.Name = "FPSCounterLabel";
            this.FPSCounterLabel.Size = new System.Drawing.Size(66, 17);
            this.FPSCounterLabel.Text = "Last FPS: ??";
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.glGraph);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.splitContainer_Right);
            this.splitContainer_Main.Size = new System.Drawing.Size(784, 466);
            this.splitContainer_Main.SplitterDistance = 413;
            this.splitContainer_Main.TabIndex = 2;
            // 
            // glGraph
            // 
            this.glGraph.BackColor = System.Drawing.Color.White;
            this.glGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glGraph.Location = new System.Drawing.Point(0, 0);
            this.glGraph.Name = "glGraph";
            this.glGraph.Size = new System.Drawing.Size(413, 466);
            this.glGraph.TabIndex = 0;
            // 
            // splitContainer_Right
            // 
            this.splitContainer_Right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Right.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Right.Name = "splitContainer_Right";
            this.splitContainer_Right.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Right.Panel1
            // 
            this.splitContainer_Right.Panel1.Controls.Add(this.tabControl_sidebar);
            // 
            // splitContainer_Right.Panel2
            // 
            this.splitContainer_Right.Panel2.Controls.Add(this.textBox_Log);
            this.splitContainer_Right.Panel2.Controls.Add(this.splitter_Log);
            this.splitContainer_Right.Panel2.Controls.Add(this.textBox_RuntimeREPL);
            this.splitContainer_Right.Panel2.Controls.Add(this.panel1);
            this.splitContainer_Right.Size = new System.Drawing.Size(367, 466);
            this.splitContainer_Right.SplitterDistance = 243;
            this.splitContainer_Right.TabIndex = 0;
            // 
            // tabControl_sidebar
            // 
            this.tabControl_sidebar.Controls.Add(this.tabPage_GraphScript);
            this.tabControl_sidebar.Controls.Add(this.tabPage_Series);
            this.tabControl_sidebar.Controls.Add(this.tabPage_DataSource);
            this.tabControl_sidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_sidebar.Location = new System.Drawing.Point(0, 0);
            this.tabControl_sidebar.Name = "tabControl_sidebar";
            this.tabControl_sidebar.SelectedIndex = 0;
            this.tabControl_sidebar.Size = new System.Drawing.Size(367, 243);
            this.tabControl_sidebar.TabIndex = 0;
            this.tabControl_sidebar.SelectedIndexChanged += new System.EventHandler(this.tabControl_sidebar_SelectedIndexChanged);
            // 
            // tabPage_GraphScript
            // 
            this.tabPage_GraphScript.Controls.Add(this.textBox_GraphScript);
            this.tabPage_GraphScript.Controls.Add(this.toolStrip_GraphScript);
            this.tabPage_GraphScript.Location = new System.Drawing.Point(4, 22);
            this.tabPage_GraphScript.Name = "tabPage_GraphScript";
            this.tabPage_GraphScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_GraphScript.Size = new System.Drawing.Size(359, 217);
            this.tabPage_GraphScript.TabIndex = 1;
            this.tabPage_GraphScript.Text = "Graph Script";
            this.tabPage_GraphScript.UseVisualStyleBackColor = true;
            // 
            // textBox_GraphScript
            // 
            this.textBox_GraphScript.AcceptsReturn = true;
            this.textBox_GraphScript.AcceptsTab = true;
            this.textBox_GraphScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_GraphScript.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_GraphScript.Location = new System.Drawing.Point(3, 28);
            this.textBox_GraphScript.Multiline = true;
            this.textBox_GraphScript.Name = "textBox_GraphScript";
            this.textBox_GraphScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_GraphScript.Size = new System.Drawing.Size(353, 186);
            this.textBox_GraphScript.TabIndex = 1;
            this.textBox_GraphScript.WordWrap = false;
            // 
            // toolStrip_GraphScript
            // 
            this.toolStrip_GraphScript.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_GraphScript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_NewGraphScript,
            this.toolStripButton_OpenGraphScript,
            this.toolStripButton_SaveGraphScript,
            this.toolStripButton_SaveAsGraphScript,
            this.toolStripSeparator1,
            this.toolStripButton_CompileGraphScript});
            this.toolStrip_GraphScript.Location = new System.Drawing.Point(3, 3);
            this.toolStrip_GraphScript.Name = "toolStrip_GraphScript";
            this.toolStrip_GraphScript.Size = new System.Drawing.Size(353, 25);
            this.toolStrip_GraphScript.TabIndex = 0;
            this.toolStrip_GraphScript.Text = "Graph Script Toolstrip";
            // 
            // toolStripButton_NewGraphScript
            // 
            this.toolStripButton_NewGraphScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_NewGraphScript.Image")));
            this.toolStripButton_NewGraphScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_NewGraphScript.Name = "toolStripButton_NewGraphScript";
            this.toolStripButton_NewGraphScript.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_NewGraphScript.Text = "New";
            this.toolStripButton_NewGraphScript.Click += new System.EventHandler(this.toolStripButton_NewGraphScript_Click);
            // 
            // toolStripButton_OpenGraphScript
            // 
            this.toolStripButton_OpenGraphScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_OpenGraphScript.Image")));
            this.toolStripButton_OpenGraphScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_OpenGraphScript.Name = "toolStripButton_OpenGraphScript";
            this.toolStripButton_OpenGraphScript.Size = new System.Drawing.Size(56, 22);
            this.toolStripButton_OpenGraphScript.Text = "Open";
            this.toolStripButton_OpenGraphScript.Click += new System.EventHandler(this.toolStripButton_OpenGraphScript_Click);
            // 
            // toolStripButton_SaveGraphScript
            // 
            this.toolStripButton_SaveGraphScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SaveGraphScript.Image")));
            this.toolStripButton_SaveGraphScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SaveGraphScript.Name = "toolStripButton_SaveGraphScript";
            this.toolStripButton_SaveGraphScript.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_SaveGraphScript.Text = "Save";
            this.toolStripButton_SaveGraphScript.Click += new System.EventHandler(this.toolStripButton_SaveGraphScript_Click);
            // 
            // toolStripButton_SaveAsGraphScript
            // 
            this.toolStripButton_SaveAsGraphScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SaveAsGraphScript.Image")));
            this.toolStripButton_SaveAsGraphScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SaveAsGraphScript.Name = "toolStripButton_SaveAsGraphScript";
            this.toolStripButton_SaveAsGraphScript.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton_SaveAsGraphScript.Text = "Save As...";
            this.toolStripButton_SaveAsGraphScript.Click += new System.EventHandler(this.toolStripButton_SaveAsGraphScript_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_CompileGraphScript
            // 
            this.toolStripButton_CompileGraphScript.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CompileGraphScript.Image")));
            this.toolStripButton_CompileGraphScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CompileGraphScript.Name = "toolStripButton_CompileGraphScript";
            this.toolStripButton_CompileGraphScript.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton_CompileGraphScript.Text = "Compile";
            this.toolStripButton_CompileGraphScript.Click += new System.EventHandler(this.toolStripButton_CompileGraphScript_Click);
            // 
            // tabPage_Series
            // 
            this.tabPage_Series.Controls.Add(this.listView_series);
            this.tabPage_Series.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Series.Name = "tabPage_Series";
            this.tabPage_Series.Size = new System.Drawing.Size(359, 217);
            this.tabPage_Series.TabIndex = 2;
            this.tabPage_Series.Text = "Series";
            this.tabPage_Series.UseVisualStyleBackColor = true;
            // 
            // listView_series
            // 
            this.listView_series.CheckBoxes = true;
            this.listView_series.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Visible,
            this.columnHeader_Current,
            this.columnHeader_ID,
            this.columnHeader_Color,
            this.columnHeader_Name,
            this.columnHeader_PointCount});
            this.listView_series.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_series.FullRowSelect = true;
            this.listView_series.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_series.Location = new System.Drawing.Point(0, 0);
            this.listView_series.MultiSelect = false;
            this.listView_series.Name = "listView_series";
            this.listView_series.Size = new System.Drawing.Size(359, 217);
            this.listView_series.TabIndex = 0;
            this.listView_series.UseCompatibleStateImageBehavior = false;
            this.listView_series.View = System.Windows.Forms.View.Details;
            this.listView_series.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_series_ItemChecked);
            this.listView_series.SelectedIndexChanged += new System.EventHandler(this.listView_series_SelectedIndexChanged);
            // 
            // columnHeader_Visible
            // 
            this.columnHeader_Visible.Text = "Vis";
            this.columnHeader_Visible.Width = 30;
            // 
            // columnHeader_Current
            // 
            this.columnHeader_Current.Text = "Cur";
            this.columnHeader_Current.Width = 30;
            // 
            // columnHeader_ID
            // 
            this.columnHeader_ID.Text = "ID";
            this.columnHeader_ID.Width = 40;
            // 
            // columnHeader_Color
            // 
            this.columnHeader_Color.Text = "";
            this.columnHeader_Color.Width = 30;
            // 
            // columnHeader_Name
            // 
            this.columnHeader_Name.Text = "Name";
            this.columnHeader_Name.Width = 140;
            // 
            // columnHeader_PointCount
            // 
            this.columnHeader_PointCount.Text = "Points";
            // 
            // tabPage_DataSource
            // 
            this.tabPage_DataSource.Controls.Add(this.listView_DataSources);
            this.tabPage_DataSource.Controls.Add(this.toolStrip1);
            this.tabPage_DataSource.Location = new System.Drawing.Point(4, 22);
            this.tabPage_DataSource.Name = "tabPage_DataSource";
            this.tabPage_DataSource.Size = new System.Drawing.Size(359, 217);
            this.tabPage_DataSource.TabIndex = 3;
            this.tabPage_DataSource.Text = "Data Sources";
            this.tabPage_DataSource.UseVisualStyleBackColor = true;
            // 
            // listView_DataSources
            // 
            this.listView_DataSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_DataSourceName,
            this.columnHeader_DataSourceType,
            this.columnHeader_DataSourceLocation});
            this.listView_DataSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_DataSources.FullRowSelect = true;
            this.listView_DataSources.Location = new System.Drawing.Point(0, 25);
            this.listView_DataSources.MultiSelect = false;
            this.listView_DataSources.Name = "listView_DataSources";
            this.listView_DataSources.Size = new System.Drawing.Size(359, 192);
            this.listView_DataSources.TabIndex = 1;
            this.listView_DataSources.UseCompatibleStateImageBehavior = false;
            this.listView_DataSources.View = System.Windows.Forms.View.Details;
            this.listView_DataSources.DoubleClick += new System.EventHandler(this.listView_DataSources_DoubleClick);
            // 
            // columnHeader_DataSourceName
            // 
            this.columnHeader_DataSourceName.Text = "Name";
            this.columnHeader_DataSourceName.Width = 100;
            // 
            // columnHeader_DataSourceType
            // 
            this.columnHeader_DataSourceType.Text = "Type";
            this.columnHeader_DataSourceType.Width = 100;
            // 
            // columnHeader_DataSourceLocation
            // 
            this.columnHeader_DataSourceLocation.Text = "Location";
            this.columnHeader_DataSourceLocation.Width = 250;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_AddDataSource,
            this.toolStripButton_RemoveDataSource});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(359, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton_AddDataSource
            // 
            this.toolStripButton_AddDataSource.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AddDataSource.Image")));
            this.toolStripButton_AddDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddDataSource.Name = "toolStripButton_AddDataSource";
            this.toolStripButton_AddDataSource.Size = new System.Drawing.Size(115, 22);
            this.toolStripButton_AddDataSource.Text = "Add Data Source";
            this.toolStripButton_AddDataSource.Click += new System.EventHandler(this.toolStripButton_AddDataSource_Click);
            // 
            // toolStripButton_RemoveDataSource
            // 
            this.toolStripButton_RemoveDataSource.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveDataSource.Image")));
            this.toolStripButton_RemoveDataSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveDataSource.Name = "toolStripButton_RemoveDataSource";
            this.toolStripButton_RemoveDataSource.Size = new System.Drawing.Size(136, 22);
            this.toolStripButton_RemoveDataSource.Text = "Remove Data Source";
            this.toolStripButton_RemoveDataSource.Click += new System.EventHandler(this.toolStripButton_RemoveDataSource_Click);
            // 
            // textBox_Log
            // 
            this.textBox_Log.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Log.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Log.Location = new System.Drawing.Point(0, 0);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(367, 117);
            this.textBox_Log.TabIndex = 0;
            this.textBox_Log.WordWrap = false;
            // 
            // splitter_Log
            // 
            this.splitter_Log.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter_Log.Location = new System.Drawing.Point(0, 117);
            this.splitter_Log.Name = "splitter_Log";
            this.splitter_Log.Size = new System.Drawing.Size(367, 3);
            this.splitter_Log.TabIndex = 2;
            this.splitter_Log.TabStop = false;
            // 
            // textBox_RuntimeREPL
            // 
            this.textBox_RuntimeREPL.AcceptsReturn = true;
            this.textBox_RuntimeREPL.AcceptsTab = true;
            this.textBox_RuntimeREPL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_RuntimeREPL.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_RuntimeREPL.Location = new System.Drawing.Point(0, 120);
            this.textBox_RuntimeREPL.Multiline = true;
            this.textBox_RuntimeREPL.Name = "textBox_RuntimeREPL";
            this.textBox_RuntimeREPL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_RuntimeREPL.Size = new System.Drawing.Size(367, 69);
            this.textBox_RuntimeREPL.TabIndex = 1;
            this.textBox_RuntimeREPL.WordWrap = false;
            this.textBox_RuntimeREPL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_RuntimeREPL_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_ClearLog);
            this.panel1.Controls.Add(this.button_ExecuteRuntimeInput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 189);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 30);
            this.panel1.TabIndex = 3;
            // 
            // button_ClearLog
            // 
            this.button_ClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ClearLog.Location = new System.Drawing.Point(4, 3);
            this.button_ClearLog.Name = "button_ClearLog";
            this.button_ClearLog.Size = new System.Drawing.Size(75, 23);
            this.button_ClearLog.TabIndex = 1;
            this.button_ClearLog.Text = "Clear Log";
            this.button_ClearLog.UseVisualStyleBackColor = true;
            this.button_ClearLog.Click += new System.EventHandler(this.button_ClearLog_Click);
            // 
            // button_ExecuteRuntimeInput
            // 
            this.button_ExecuteRuntimeInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ExecuteRuntimeInput.Location = new System.Drawing.Point(271, 3);
            this.button_ExecuteRuntimeInput.Name = "button_ExecuteRuntimeInput";
            this.button_ExecuteRuntimeInput.Size = new System.Drawing.Size(92, 23);
            this.button_ExecuteRuntimeInput.TabIndex = 0;
            this.button_ExecuteRuntimeInput.Text = "Execute";
            this.button_ExecuteRuntimeInput.UseVisualStyleBackColor = true;
            this.button_ExecuteRuntimeInput.Click += new System.EventHandler(this.button_ExecuteRuntimeInput_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer_Main);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(784, 466);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(784, 516);
            this.toolStripContainer.TabIndex = 3;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip_GraphControl);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip_RenderCameraControl);
            // 
            // toolStrip_GraphControl
            // 
            this.toolStrip_GraphControl.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_GraphControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton_LineThickness,
            this.toolStripDropDownButton_GraphType,
            this.toolStripDropDownButton_ShaderMode,
            this.toolStripDropDownButton_RenderTransparency,
            this.toolStripSeparator5,
            this.toolStripButton_ShowAxes,
            this.toolStripButton_ShowAxesInColor});
            this.toolStrip_GraphControl.Location = new System.Drawing.Point(3, 0);
            this.toolStrip_GraphControl.Name = "toolStrip_GraphControl";
            this.toolStrip_GraphControl.Size = new System.Drawing.Size(180, 25);
            this.toolStrip_GraphControl.TabIndex = 1;
            // 
            // toolStripDropDownButton_LineThickness
            // 
            this.toolStripDropDownButton_LineThickness.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton_LineThickness.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineWidth1pxToolStripMenuItem,
            this.lineWidth2pxToolStripMenuItem1,
            this.lineWidth3pxToolStripMenuItem2,
            this.lineWidth4pxToolStripMenuItem3,
            this.lineWidth5pxToolStripMenuItem4});
            this.toolStripDropDownButton_LineThickness.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_LineThickness.Image")));
            this.toolStripDropDownButton_LineThickness.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_LineThickness.Name = "toolStripDropDownButton_LineThickness";
            this.toolStripDropDownButton_LineThickness.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton_LineThickness.Text = "Line Thickness";
            // 
            // lineWidth1pxToolStripMenuItem
            // 
            this.lineWidth1pxToolStripMenuItem.Name = "lineWidth1pxToolStripMenuItem";
            this.lineWidth1pxToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.lineWidth1pxToolStripMenuItem.Tag = "1";
            this.lineWidth1pxToolStripMenuItem.Text = "1px";
            this.lineWidth1pxToolStripMenuItem.Click += new System.EventHandler(this.lineWidthToolStripMenuItem_Click);
            // 
            // lineWidth2pxToolStripMenuItem1
            // 
            this.lineWidth2pxToolStripMenuItem1.Name = "lineWidth2pxToolStripMenuItem1";
            this.lineWidth2pxToolStripMenuItem1.Size = new System.Drawing.Size(92, 22);
            this.lineWidth2pxToolStripMenuItem1.Tag = "2";
            this.lineWidth2pxToolStripMenuItem1.Text = "2px";
            this.lineWidth2pxToolStripMenuItem1.Click += new System.EventHandler(this.lineWidthToolStripMenuItem_Click);
            // 
            // lineWidth3pxToolStripMenuItem2
            // 
            this.lineWidth3pxToolStripMenuItem2.Name = "lineWidth3pxToolStripMenuItem2";
            this.lineWidth3pxToolStripMenuItem2.Size = new System.Drawing.Size(92, 22);
            this.lineWidth3pxToolStripMenuItem2.Tag = "3";
            this.lineWidth3pxToolStripMenuItem2.Text = "3px";
            this.lineWidth3pxToolStripMenuItem2.Click += new System.EventHandler(this.lineWidthToolStripMenuItem_Click);
            // 
            // lineWidth4pxToolStripMenuItem3
            // 
            this.lineWidth4pxToolStripMenuItem3.Name = "lineWidth4pxToolStripMenuItem3";
            this.lineWidth4pxToolStripMenuItem3.Size = new System.Drawing.Size(92, 22);
            this.lineWidth4pxToolStripMenuItem3.Tag = "4";
            this.lineWidth4pxToolStripMenuItem3.Text = "4px";
            this.lineWidth4pxToolStripMenuItem3.Click += new System.EventHandler(this.lineWidthToolStripMenuItem_Click);
            // 
            // lineWidth5pxToolStripMenuItem4
            // 
            this.lineWidth5pxToolStripMenuItem4.Name = "lineWidth5pxToolStripMenuItem4";
            this.lineWidth5pxToolStripMenuItem4.Size = new System.Drawing.Size(92, 22);
            this.lineWidth5pxToolStripMenuItem4.Tag = "5";
            this.lineWidth5pxToolStripMenuItem4.Text = "5px";
            this.lineWidth5pxToolStripMenuItem4.Click += new System.EventHandler(this.lineWidthToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton_GraphType
            // 
            this.toolStripDropDownButton_GraphType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton_GraphType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointsToolStripMenuItem,
            this.toolStripSeparator6,
            this.linesToolStripMenuItem,
            this.trianglesToolStripMenuItem,
            this.quadsToolStripMenuItem,
            this.toolStripSeparator7,
            this.connectedLinesToolStripMenuItem,
            this.histogramToolStripMenuItem});
            this.toolStripDropDownButton_GraphType.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_GraphType.Image")));
            this.toolStripDropDownButton_GraphType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_GraphType.Name = "toolStripDropDownButton_GraphType";
            this.toolStripDropDownButton_GraphType.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton_GraphType.Text = "Graph Display Type";
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.pointsToolStripMenuItem.Text = "Points";
            this.pointsToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(159, 6);
            // 
            // linesToolStripMenuItem
            // 
            this.linesToolStripMenuItem.Name = "linesToolStripMenuItem";
            this.linesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.linesToolStripMenuItem.Text = "Lines";
            this.linesToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // trianglesToolStripMenuItem
            // 
            this.trianglesToolStripMenuItem.Name = "trianglesToolStripMenuItem";
            this.trianglesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.trianglesToolStripMenuItem.Text = "Triangles";
            this.trianglesToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // quadsToolStripMenuItem
            // 
            this.quadsToolStripMenuItem.Name = "quadsToolStripMenuItem";
            this.quadsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.quadsToolStripMenuItem.Text = "Quads";
            this.quadsToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(159, 6);
            // 
            // connectedLinesToolStripMenuItem
            // 
            this.connectedLinesToolStripMenuItem.Name = "connectedLinesToolStripMenuItem";
            this.connectedLinesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.connectedLinesToolStripMenuItem.Text = "Connected Lines";
            this.connectedLinesToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // histogramToolStripMenuItem
            // 
            this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            this.histogramToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.histogramToolStripMenuItem.Text = "Histogram";
            this.histogramToolStripMenuItem.Click += new System.EventHandler(this.setGraphRenderingMethod);
            // 
            // toolStripDropDownButton_ShaderMode
            // 
            this.toolStripDropDownButton_ShaderMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton_ShaderMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidLightingToolStripMenuItem,
            this.distanceFogToolStripMenuItem,
            this.worldSpaceCoordsToolStripMenuItem,
            this.boundingCoordsToolStripMenuItem});
            this.toolStripDropDownButton_ShaderMode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_ShaderMode.Image")));
            this.toolStripDropDownButton_ShaderMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_ShaderMode.Name = "toolStripDropDownButton_ShaderMode";
            this.toolStripDropDownButton_ShaderMode.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton_ShaderMode.Text = "Graph Shader Mode";
            // 
            // solidLightingToolStripMenuItem
            // 
            this.solidLightingToolStripMenuItem.Name = "solidLightingToolStripMenuItem";
            this.solidLightingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.solidLightingToolStripMenuItem.Text = "Solid Lighting";
            this.solidLightingToolStripMenuItem.Click += new System.EventHandler(this.setGraphColorMode);
            // 
            // distanceFogToolStripMenuItem
            // 
            this.distanceFogToolStripMenuItem.Name = "distanceFogToolStripMenuItem";
            this.distanceFogToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.distanceFogToolStripMenuItem.Text = "Distance Fog";
            this.distanceFogToolStripMenuItem.Click += new System.EventHandler(this.setGraphColorMode);
            // 
            // worldSpaceCoordsToolStripMenuItem
            // 
            this.worldSpaceCoordsToolStripMenuItem.Name = "worldSpaceCoordsToolStripMenuItem";
            this.worldSpaceCoordsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.worldSpaceCoordsToolStripMenuItem.Text = "World Space Co-ords";
            this.worldSpaceCoordsToolStripMenuItem.Click += new System.EventHandler(this.setGraphColorMode);
            // 
            // boundingCoordsToolStripMenuItem
            // 
            this.boundingCoordsToolStripMenuItem.Name = "boundingCoordsToolStripMenuItem";
            this.boundingCoordsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.boundingCoordsToolStripMenuItem.Text = "Bounding Co-ords";
            this.boundingCoordsToolStripMenuItem.Click += new System.EventHandler(this.setGraphColorMode);
            // 
            // toolStripDropDownButton_RenderTransparency
            // 
            this.toolStripDropDownButton_RenderTransparency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton_RenderTransparency.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solidToolStripMenuItem,
            this.transparentToolStripMenuItem,
            this.stippleToolStripMenuItem});
            this.toolStripDropDownButton_RenderTransparency.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_RenderTransparency.Image")));
            this.toolStripDropDownButton_RenderTransparency.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_RenderTransparency.Name = "toolStripDropDownButton_RenderTransparency";
            this.toolStripDropDownButton_RenderTransparency.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton_RenderTransparency.Text = "Render Transparency";
            // 
            // solidToolStripMenuItem
            // 
            this.solidToolStripMenuItem.Name = "solidToolStripMenuItem";
            this.solidToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.solidToolStripMenuItem.Text = "Solid";
            this.solidToolStripMenuItem.Click += new System.EventHandler(this.setGraphTransparencyMode);
            // 
            // transparentToolStripMenuItem
            // 
            this.transparentToolStripMenuItem.Name = "transparentToolStripMenuItem";
            this.transparentToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.transparentToolStripMenuItem.Text = "Transparent";
            this.transparentToolStripMenuItem.Click += new System.EventHandler(this.setGraphTransparencyMode);
            // 
            // stippleToolStripMenuItem
            // 
            this.stippleToolStripMenuItem.Name = "stippleToolStripMenuItem";
            this.stippleToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.stippleToolStripMenuItem.Text = "Stipple";
            this.stippleToolStripMenuItem.Click += new System.EventHandler(this.setGraphTransparencyMode);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ShowAxes
            // 
            this.toolStripButton_ShowAxes.Checked = true;
            this.toolStripButton_ShowAxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_ShowAxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ShowAxes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ShowAxes.Image")));
            this.toolStripButton_ShowAxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ShowAxes.Name = "toolStripButton_ShowAxes";
            this.toolStripButton_ShowAxes.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ShowAxes.Text = "Show Axes";
            this.toolStripButton_ShowAxes.Click += new System.EventHandler(this.toolStripButton_ShowAxes_Click);
            // 
            // toolStripButton_ShowAxesInColor
            // 
            this.toolStripButton_ShowAxesInColor.Checked = true;
            this.toolStripButton_ShowAxesInColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_ShowAxesInColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ShowAxesInColor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ShowAxesInColor.Image")));
            this.toolStripButton_ShowAxesInColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ShowAxesInColor.Name = "toolStripButton_ShowAxesInColor";
            this.toolStripButton_ShowAxesInColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_ShowAxesInColor.Text = "Show axes in color";
            this.toolStripButton_ShowAxesInColor.Click += new System.EventHandler(this.toolStripButton_ShowAxesInColor_Click);
            // 
            // toolStrip_RenderCameraControl
            // 
            this.toolStrip_RenderCameraControl.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_RenderCameraControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_RenderMode2D,
            this.toolStripButton_RenderMode3DOrth,
            this.toolStripButton_RenderMode3DPersp,
            this.toolStripSeparator3,
            this.toolStripButton_ResetCameraFocus,
            this.toolStripButton_ResetCamera});
            this.toolStrip_RenderCameraControl.Location = new System.Drawing.Point(3, 25);
            this.toolStrip_RenderCameraControl.Name = "toolStrip_RenderCameraControl";
            this.toolStrip_RenderCameraControl.Size = new System.Drawing.Size(319, 25);
            this.toolStrip_RenderCameraControl.TabIndex = 0;
            // 
            // toolStripButton_RenderMode2D
            // 
            this.toolStripButton_RenderMode2D.Checked = true;
            this.toolStripButton_RenderMode2D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_RenderMode2D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_RenderMode2D.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RenderMode2D.Image")));
            this.toolStripButton_RenderMode2D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RenderMode2D.Name = "toolStripButton_RenderMode2D";
            this.toolStripButton_RenderMode2D.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_RenderMode2D.Text = "2D";
            this.toolStripButton_RenderMode2D.Click += new System.EventHandler(this.toolStripButton_RenderMode);
            // 
            // toolStripButton_RenderMode3DOrth
            // 
            this.toolStripButton_RenderMode3DOrth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_RenderMode3DOrth.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RenderMode3DOrth.Image")));
            this.toolStripButton_RenderMode3DOrth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RenderMode3DOrth.Name = "toolStripButton_RenderMode3DOrth";
            this.toolStripButton_RenderMode3DOrth.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_RenderMode3DOrth.Text = "3D (Orth)";
            this.toolStripButton_RenderMode3DOrth.Click += new System.EventHandler(this.toolStripButton_RenderMode);
            // 
            // toolStripButton_RenderMode3DPersp
            // 
            this.toolStripButton_RenderMode3DPersp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_RenderMode3DPersp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RenderMode3DPersp.Image")));
            this.toolStripButton_RenderMode3DPersp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RenderMode3DPersp.Name = "toolStripButton_RenderMode3DPersp";
            this.toolStripButton_RenderMode3DPersp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_RenderMode3DPersp.Text = "3D (Persp)";
            this.toolStripButton_RenderMode3DPersp.Click += new System.EventHandler(this.toolStripButton_RenderMode);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ResetCameraFocus
            // 
            this.toolStripButton_ResetCameraFocus.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ResetCameraFocus.Image")));
            this.toolStripButton_ResetCameraFocus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ResetCameraFocus.Name = "toolStripButton_ResetCameraFocus";
            this.toolStripButton_ResetCameraFocus.Size = new System.Drawing.Size(133, 22);
            this.toolStripButton_ResetCameraFocus.Text = "Reset Camera Focus";
            this.toolStripButton_ResetCameraFocus.Click += new System.EventHandler(this.toolStripButton_ResetCamera_Click);
            // 
            // toolStripButton_ResetCamera
            // 
            this.toolStripButton_ResetCamera.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ResetCamera.Image")));
            this.toolStripButton_ResetCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ResetCamera.Name = "toolStripButton_ResetCamera";
            this.toolStripButton_ResetCamera.Size = new System.Drawing.Size(99, 22);
            this.toolStripButton_ResetCamera.Text = "Reset Camera";
            this.toolStripButton_ResetCamera.Click += new System.EventHandler(this.toolStripButton_ResetCamera_Click);
            // 
            // CLRGraph_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "CLRGraph_MainForm";
            this.Text = "CLRGraph";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CLRGraph_MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.splitContainer_Right.Panel1.ResumeLayout(false);
            this.splitContainer_Right.Panel2.ResumeLayout(false);
            this.splitContainer_Right.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Right)).EndInit();
            this.splitContainer_Right.ResumeLayout(false);
            this.tabControl_sidebar.ResumeLayout(false);
            this.tabPage_GraphScript.ResumeLayout(false);
            this.tabPage_GraphScript.PerformLayout();
            this.toolStrip_GraphScript.ResumeLayout(false);
            this.toolStrip_GraphScript.PerformLayout();
            this.tabPage_Series.ResumeLayout(false);
            this.tabPage_DataSource.ResumeLayout(false);
            this.tabPage_DataSource.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStrip_GraphControl.ResumeLayout(false);
            this.toolStrip_GraphControl.PerformLayout();
            this.toolStrip_RenderCameraControl.ResumeLayout(false);
            this.toolStrip_RenderCameraControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.SplitContainer splitContainer_Right;
        private System.Windows.Forms.TabControl tabControl_sidebar;
        private System.Windows.Forms.TabPage tabPage_GraphScript;
        private System.Windows.Forms.TextBox textBox_RuntimeREPL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_ClearLog;
        private System.Windows.Forms.Button button_ExecuteRuntimeInput;
        private System.Windows.Forms.Splitter splitter_Log;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.TextBox textBox_GraphScript;
        private System.Windows.Forms.ToolStrip toolStrip_GraphScript;
        private System.Windows.Forms.ToolStripButton toolStripButton_NewGraphScript;
        private System.Windows.Forms.ToolStripButton toolStripButton_OpenGraphScript;
        private System.Windows.Forms.ToolStripButton toolStripButton_SaveGraphScript;
        private System.Windows.Forms.ToolStripButton toolStripButton_SaveAsGraphScript;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_CompileGraphScript;
        private GLGraph glGraph;
        private System.Windows.Forms.ToolStrip toolStrip_RenderCameraControl;
        private System.Windows.Forms.ToolStripButton toolStripButton_RenderMode2D;
        private System.Windows.Forms.ToolStripButton toolStripButton_RenderMode3DOrth;
        private System.Windows.Forms.ToolStripButton toolStripButton_RenderMode3DPersp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_ResetCameraFocus;
        private System.Windows.Forms.ToolStripButton toolStripButton_ResetCamera;
        private System.Windows.Forms.ToolStrip toolStrip_GraphControl;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_LineThickness;
        private System.Windows.Forms.ToolStripMenuItem lineWidth1pxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineWidth2pxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lineWidth3pxToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem lineWidth4pxToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem lineWidth5pxToolStripMenuItem4;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_GraphType;
        private System.Windows.Forms.ToolStripMenuItem connectedLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_ShowAxes;
        private System.Windows.Forms.ToolStripButton toolStripButton_ShowAxesInColor;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_ShaderMode;
        private System.Windows.Forms.ToolStripMenuItem solidLightingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem distanceFogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem worldSpaceCoordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boundingCoordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem linesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trianglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.TabPage tabPage_Series;
        private System.Windows.Forms.ColumnHeader columnHeader_ID;
        private System.Windows.Forms.ColumnHeader columnHeader_Color;
        private System.Windows.Forms.ColumnHeader columnHeader_Name;
        private System.Windows.Forms.ColumnHeader columnHeader_PointCount;
        public System.Windows.Forms.ListView listView_series;
        private System.Windows.Forms.ColumnHeader columnHeader_Visible;
        private System.Windows.Forms.ColumnHeader columnHeader_Current;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_RenderTransparency;
        private System.Windows.Forms.ToolStripMenuItem solidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stippleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel FPSCounterLabel;
        private System.Windows.Forms.TabPage tabPage_DataSource;
        private System.Windows.Forms.ColumnHeader columnHeader_DataSourceName;
        private System.Windows.Forms.ColumnHeader columnHeader_DataSourceType;
        private System.Windows.Forms.ColumnHeader columnHeader_DataSourceLocation;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_AddDataSource;
        public System.Windows.Forms.ListView listView_DataSources;
        private System.Windows.Forms.ToolStripButton toolStripButton_RemoveDataSource;
    }
}

