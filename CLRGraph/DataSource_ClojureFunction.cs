using clojure.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public enum DataSource_ClojureFunction_Mode
    {
        Mode_2D_Plot,
        Mode_3D_Surface,
    }

    public partial class DataSource_ClojureFunction_Config : Form
    {
        DataSource_ClojureFunction owner = null;

        public double MinX = -1, MaxX = 1, MinY = -1, MaxY = 1, Precision = 0.1;

        public string ClojureFunction1 = "";
        public string ClojureFunction2 = "";

        public DataSource_ClojureFunction_Config(DataSource_ClojureFunction nOwner, double minX, double maxX, double minY, double maxY, double precision, string clojureFunction1, string clojureFunction2)
        {
            InitializeComponent();

            owner = nOwner;

            numericUpDown_minX.Value = (decimal)(MinX = minX);
            numericUpDown_maxX.Value = (decimal)(MaxX = maxX);
            MinY = minY;
            MaxY = maxY;
            numericUpDown_precision.Value = (decimal)(Precision = precision);

            textBox_plot1.Text = ClojureFunction1 = clojureFunction1;
            textBox_plot2.Text = ClojureFunction2 = clojureFunction2;
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            MinX = (double)numericUpDown_minX.Value;
            MaxX = (double)numericUpDown_maxX.Value;
            Precision = (double)numericUpDown_precision.Value;

            ClojureFunction1 = textBox_plot1.Text;
            ClojureFunction2 = textBox_plot2.Text;

            owner.UpdateSettings(this);
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }

    [DataSourceAttribute("Clojure Function", "Function")]
    public class DataSource_ClojureFunction : DataSource
    {
        IFn clojureFunction1 = null;
        IFn clojureFunction2 = null;
        string clojureFunctionText1 = "(CMath/Sin (+ x t))";
        string clojureFunctionText2 = "";

        double minXVal = -1;
        double maxXVal = 1;
        double minYVal = -1;
        double maxYVal = 1;
        double precision = 0.1;
        DataSource_ClojureFunction_Mode drawMode = DataSource_ClojureFunction_Mode.Mode_2D_Plot;

        public override bool ShowDataSourceSelector()
        {
            Timer t = new Timer();
            t.Interval = 5;
            t.Tick += (s, e) => { t.Stop(); t.Dispose(); t = null; ShowDataSeriesConfig(); };
            t.Start();
            return true;
        }

        public override void ShowDataSeriesConfig()
        {
            DataSource_ClojureFunction_Config config = new DataSource_ClojureFunction_Config(this, minXVal, maxXVal, minYVal, maxYVal, precision, clojureFunctionText1, clojureFunctionText2);
            config.Show();
        }

        public void UpdateSettings(DataSource_ClojureFunction_Config sender)
        {
            minXVal = sender.MinX;
            maxXVal = sender.MaxX;
            minYVal = sender.MinY;
            maxYVal = sender.MaxY;
            precision = sender.Precision;

            clojureFunctionText1 = sender.ClojureFunction1.Trim();
            clojureFunctionText2 = sender.ClojureFunction2.Trim();

            clojureFunction1 = null;
            clojureFunction2 = null;

            if (clojureFunctionText1 != "")
            {
                try { clojureFunction1 = (IFn)ClojureEngine.EvalRaw("(fn [x y z t] " + clojureFunctionText1 + ")"); }
                catch { }
            }

            if (clojureFunctionText2 != "")
            {
                try { clojureFunction2 = (IFn)ClojureEngine.EvalRaw("(fn [x y z t] " + clojureFunctionText2 + ")"); }
                catch { }
            }
        }

        public override List<GraphPoint> GetData(int channel, double elapsedTime)
        {
            if (clojureFunction1 == null && clojureFunction2 == null)
                return new List<GraphPoint>();

            List<GraphPoint> points = Graph_Funcs.MakePlot3DFN(minXVal, maxXVal, precision, clojureFunction1, clojureFunction2, elapsedTime);

            return points;
        }
    }
}
