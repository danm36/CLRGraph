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

        public override PersistentVector GetData()
        {
            return PersistentVector.create1(bShowWaterInsteadOfLand ? waterPoints : landPoints);
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

                bool bIs3D = MessageBox.Show("Create points to render 3D cubes (Memory hog)", "Cubes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

                line = sr.ReadLine();

                line = sr.ReadLine();
                splLine = line.Split(' ');

                int xCount = int.Parse(splLine[0]);
                int yCount = int.Parse(splLine[1]);
                int zCount = int.Parse(splLine[2]);

                line = sr.ReadLine();

                GraphPoint p1, p2, p3, p4, p5, p6, p7, p8;

                for (int z = 0; z < zCount; z++)
                {
                    for (int y = 0; y < yCount; y++)
                    {
                        for (int x = 0; x < xCount; x++)
                        {
                            List<GraphPoint> newPoints = new List<GraphPoint>();


                            if (bIs3D)
                            {
                                newPoints.Add(p1 = new GraphPoint(x - 0.5, y - 0.5, z - 0.5));
                                newPoints.Add(p2 = new GraphPoint(x + 0.5, y - 0.5, z - 0.5));
                                newPoints.Add(p3 = new GraphPoint(x - 0.5, y + 0.5, z - 0.5));
                                newPoints.Add(p4 = new GraphPoint(x - 0.5, y - 0.5, z + 0.5));
                                newPoints.Add(p5 = new GraphPoint(x + 0.5, y + 0.5, z - 0.5));
                                newPoints.Add(p6 = new GraphPoint(x + 0.5, y - 0.5, z + 0.5));
                                newPoints.Add(p7 = new GraphPoint(x - 0.5, y + 0.5, z + 0.5));
                                newPoints.Add(p8 = new GraphPoint(x + 0.5, y + 0.5, z + 0.5));

                                p1.AddEdges(p2, p3, p4);
                                p8.AddEdges(p7, p6, p5);

                                p2.AddEdges(p1, p5, p6);
                                p7.AddEdges(p8, p4, p3);

                                p3.AddEdges(p1, p5, p7);
                                p6.AddEdges(p8, p4, p2);

                                p4.AddEdges(p1, p6, p7);
                                p5.AddEdges(p8, p3, p2);
                            }
                            else
                            {
                                newPoints.Add(new GraphPoint(x, y, z));
                            }

                            if (line[x + y * xCount + z * yCount * xCount] == '1')
                                landPoints.AddRange(newPoints);
                            else
                                waterPoints.AddRange(newPoints);
                        }
                    }
                }
            }

            return true;
        }
    }
}
