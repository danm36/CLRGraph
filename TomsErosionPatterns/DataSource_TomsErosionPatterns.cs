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
    [DataSourceAttribute("Tom's Erosion Pattern", "File")]
    public class DataSource_TomsErosionPatterns : DataSource
    {
        const int pointGroupCount = 10;

        List<GraphPoint>[] points = new List<GraphPoint>[pointGroupCount];
        bool[] needToGetPoints = new bool[pointGroupCount];

        public DataSource_TomsErosionPatterns()
        {
            for (int i = 0; i < points.Length; i++)
                points[i] = new List<GraphPoint>();
        }

        public override void GraphReset()
        {
            for (int i = 0; i < pointGroupCount; i++)
                needToGetPoints[i] = true;
        }

        public override bool NeedToGetNewData(int channel)
        {
            return needToGetPoints[channel];
        }

        public override List<GraphPoint> GetData(int channel, double elapsedTime)
        {
            channel = Math.Min(points.Length - 1, Math.Max(channel, 0));
            needToGetPoints[channel] = false;
            return points[channel];
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
                            char chr = line[x + y * xCount + z * yCount * xCount];
                            int channel = 0;
                            if (char.IsDigit(chr))
                                channel = (int)char.GetNumericValue(line[x + y * xCount + z * yCount * xCount]);

                            points[channel].Add(new GraphPoint(x, y, z));
                        }
                    }
                }
            }

            for (int i = 0; i < pointGroupCount; i++)
                needToGetPoints[i] = true;

            return true;
        }
    }
}
