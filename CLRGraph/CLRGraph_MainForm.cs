using clojure.lang;
using CLRGraph.Properties;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    enum DataGridState
    {
        NotLoaded,
        Loading,
        NeedCommit,
        Committing,
        Committed,
    }

    public partial class CLRGraph_MainForm : Form
    {
        public static CLRGraph_MainForm self;

        #region Graph Data Variables
        DataGridState dataGridState = DataGridState.NotLoaded;

        string xAxisColumn, yAxisColumn, zAxisColumn;
        #endregion

        #region GraphScript Variables
        const string GraphScript_DefaultScript = "(set-data-points (range 0 10))";
        string GraphScript_LoadedFile = null;
        string GraphScript_LastDirectory = Path.GetFullPath("examples/graphscripts");
        #endregion

        #region Log Variables
        List<string> commandHistory = new List<string>();
        int currentCommandIndex = -1;
        bool isNavigatingCommandHistory = false;
        #endregion

        public CLRGraph_MainForm()
        {
            self = this;

            InitializeComponent();

            ClojureEngine.Initialize(textBox_Log);

            GraphData_SetState(DataGridState.NotLoaded);
            //textBox_GraphScript.Text = File.ReadAllText("examples/graphscripts/trig_tube.clj");
            textBox_GraphScript.Text = File.ReadAllText("examples/graphscripts/test.clj");
        }

        #region Form Events
        private void CLRGraph_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        #endregion

        #region Graph Data
        private void GraphData_Open()
        {
            //TODO: Filetype selector

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV File (*.csv)|*.csv|All Files (*)|*";
            ofd.Title = "Open Data File";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            GraphData_SetState(DataGridState.Loading);

            //Temp CSV parsing
            string[] filedata = File.ReadAllText(ofd.FileName).Split('\r', '\n');

            bool skipFirst = false;
            if(MessageBox.Show("Does the first row contains column names?", "CSV File", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                skipFirst = true;
                string[] splline = filedata[0].Split(',');

                for (int i = 0; i < splline.Length; i++)
                    dataGridView.Columns.Add(splline[i], splline[i]);
            }

            foreach (string line in filedata)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }

                if (line.Trim() == "")
                    continue;

                string[] splline = line.Split(',');

                while (splline.Length > dataGridView.Columns.Count)
                    dataGridView.Columns.Add("col" + (dataGridView.Columns.Count + 1), "Col " + (dataGridView.Columns.Count + 1));

                dataGridView.Rows.Add(splline);

                Application.DoEvents();
            }

            GraphData_SetState(DataGridState.NeedCommit);
            GraphData_ShowAxisDefiner();
        }

        private void GraphData_ShowAxisDefiner()
        {
            List<string> columns = new List<string>();

            for (int i = 0; i < dataGridView.Columns.Count; i++)
                columns.Add(dataGridView.Columns[i].HeaderText);

            AxisDefiner ad = new AxisDefiner(columns);

            if (ad.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            xAxisColumn = ad.xAxisColumn;
            yAxisColumn = ad.yAxisColumn;
            zAxisColumn = ad.zAxisColumn;

            GraphData_SetState(DataGridState.NeedCommit);
        }

        private void GraphData_Save()
        {
            MessageBox.Show("Graph Data saving not yet implemented!", "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void GraphData_Commit()
        {

            //TEMP
            /*List<GraphPoint> graphPoints = new List<GraphPoint>();

            for (float i = -10; i < 10; i += 0.1f)
            {
                graphPoints.Add(new GraphPoint(i, Math.Sin(i), Math.Cos(i)));
            }

            glGraph.SetDataPoints(graphPoints);*/

            if (xAxisColumn == null && yAxisColumn == null && zAxisColumn == null)
            {
                MessageBox.Show("You need to define which columns are going to be used in the graph!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GraphData_SetState(DataGridState.Committing);

            List<GraphPoint> graphPoints = new List<GraphPoint>();

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                double nX = i;
                double nY = 0;
                double nZ = 0;

                if (xAxisColumn != null && dataGridView.Rows[i].Cells[xAxisColumn].Value != null)
                    double.TryParse(dataGridView.Rows[i].Cells[xAxisColumn].Value.ToString(), out nX);

                if (yAxisColumn != null && dataGridView.Rows[i].Cells[yAxisColumn].Value != null)
                    double.TryParse(dataGridView.Rows[i].Cells[yAxisColumn].Value.ToString(), out nY);

                if (zAxisColumn != null && dataGridView.Rows[i].Cells[zAxisColumn].Value != null)
                    double.TryParse(dataGridView.Rows[i].Cells[zAxisColumn].Value.ToString(), out nZ);

                graphPoints.Add(new GraphPoint(nX, nY, nZ));
            }

            GraphFuncs.SetDataPoints(graphPoints);

            GraphData_SetState(DataGridState.Committed);
        }

        private void GraphData_SetState(DataGridState newState)
        {
            switch (newState)
            {
                case DataGridState.NotLoaded:
                    toolStripLabel_DataState.Text = "Not Loaded";
                    toolStripButton_CommitData.Enabled = true;
                    break;

                case DataGridState.Loading:
                    toolStripLabel_DataState.Text = "Loading...";
                    toolStripButton_CommitData.Enabled = false;
                    break;

                case DataGridState.NeedCommit:
                    toolStripLabel_DataState.Text = "Not Committed";
                    toolStripButton_CommitData.Enabled = true;
                    break;

                case DataGridState.Committing:
                    toolStripLabel_DataState.Text = "Committing...";
                    toolStripButton_CommitData.Enabled = false;
                    break;

                case DataGridState.Committed:
                    toolStripLabel_DataState.Text = "Committed";
                    toolStripButton_CommitData.Enabled = false;
                    break;
            }

            dataGridState = newState;
        }

        private void toolStripButton_OpenData_Click(object sender, EventArgs e)
        {
            GraphData_Open();
        }

        private void toolStripButton_SaveData_Click(object sender, EventArgs e)
        {
            GraphData_Save();
        }

        private void toolStripButton_CommitData_Click(object sender, EventArgs e)
        {
            GraphData_Commit();
        }

        private void toolStripButton_DefineAxes_Click(object sender, EventArgs e)
        {
            GraphData_ShowAxisDefiner();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            GraphData_SetState(DataGridState.NeedCommit);
        }

        private void dataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            GraphData_SetState(DataGridState.NeedCommit);
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            GraphData_SetState(DataGridState.NeedCommit);
        }


        #endregion

        #region Graph Script
        private bool GraphScript_WillCauseOverwrite()
        {
            return textBox_GraphScript.Text.Trim() != "";
        }

        private void GraphScript_New()
        {
            if (GraphScript_WillCauseOverwrite() && MessageBox.Show("Are you sure you want to create a new GraphScript?\n\nUnsaved changes will be lost!", "New GraphScript", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                return;

            textBox_GraphScript.Text = GraphScript_DefaultScript;
            GraphScript_LoadedFile = null;
        }

        private void GraphScript_Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Clojure File (*.clj)|*.clj|Any File (*)|*";
            ofd.Title = "Open Clojure File";
            ofd.InitialDirectory = GraphScript_LastDirectory;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            if (GraphScript_WillCauseOverwrite())
            {
                DialogResult dr = MessageBox.Show("Do you want to save the current GraphScript file?", "Save GraphScript", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                if (dr == System.Windows.Forms.DialogResult.Cancel)
                    return;

                if (dr == System.Windows.Forms.DialogResult.Yes)
                    GraphScript_Save();
            }

            //FIXME: Check for binary files

            textBox_GraphScript.Text = File.ReadAllText(ofd.FileName);

            GraphScript_LoadedFile = ofd.FileName;
            GraphScript_LastDirectory = Path.GetDirectoryName(ofd.FileName) + Path.DirectorySeparatorChar;
        }

        private void GraphScript_Save()
        {
            if (GraphScript_LoadedFile == null)
            {
                GraphScript_SaveAs();
                return;
            }

            File.WriteAllText(GraphScript_LoadedFile, textBox_GraphScript.Text);
        }

        private void GraphScript_SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Clojure File (*.clj)|*.clj|Any File (*)|*";
            sfd.Title = "Save Clojure File";
            sfd.InitialDirectory = GraphScript_LastDirectory;
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            GraphScript_LoadedFile = sfd.FileName;
            GraphScript_Save();

            GraphScript_LastDirectory = Path.GetDirectoryName(sfd.FileName) + Path.DirectorySeparatorChar;
        }

        private void toolStripButton_NewGraphScript_Click(object sender, EventArgs e) { GraphScript_New(); }
        private void toolStripButton_OpenGraphScript_Click(object sender, EventArgs e) { GraphScript_Open(); }
        private void toolStripButton_SaveGraphScript_Click(object sender, EventArgs e) { GraphScript_Save(); }
        private void toolStripButton_SaveAsGraphScript_Click(object sender, EventArgs e) { GraphScript_SaveAs(); }
        private void toolStripButton_CompileGraphScript_Click(object sender, EventArgs e) { ClojureEngine.Eval("(reset-graph)\n" + textBox_GraphScript.Text, true, true); }
        #endregion

        #region Runtime Log
        private void button_ExecuteRuntimeInput_Click(object sender, EventArgs e)
        {
            if (ClojureEngine.Eval(textBox_RuntimeREPL.Text))
            {
                commandHistory.Add(textBox_RuntimeREPL.Text);
                currentCommandIndex = -1;
                textBox_RuntimeREPL.Text = "";
            }
        }

        private void textBox_RuntimeREPL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == 0)
            {
                e.SuppressKeyPress = true;
                button_ExecuteRuntimeInput_Click(sender, null);
                isNavigatingCommandHistory = false;
            }
            else if (e.KeyCode == Keys.Up && (isNavigatingCommandHistory || textBox_RuntimeREPL.TextLength == 0))
            {
                isNavigatingCommandHistory = true;

                --currentCommandIndex;

                if (currentCommandIndex < 0)
                    currentCommandIndex = commandHistory.Count - 1;

                textBox_RuntimeREPL.Text = commandHistory[currentCommandIndex];
                textBox_RuntimeREPL.SelectionStart = textBox_RuntimeREPL.TextLength - 1;
            }
            else if (e.KeyCode == Keys.Down && (isNavigatingCommandHistory || textBox_RuntimeREPL.TextLength == 0))
            {
                isNavigatingCommandHistory = true;

                ++currentCommandIndex;

                if (currentCommandIndex >= commandHistory.Count)
                    currentCommandIndex = 0;

                textBox_RuntimeREPL.Text = commandHistory[currentCommandIndex];
                textBox_RuntimeREPL.SelectionStart = textBox_RuntimeREPL.TextLength - 1;
            }
            else
            {
                isNavigatingCommandHistory = false;
                currentCommandIndex = -1;
            }
        }

        private void button_ClearLog_Click(object sender, EventArgs e)
        {
            ClojureEngine.ClearLog();
        }
        #endregion

        #region OpenGL Specific
        private void toolStripButton_RenderMode(object sender, EventArgs e)
        {
            ProjectionType renderMode = ProjectionType.RM_2D;

            if (sender == toolStripButton_RenderMode3DOrth)
                renderMode = ProjectionType.RM_3D_Orth;
            else if (sender == toolStripButton_RenderMode3DPersp)
                renderMode = ProjectionType.RM_3D_Persp;

            toolStripButton_RenderMode2D.Checked =
                toolStripButton_RenderMode3DOrth.Checked =
                toolStripButton_RenderMode3DPersp.Checked = false;

            ((ToolStripButton)sender).Checked = true;

            glGraph.SetProjectionType(renderMode);
        }

        private void toolStripButton_ResetCamera_Click(object sender, EventArgs e)
        {
            glGraph.ResetCamera(sender == toolStripButton_ResetCameraFocus);
        }

        private void lineWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                DataSeriesFuncs.SetSeriesLineWidthAll(float.Parse(((ToolStripMenuItem)sender).Tag.ToString()));
            }
        }

        private void setGraphRenderingMethod(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if (sender == pointsToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.Points);
                else if (sender == linesToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.Lines);
                else if (sender == trianglesToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.Triangles);
                else if (sender == quadsToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.Quads);
                else if (sender == connectedLinesToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.ConnectedLines);
                else if (sender == histogramToolStripMenuItem)
                    DataSeriesFuncs.SetSeriesDrawModeAll(DrawMode.Histogram);
            }
        }

        private void setGraphColorMode(object sender, EventArgs e)
        {
            if (sender == solidLightingToolStripMenuItem)
                DataSeriesFuncs.SetSeriesColorModeAll(ColorMode.Solid);
            else if (sender == distanceFogToolStripMenuItem)
                DataSeriesFuncs.SetSeriesColorModeAll(ColorMode.DistanceFog);
            else if (sender == worldSpaceCoordsToolStripMenuItem)
                DataSeriesFuncs.SetSeriesColorModeAll(ColorMode.WorldCoords);
            else if (sender == boundingCoordsToolStripMenuItem)
                DataSeriesFuncs.SetSeriesColorModeAll(ColorMode.BoundsCoords);
        }
        
        private void setGraphTransparencyMode(object sender, EventArgs e)
        {
            if (sender == solidToolStripMenuItem)
                DataSeriesFuncs.SetSeriesTransparencyModeAll(TransparencyMode.Solid);
            else if(sender == transparentToolStripMenuItem)
                DataSeriesFuncs.SetSeriesTransparencyModeAll(TransparencyMode.Transparent);
        }


        private void toolStripButton_ShowAxes_Click(object sender, EventArgs e)
        {
            toolStripButton_ShowAxes.Checked = !toolStripButton_ShowAxes.Checked;
            glGraph.SetAxisDisplay(toolStripButton_ShowAxes.Checked);
        }

        private void toolStripButton_ShowAxesInColor_Click(object sender, EventArgs e)
        {
            toolStripButton_ShowAxesInColor.Checked = !toolStripButton_ShowAxesInColor.Checked;
            glGraph.SetAxisColor(toolStripButton_ShowAxesInColor.Checked);
        }
        #endregion        

        #region Series Properties
        private void tabControl_sidebar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_sidebar.SelectedTab == tabPage_Series)
                DataSeriesFuncs.UpdateSeriesInfoInUI();
        }

        public bool listview_series_disable_ItemChecked = false;
        private void listView_series_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (listview_series_disable_ItemChecked || e.Item == null || e.Item.Tag == null || !(e.Item.Tag is DataSeries))
                return;

            ((DataSeries)e.Item.Tag).SetHidden(!e.Item.Checked);
        }

        private void listView_series_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_series.SelectedItems.Count < 1 || listView_series.SelectedItems[0] == null || !(listView_series.SelectedItems[0].Tag is DataSeries))
                return;

            for(int i = 0; i < listView_series.Items.Count; i++)
                listView_series.Items[i].SubItems[1].Text = "";

            DataSeriesFuncs.SetCurrentSeries((DataSeries)listView_series.SelectedItems[0].Tag);
            listView_series.SelectedItems[0].SubItems[1].Text = "✓";
        }
        #endregion
    }
}
