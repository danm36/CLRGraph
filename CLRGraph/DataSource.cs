using clojure.lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public abstract class DataSource : IDisposable
    {
        public static Dictionary<string, DataSource> DataSources = new Dictionary<string, DataSource>();
        public static void UpdateDataSourceInfoInUI()
        {
            List<ListViewItem> items = new List<ListViewItem>();

            foreach(KeyValuePair<string, DataSource> kvp in DataSources)
            {
                ListViewItem lvi = new ListViewItem(kvp.Key);
                lvi.Tag = kvp.Value;

                lvi.SubItems.Add(kvp.Value.GetType().Name);
                lvi.SubItems.Add(kvp.Value.SourceLocation);

                items.Add(lvi);
            }

            int topItemIndex = CLRGraph_MainForm.self.listView_DataSources.TopItem == null ? 0 : CLRGraph_MainForm.self.listView_DataSources.TopItem.Index;
            CLRGraph_MainForm.self.listView_DataSources.Items.Clear();
            CLRGraph_MainForm.self.listView_DataSources.Items.AddRange(items.ToArray());

            if (CLRGraph_MainForm.self.listView_DataSources.Items.Count > 0)
                CLRGraph_MainForm.self.listView_DataSources.TopItem = CLRGraph_MainForm.self.listView_DataSources.Items[topItemIndex];
        }

        private string _SourceName = "";
        public string SourceName
        {
            get
            {
                return _SourceName;
            }

            private set
            {
                DataSources.Remove(_SourceName);
                DataSources.Add(value, this);
                _SourceName = value;
            }
        }
        public string SourceLocation { get; private set; }
        protected bool IsDisposed = false;

        public DataSource(string name, string location)
        {
            if (name == null)
            {
                int index = DataSources.Count;
                do
                {
                    name = "DataSource_" + ++index;
                }
                while (DataSources.ContainsKey(name));
            }
            _SourceName = name;

            if(location == null)
                location = "Unknwon Source";
            SourceLocation = location;

            DataSources.Add(_SourceName, this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
        }

        public abstract PersistentVector GetData();
        public virtual void ShowDataSeriesConfig()
        {
            MessageBox.Show("This data source does not have any configuration properties", "Config", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
    
    public class CSVDataSource : DataSource
    {
        Dictionary<string, List<object>> CSVTable = new Dictionary<string, List<object>>();
        int rows = 0;

        string xaxis = null;
        string yaxis = null;
        string zaxis = null;

        public CSVDataSource(string name, string file, char delimmiter = ',')
            : base(name, file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string[] headers = sr.ReadLine().Split(delimmiter);

                for (int i = 0; i < headers.Length; i++)
                    CSVTable.Add(headers[i], new List<object>());

                string[] line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(delimmiter);

                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (i >= line.Length)
                            CSVTable[headers[i]].Add("");
                        else
                            CSVTable[headers[i]].Add(line[i]);
                    }

                    ++rows;
                }
            }
        }

        public override PersistentVector GetData()
        {
            List<GraphPoint> points = new List<GraphPoint>();

            int errorCount = 0;
            for (int i = 0; i < rows; i++)
            {
                double x = i;
                double y = 0;
                double z = 0;

                if (xaxis != null && !double.TryParse(CSVTable[xaxis][i].ToString(), out x))
                {
                    errorCount++;
                    continue;
                }

                if (yaxis != null && !double.TryParse(CSVTable[yaxis][i].ToString(), out y))
                {
                    errorCount++;
                    ClojureEngine.Log("[CSVDataSource] Could not read double value from y-axis '" + xaxis + "' on row '" + i + "' in data source '" + SourceName + "'. Skipping point.");
                    continue;
                }

                if (zaxis != null && !double.TryParse(CSVTable[zaxis][i].ToString(), out z))
                {
                    errorCount++;
                    ClojureEngine.Log("[CSVDataSource] Could not read double value from z-axis '" + xaxis + "' on row '" + i + "' in data source '" + SourceName + "'. Skipping point.");
                    continue;
                }

                if (errorCount > 0)
                    ClojureEngine.Log("[CSVDataSource] Note: " + errorCount + " rows could not be read from data source '" + SourceName + "' due to the data on one of the rows not being numeric.");

                points.Add(new GraphPoint(x, y, z));
            }

            return PersistentVector.create1(points);
        }

        public override void ShowDataSeriesConfig()
        {
            AxisDefiner axisDefiner = new AxisDefiner(CSVTable.Keys.ToList(), xaxis, yaxis, zaxis);

            if (axisDefiner.ShowDialog() != DialogResult.OK)
                return;

            xaxis = axisDefiner.xAxisColumn;
            yaxis = axisDefiner.yAxisColumn;
            zaxis = axisDefiner.zAxisColumn;
        }
    }
}
