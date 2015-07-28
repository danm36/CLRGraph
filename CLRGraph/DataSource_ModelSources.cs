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
    #region PLY
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

                points = PersistentVector.create1(newPoints);
                needNewData = true;
            }

            return true;
        }
    }
    #endregion
}
