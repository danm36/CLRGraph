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
    #region OBJ
    [DataSourceAttribute("OBJ File (Wavefront OBJ)", "File")]
    public class DataSource_OBJFile : DataSource
    {
        List<GraphPoint> points = null;
        bool needNewData = false;

        public override void GraphReset()
        {
            needNewData = true;
        }

        public override bool NeedToGetNewData(int channel)
        {
            bool ret = needNewData;
            needNewData = false;
            return ret;
        }

        public override List<GraphPoint> GetData(int channel, double elapsedTime)
        {
            return points;
        }

        public override bool ShowDataSourceSelector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wavefront OBJ Files (*.obj)|*.obj";
            ofd.Multiselect = false;
            ofd.Title = "Select OBJ File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            SourceLocation = ofd.FileName;

            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                string line;
                string[] splLine;
                List<GraphPoint> newPoints = new List<GraphPoint>();

                double offsetX = 0, offsetY = 0, offsetZ = 0;
                double scaleX = 1, scaleY = 1, scaleZ = 1;

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.Length == 0 || (line[0] == '#' && !line.StartsWith("#Auto")))
                        continue;

                    splLine = line.Split(' ');

                    if (splLine[0] == "#Auto") //Handle ZBrush automatic transforms
                    {
                        if (splLine[1] == "scale")
                        {
                            scaleX = double.Parse(splLine[2].Substring(2));
                            scaleY = double.Parse(splLine[3].Substring(2));
                            scaleZ = double.Parse(splLine[4].Substring(2));
                        }
                        else if (splLine[1] == "offset")
                        {
                            offsetX = double.Parse(splLine[2].Substring(2));
                            offsetY = double.Parse(splLine[3].Substring(2));
                            offsetZ = double.Parse(splLine[4].Substring(2));
                        }
                    }
                    else if (line[0] == 'v')
                    {
                        //Note: Right handed co-ords
                        newPoints.Add(new GraphPoint(   double.Parse(splLine[1]) * scaleX + offsetX,
                                                        double.Parse(splLine[3]) * scaleZ + offsetZ,
                                                        double.Parse(splLine[2]) * scaleY + offsetY));
                    }
                    else if (line[0] == 'f')
                    {
                        int firstPoint = int.Parse(splLine[1]);
                        int lastPoint = firstPoint;
                        int curPoint;

                        for (int j = 2; j < splLine.Length; j++)
                        {
                            curPoint = int.Parse(splLine[j]);
                            newPoints[lastPoint - 1].AddEdge(newPoints[curPoint - 1]);
                            newPoints[curPoint - 1].AddEdge(newPoints[lastPoint - 1]);
                            lastPoint = curPoint;
                        }

                        newPoints[lastPoint - 1].AddEdge(newPoints[firstPoint - 1]);
                    }
                }

                points = newPoints;
                needNewData = true;
            }

            return true;
        }
    }
    #endregion

    #region PLY
    [DataSourceAttribute("PLY File", "File")]
    public class DataSource_PLYFile : DataSource
    {
        List<GraphPoint> points = null;
        bool needNewData = false;

        public override void GraphReset()
        {
            needNewData = true;
        }

        public override bool NeedToGetNewData(int channel)
        {
            bool ret = needNewData;
            needNewData = false;
            return ret;
        }

        public override List<GraphPoint> GetData(int channel, double elapsedTime)
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

                List<Tuple<string, int>> elementGroups = new List<Tuple<string, int>>();
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
                    else if (elementGroup.Item1 == "face") //TODO: Somehow fix normals and winding or whatever
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
                                newPoints[lastPoint].AddEdge(newPoints[curPoint]);
                                newPoints[curPoint].AddEdge(newPoints[lastPoint]);
                                lastPoint = curPoint;
                            }

                            newPoints[lastPoint].AddEdge(newPoints[firstPoint]);
                        }
                    }
                }

                points = newPoints;
                needNewData = true;
            }

            return true;
        }
    }
    #endregion

    #region PTS
    [DataSourceAttribute("PTS File", "File")]
    public class DataSource_PTSFile : DataSource
    {
        List<GraphPoint> points = null;
        bool needNewData = false;

        public override void GraphReset()
        {
            needNewData = true;
        }

        public override bool NeedToGetNewData(int channel)
        {
            bool ret = needNewData;
            needNewData = false;
            return ret;
        }

        public override List<GraphPoint> GetData(int channel, double elapsedTime)
        {
            return points;
        }

        public override bool ShowDataSourceSelector()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PTS Files (*.pts)|*.pts";
            ofd.Multiselect = false;
            ofd.Title = "Select PTS File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            SourceLocation = ofd.FileName;

            using (StreamReader sr = new StreamReader(ofd.FileName))
            {
                string line;
                string[] splLine;
                List<GraphPoint> newPoints = new List<GraphPoint>();

                line = sr.ReadLine(); //Number of points

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    splLine = line.Split(' ');

                    //Right handed coordinate system
                    newPoints.Add(new GraphPoint(double.Parse(splLine[0]),
                                                    double.Parse(splLine[2]),
                                                    double.Parse(splLine[1])));
                }

                points = newPoints;
                needNewData = true;
            }

            return true;
        }
    }
    #endregion
}
