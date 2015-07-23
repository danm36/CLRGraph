using clojure.lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Type sourceType = kvp.Value.GetType();
                DataSourceAttribute attr = (DataSourceAttribute)sourceType.GetCustomAttribute(typeof(DataSourceAttribute));

                ListViewItem lvi = new ListViewItem(kvp.Key);
                lvi.Tag = kvp.Value;

                lvi.SubItems.Add(attr.Name);
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

            protected set
            {
                DataSources.Remove(_SourceName);
                DataSources.Add(value, this);
                _SourceName = value;
                UpdateDataSourceInfoInUI();
            }
        }
        private string _SourceLocation = "";
        public string SourceLocation
        {
            get
            {
                return _SourceLocation;
            }
            protected set
            {
                _SourceLocation = value;
                UpdateDataSourceInfoInUI();
            }
        }
        protected bool IsDisposed = false;

        public DataSource(string sourceName)
        {
            if (sourceName == null || sourceName.Trim() == "")
            {
                int index = DataSources.Count;
                do
                {
                    sourceName = "DataSource_" + ++index;
                }
                while (DataSources.ContainsKey(sourceName));
            }
            _SourceName = sourceName;
            SourceLocation = "Unknown Source";

            //DataSources.Add(_SourceName, this);
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

        public virtual bool NeedToGetNewData(int channel = 0) { return true; }
        public abstract PersistentVector GetData(int channel = 0);

        public virtual bool ShowDataSourceSelector()
        {
            ShowDataSeriesConfig();
            return true;
        }

        public virtual void ShowDataSeriesConfig()
        {
            MessageBox.Show("This data source does not have any configuration properties.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DataSourceAttribute : Attribute
    {
        public string Name;
        public string Category;

        public DataSourceAttribute(string name)
            : this(name, "Undefined")
        {
        }

        public DataSourceAttribute(string name, string category)
        {
            Name = name;
            Category = category;
        }
    }
}
