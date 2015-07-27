using clojure.lang;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    [DataSourceAttribute("Random Number", "Random")]
    public class DataSource_Random : DataSource
    {
        Random random = new Random();

        double minX = -10, maxX = 10, minY = -10, maxY = 10, minZ = -10, maxZ = 10;
        //int pointCountPerTick = 1000;
        int pointCountPerTick = 1;

        public DataSource_Random(string name)
            : base(name)
        {

        }

        public override PersistentVector GetData(int channel = 0)
        {
            GraphPoint[] points = new GraphPoint[pointCountPerTick];

            for (int i = 0; i < pointCountPerTick; i++)
            {
                points[i] = new GraphPoint(  (random.NextDouble() * (maxX - minX) + minX),
                                            (random.NextDouble() * (maxY - minY) + minY),
                                            (random.NextDouble() * (maxZ - minZ) + minZ));
            }

            return PersistentVector.create1(points);
        }
    }

    [DataSourceAttribute("Sine Wave", "Over Time")]
    public class DataSource_SineWave : DataSource
    {
        double newTime = 0;
        double sineIncrement = 0.1;
        int pointCount = 1000;

        public DataSource_SineWave(string name)
            : base(name)
        {

        }

        public override PersistentVector GetData(int channel = 0)
        {
            GraphPoint[] points = new GraphPoint[pointCount];

            Parallel.For(0, pointCount, (i) =>
                {
                    points[i] = new GraphPoint(i * sineIncrement, Math.Sin(i * sineIncrement + newTime), 0);
                });

            newTime += sineIncrement;
            return PersistentVector.create1(points);
        }
    }


    [DataSourceAttribute("CSV File", "File")]
    public class DataSource_CSVFile : DataSource
    {
        Dictionary<string, List<object>> CSVTable = new Dictionary<string, List<object>>();
        int rows = 0;

        string xaxis = null;
        string yaxis = null;
        string zaxis = null;

        public DataSource_CSVFile(string name)
            : base(name)
        {

        }

        public override PersistentVector GetData(int channel = 0)
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

        public override bool ShowDataSourceSelector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            ofd.Multiselect = false;
            ofd.Title = "Select CSV File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            SourceLocation = ofd.FileName;

            char delimiter = ',';

            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                string[] headers = sr.ReadLine().Split(delimiter);

                for (int i = 0; i < headers.Length; i++)
                    CSVTable.Add(headers[i], new List<object>());

                string[] line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(delimiter);

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

            return true;
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

    [DataSourceAttribute("PLY File", "File")]
    public class DataSource_PLYFile : DataSource
    {
        PersistentVector points = null;
        bool needNewData = false;

        public DataSource_PLYFile(string name)
            : base(name)
        {

        }

        public override void GraphReset()
        {
            needNewData = true;
        }

        public override bool NeedToGetNewData(int channel = 0)
        {
            bool ret = needNewData;
            needNewData = false;
            return ret;
        }

        public override PersistentVector GetData(int channel = 0)
        {
            return points;
        }

        public override bool ShowDataSourceSelector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PLY Files (*.ply)|*.ply";
            ofd.Multiselect = false;
            ofd.Title = "Select PLY File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            SourceLocation = ofd.FileName;

            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                string line = sr.ReadLine();
                string[] splLine;

                if (line != "ply")
                {
                    MessageBox.Show("Error: File is not a valid PLY file. Aborting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                List<Tuple<string, int>> elementGroups = new List<Tuple<string,int>>();
                while ((line = sr.ReadLine()) != "end_header")
                {
                    splLine = line.Trim().Split(' ');
                    if (splLine[0] == "format" && splLine[1] != "ascii")
                    {
                        MessageBox.Show("Sorry, but the PLY importer currently only supports ASCII format files", "Unsupported Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if (splLine[0] == "element" && splLine.Length >= 3)
                    {
                        elementGroups.Add(new Tuple<string, int>(splLine[1], int.Parse(splLine[2])));
                    }
                }

                List<GraphPoint> newPoints = new List<GraphPoint>();

                foreach (Tuple<string, int> elementGroup in elementGroups)
                {
                    if (elementGroup.Item1 == "vertex")
                    {
                        for (int i = 0; i < elementGroup.Item2; i++)
                        {
                            splLine = sr.ReadLine().Trim().Split(' ');
                            newPoints.Add(new GraphPoint(double.Parse(splLine[0]), double.Parse(splLine[1]), double.Parse(splLine[2])));
                        }
                    }
                    else if (elementGroup.Item1 == "face")
                    {
                        for (int i = 0; i < elementGroup.Item2; i++)
                        {
                            splLine = sr.ReadLine().Trim().Split(' ');
                            int count = int.Parse(splLine[0]);
                            int firstPoint = int.Parse(splLine[1]);
                            int lastPoint = firstPoint;
                            int curPoint;

                            for (int j = 1; j < count; j++)
                            {
                                curPoint = int.Parse(splLine[j + 1]);
                                newPoints[lastPoint].edges.Add(newPoints[curPoint]);
                                lastPoint = curPoint;
                            }

                            newPoints[lastPoint].edges.Add(newPoints[firstPoint]);
                        }
                    }
                }

                points = PersistentVector.create1(newPoints);
                needNewData = true;
            }

            return true;
        }
    }
}
