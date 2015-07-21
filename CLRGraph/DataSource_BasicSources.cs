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

        public override PersistentVector GetData()
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

        public override PersistentVector GetData()
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
}
