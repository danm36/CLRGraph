using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public partial class DataSource_Selector : Form
    {
        public Type SourceType = null;
        public string SourceName = null;

        static Dictionary<Type, DataSourceAttribute> sources = null;

        public DataSource_Selector()
        {
            InitializeComponent();

            if (sources == null)
            {
                sources = new Dictionary<Type, DataSourceAttribute>();

                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Type[] types = (from type in assemblies[i].GetTypes()
                                    where Attribute.IsDefined(type, typeof(DataSourceAttribute))
                                    select type).ToArray();

                    for (int j = 0; j < types.Length; j++)
                    {
                        sources.Add(types[j], (DataSourceAttribute)types[j].GetCustomAttribute(typeof(DataSourceAttribute)));
                    }
                }
            }

            foreach (KeyValuePair<Type, DataSourceAttribute> source in sources)
            {
                ListViewItem lvi = new ListViewItem(source.Value.Name);
                lvi.Tag = source.Key;
                lvi.SubItems.Add(source.Value.Category);

                listView_DataSourceTypes.Items.Add(lvi);
            }

            string initialName = "";
            int index = DataSource.DataSources.Count;
            do
            {
                initialName = "DataSource_" + ++index;
            }
            while (DataSource.DataSources.ContainsKey(initialName));
            textBox_SourceName.Text = initialName;
        }

        private void button_AddSource_Click(object sender, EventArgs e)
        {
            if (listView_DataSourceTypes.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please selecte a data source type first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SourceName = textBox_SourceName.Text;
            SourceType = (Type)listView_DataSourceTypes.SelectedItems[0].Tag;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            SourceType = null;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
