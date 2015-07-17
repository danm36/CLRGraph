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

            textBox_GraphScript.Text = File.ReadAllText("examples/graphscripts/init.clj");
        }

        #region Form Events
        private void CLRGraph_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

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
                DataSeries_Funcs.SetSeriesLineWidthAll(float.Parse(((ToolStripMenuItem)sender).Tag.ToString()));
                GLGraph.Redraw(true);
            }
        }

        private void setGraphRenderingMethod(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if (sender == pointsToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.Points);
                else if (sender == linesToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.Lines);
                else if (sender == trianglesToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.Triangles);
                else if (sender == quadsToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.Quads);
                else if (sender == connectedLinesToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.ConnectedLines);
                else if (sender == histogramToolStripMenuItem)
                    DataSeries_Funcs.SetSeriesDrawModeAll(DrawMode.Histogram);

                GLGraph.Redraw(true);
            }
        }

        private void setGraphColorMode(object sender, EventArgs e)
        {
            if (sender == solidLightingToolStripMenuItem)
                DataSeries_Funcs.SetSeriesColorModeAll(ColorMode.Solid);
            else if (sender == distanceFogToolStripMenuItem)
                DataSeries_Funcs.SetSeriesColorModeAll(ColorMode.DistanceFog);
            else if (sender == worldSpaceCoordsToolStripMenuItem)
                DataSeries_Funcs.SetSeriesColorModeAll(ColorMode.WorldCoords);
            else if (sender == boundingCoordsToolStripMenuItem)
                DataSeries_Funcs.SetSeriesColorModeAll(ColorMode.BoundsCoords);

            GLGraph.Redraw(true);
        }
        
        private void setGraphTransparencyMode(object sender, EventArgs e)
        {
            if (sender == solidToolStripMenuItem)
                DataSeries_Funcs.SetSeriesTransparencyModeAll(TransparencyMode.Solid);
            else if(sender == transparentToolStripMenuItem)
                DataSeries_Funcs.SetSeriesTransparencyModeAll(TransparencyMode.Transparent);

            GLGraph.Redraw(true);
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
                DataSeries_Funcs.UpdateSeriesInfoInUI();
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

            DataSeries_Funcs.SetCurrentSeries((DataSeries)listView_series.SelectedItems[0].Tag);
            listView_series.SelectedItems[0].SubItems[1].Text = "✓";
        }
        #endregion

        #region Data Source Properties
        private void toolStripButton_AddDataSource_Click(object sender, EventArgs e)
        {
            //Todo: Proper sources dialog
            DataSource_Selector selector = new DataSource_Selector();

            if (selector.ShowDialog() != System.Windows.Forms.DialogResult.OK || selector.SourceType == null)
                return;

            DataSource source = (DataSource)Activator.CreateInstance(selector.SourceType, selector.SourceName);

            if (!source.ShowDataSourceSelector())
                return;

            //Show name select

            DataSource.DataSources.Add(source.SourceName, source);
            DataSource.UpdateDataSourceInfoInUI();
        }

        private void toolStripButton_RemoveDataSource_Click(object sender, EventArgs e)
        {
            if (listView_DataSources.SelectedItems.Count < 1)
            {
                MessageBox.Show("No data source is selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove this data source reference?", "Remove Data Source", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No)
                return;

            DataSource toDelete = ((DataSource)listView_DataSources.SelectedItems[0].Tag);
            DataSource.DataSources.Remove(toDelete.SourceName);
            toDelete.Dispose();

            DataSource.UpdateDataSourceInfoInUI();
        }

        private void listView_DataSources_DoubleClick(object sender, EventArgs e)
        {
            if (listView_DataSources.SelectedItems.Count < 1)
                return;

            ((DataSource)listView_DataSources.SelectedItems[0].Tag).ShowDataSeriesConfig();
        }
        #endregion

    }
}
