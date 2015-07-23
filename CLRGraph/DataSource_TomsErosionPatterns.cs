using clojure.lang;
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
    public partial class DataSource_TomsErosionPatterns_Config : Form
    {
        public bool bDisplayWaterInsteadOfLand = false;

        public DataSource_TomsErosionPatterns_Config(bool displayWater)
        {
            InitializeComponent();

            checkBox_displayWater.Checked = bDisplayWaterInsteadOfLand = displayWater;
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            bDisplayWaterInsteadOfLand = checkBox_displayWater.Checked;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }

    [DataSourceAttribute("Tom's Erosion Pattern", "File")]
    public class DataSource_TomsErosionPatterns : DataSource
    {
        bool bShowWaterInsteadOfLand = false;
        List<GraphPoint> landPoints = new List<GraphPoint>();
        List<GraphPoint> waterPoints = new List<GraphPoint>();

        public DataSource_TomsErosionPatterns(string name)
            : base(name)
        {

        }

        public override PersistentVector GetData(int channel = 0)
        {
            bool actualChannel = channel == 1 ? !bShowWaterInsteadOfLand : bShowWaterInsteadOfLand;
            return PersistentVector.create1(actualChannel ? waterPoints : landPoints);
        }

        public override void ShowDataSeriesConfig()
        {
            DataSource_TomsErosionPatterns_Config cfg = new DataSource_TomsErosionPatterns_Config(bShowWaterInsteadOfLand);

            if (cfg.ShowDialog() != DialogResult.OK)
                return;

            bShowWaterInsteadOfLand = cfg.bDisplayWaterInsteadOfLand;
        }

        public override bool ShowDataSourceSelector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Erosion Files (*.erosion)|*.erosion";
            ofd.Multiselect = false;
            ofd.Title = "Select Erosion File File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            SourceLocation = ofd.FileName;

            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                string line = sr.ReadLine();
                string[] splLine;

                if (line != "E1")
                {
                    MessageBox.Show("File is not a correct erosion type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                line = sr.ReadLine();

                line = sr.ReadLine();
                splLine = line.Split(' ');

                int xCount = int.Parse(splLine[0]);
                int yCount = int.Parse(splLine[1]);
                int zCount = int.Parse(splLine[2]);

                line = sr.ReadLine();

                for (int z = 0; z < zCount; z++)
                {
                    for (int y = 0; y < yCount; y++)
                    {
                        for (int x = 0; x < xCount; x++)
                        {
                            if (line[x + y * xCount + z * yCount * xCount] == '1')
                                landPoints.Add(new GraphPoint(x, y, z));
                            else
                                waterPoints.Add(new GraphPoint(x, y, z));
                        }
                    }
                }
            }

            return true;
        }
    }
}
