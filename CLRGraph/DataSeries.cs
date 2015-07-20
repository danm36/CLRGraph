using clojure.lang;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    [ClojureImport]
    public enum DrawMode : int
    {
        Points =  0,
        Lines,
        Triangles,
        Quads,
        ConnectedLines,
        Histogram,
    }

    [ClojureImport]
    public enum ColorMode : int
    {
        Solid = 0,
        DistanceFog,
        WorldCoords,
        BoundsCoords,
    }

    [ClojureImport]
    public enum TransparencyMode : int
    {
        Solid = 0,
        Transparent,
    }

    [ClojureClass]
    public class DataSeries : IDisposable
    {
        readonly Color[] defaultColors = new Color[]
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Orange,
            Color.Purple,
            Color.Yellow,
            Color.Pink,
            Color.Brown,
        };
        static int currentDefaultColor = 0;
        static DrawMode lastDrawMode = DrawMode.Points;
        static ColorMode lastColorMode = ColorMode.Solid;
        static TransparencyMode lastTransparencyMode = TransparencyMode.Solid;
        static float lastLineWidth = 3.0f;

        #region Static DataSeries Funcs
        public static List<DataSeries> AllDataSeries = new List<DataSeries>();
        public static int CurrentDataSeries = 0;

        public static void ResetAll()
        {
            for (int i = 0; i < AllDataSeries.Count; i++)
            {
                AllDataSeries[i].Reset();
                AllDataSeries[i].Dispose();
            }

            currentDefaultColor = 0;

            AllDataSeries.Clear();
            new DataSeries();
            CurrentDataSeries = 0;

            DataSeries_Funcs.UpdateSeriesInfoInUI();
        }
        #endregion


        public string Name { get; private set; }
        public Color DrawColor { get; private set; }
        public Shader DrawShader { get; private set; }
        public PersistentVector DataPoints { get; private set; }
        public DrawMode DrawMode { get; private set; }
        public ColorMode ColorMode { get; private set; }
        public TransparencyMode TransparencyMode { get; private set; }
        public bool Hidden { get; private set; }
        public float LineWidth { get; private set; }

        private DataSource dataSource = null;
        private Timer dataSourcePollTimer = null;
        private const int dataSourcePollTimerMin = 10;
        private int lastPollInterval = 1000;

        private uint[] indices = null;

        private Box CachedBounds = null;

        private bool disposed = false;
        private int VertexVBO = -1, IndexVBO = -1;

        public DataSeries() : this(null, null, null, null)
        {
        }

        public DataSeries(PersistentVector points) : this(points, null, null, null)
        {
        }

        public DataSeries(PersistentVector points, Shader shader) : this(points, shader, null, null)
        {
        }

        public DataSeries(PersistentVector points, string name) : this(points, null, name, null)
        {
        }

        public DataSeries(PersistentVector points, Shader shader, string name) : this(points, shader, name, null)
        {
        }

        public DataSeries(PersistentVector points, string name, Color? color) : this(points, null, name, color)
        {
        }

        public DataSeries(string name, Color? color) : this(null, null, name, color)
        {
        }

        public DataSeries(PersistentVector points, Shader shader, string name, Color? color)
        {
            if (points == null)
                points = PersistentVector.EMPTY;

            List<GraphPoint> idCorrection = new List<GraphPoint>();
            int count = 0;
            foreach (object obj in points)
            {
                GraphPoint newPoint = (GraphPoint)obj;
                newPoint.index = count++;
                idCorrection.Add(newPoint);
            }

            DataPoints = PersistentVector.create1(idCorrection);

            if (shader == null)
            {
                shader = new Shader("shaders/v_graphplot.vert", "shaders/f_graphplot.frag");
            }
            DrawShader = shader;

            Name = name;
            if (Name == null)
            {
                Name = "Series1";
                int currentIndex = 1;

                for (int i = 0; i < AllDataSeries.Count; i++)
                {
                    if (AllDataSeries[i].Name == Name)
                    {
                        i = -1;
                        Name = "Series" + (++currentIndex);
                    }
                }
            }

            if (color == null)
            {
                DrawColor = defaultColors[currentDefaultColor];
                currentDefaultColor = (currentDefaultColor + 1) % defaultColors.Length;
            }
            else
            {
                DrawColor = (Color)color;
            }


            VertexVBO = GL.GenBuffer();
            IndexVBO = GL.GenBuffer();

            AllDataSeries.Add(this);

            DrawMode = lastDrawMode;
            ColorMode = lastColorMode;
            TransparencyMode = lastTransparencyMode;
            LineWidth = lastLineWidth;
            Hidden = false;

            UpdateVertexVBO();
            UpdateIndexVBO();
            DataSeries_Funcs.UpdateSeriesInfoInUI();

            GLGraph.self.UpdateMatrices(true);
        }

        public DataSeries SetName(string newName)
        {
            newName = newName.Trim();

            if (newName.Length == 0)
                return this;

            Name = newName;

            DataSeries_Funcs.UpdateSeriesInfoInUI();

            return this;
        }

        public DataSeries SetColor(Color newColor)
        {
            newColor = Color.FromArgb(255, newColor.R, newColor.G, newColor.B);
            DrawColor = newColor;

            DataSeries_Funcs.UpdateSeriesInfoInUI();

            return this;
        }

        public DataSeries SetHidden(bool newHidden)
        {
            Hidden = newHidden;

            DataSeries_Funcs.UpdateSeriesInfoInUI();

            return this;
        }

        public void ChangeShader(Shader newShader)
        {
            DrawShader.Dispose();
            DrawShader = newShader;
        }

        public DataSeries Reset()
        {
            StopDataSourcePoll();
            ClearDataPoints();

            return this;
        }

        public DataSeries ClearDataPoints()
        {
            DataPoints = PersistentVector.EMPTY;
            CachedBounds = null;

            DataSeries_Funcs.UpdateSeriesInfoInUI();

            return this;
        }

        public PersistentVector GetDataPoints()
        {
            return DataPoints;
        }


        public int SetDataPoints(List<GraphPoint> newPoints)
        {
            ClearDataPoints();
            return AddDataPoints(newPoints);
        }

        public int SetDataPoints(PersistentVector newPoints)
        {
            ClearDataPoints();
            return AddDataPoints(newPoints);
        }

        public int AddDataPoints(List<GraphPoint> newPoints)
        {
            if (newPoints == null || newPoints.Count == 0)
                return 0;

            List<object> dpList = DataPoints.ToList();
            int curCount = dpList.Count;

            int newPointsOldPos = int.MaxValue;
            for (int i = 0; i < newPoints.Count; i++)
                newPointsOldPos = Math.Min(newPointsOldPos, newPoints[i].index);
            if (newPointsOldPos < 0)
                newPointsOldPos = curCount;

            int diff = curCount - newPointsOldPos;

            for (int i = 0; i < newPoints.Count; i++)
            {
                for (int j = 0; j < newPoints[i].pendingEdges.Count; j++)
                {
                    //FIXME: HANDLE OFFSETS
                    //newPoints[i].pendingEdges[j] += diff; //Offset edge indices
                }
            }
            dpList.AddRange(newPoints);

            DataPoints = PersistentVector.create(dpList.ToArray());// (PersistentVector)DataPoints.asTransient().conj(newPoints.ToArray()).persistent();

            for (int i = 0; i < DataPoints.Count; i++)
                ((GraphPoint)DataPoints[i]).index = i;

            UpdateVertexVBO();
            DataSeries_Funcs.UpdateSeriesInfoInUI();

            CachedBounds = null;
            return newPoints.Count;
        }

        public int AddDataPoints(PersistentVector newPoints)
        {
            GraphPoint[] points = new GraphPoint[newPoints.count()];

            for (int i = 0; i < points.Length; i++)
                points[i] = (GraphPoint)newPoints[i];

            return AddDataPoints(points.ToList());
        }

        public void ValidateDatapointEdges()
        {
            GraphPoint curPoint;

            for (int i = 0; i < DataPoints.Count; i++)
            {
                curPoint = (GraphPoint)DataPoints[i];

                foreach(int pendingEdge in curPoint.pendingEdges)
                {
                    if (pendingEdge < 0 || pendingEdge >= DataPoints.Count)
                        continue;

                    curPoint.AddEdge((GraphPoint)DataPoints[pendingEdge]);
                }

                curPoint.pendingEdges.Clear();
            }
        }

        public DataSeries SetDataSource(DataSource newSource)
        {
            StopDataSourcePoll();
            dataSource = newSource;
            UpdatePointsFromDataSource();
            return this;
        }

        public DataSeries SetDataSource(DataSource newSource, double pollInterval)
        {
            bool timerWasRunning = dataSourcePollTimer == null ? false : dataSourcePollTimer.Enabled;

            StopDataSourcePoll();
            dataSource = newSource;
            lastPollInterval = Math.Max((int)Math.Round(pollInterval * 1000), dataSourcePollTimerMin);
            UpdatePointsFromDataSource();

            if (timerWasRunning)
                StartDataSourcePoll();

            return this;
        }

        public DataSeries StartDataSourcePoll()
        {
            StopDataSourcePoll();
            dataSourcePollTimer = new Timer();
            dataSourcePollTimer.Interval = lastPollInterval;
            dataSourcePollTimer.Tick += (s, e) =>
            {
                UpdatePointsFromDataSource();
            };
            dataSourcePollTimer.Start();
            return this;
        }

        public DataSeries StartDataSourcePoll(double newInterval)
        {
            lastPollInterval = Math.Max((int)Math.Round(newInterval * 1000), dataSourcePollTimerMin);
            return StartDataSourcePoll();
        }

        public DataSeries StopDataSourcePoll()
        {
            if (dataSourcePollTimer != null)
            {
                dataSourcePollTimer.Stop();
                dataSourcePollTimer.Enabled = false;
                dataSourcePollTimer = null;
            }

            return this;
        }

        public void UpdatePointsFromDataSource()
        {
            if (dataSource == null)
            {
                if (dataSourcePollTimer != null)
                    StopDataSourcePoll();

                return;
            }

            SetDataPoints(dataSource.GetData());
        }

        private void UpdateVertexVBO()
        {
            float[] points = new float[DataPoints.Count * 3];
            GraphPoint curPoint;
            for (int i = 0; i < DataPoints.Count; i++)
            {
                curPoint = (GraphPoint)DataPoints[i];

                points[i * 3 + 0] = curPoint.pos.X;
                points[i * 3 + 1] = curPoint.pos.Y;
                points[i * 3 + 2] = curPoint.pos.Z;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * points.Length), points, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            UpdateIndexVBO();
        }

        public void UpdateIndexVBO()
        {
            if (Hidden || DrawMode == CLRGraph.DrawMode.Points)
                return;

            ValidateDatapointEdges();

            List<uint> indexList = new List<uint>();
            GraphPoint curPoint;

            int edgeCount = 0;
            for (int i = 0; i < DataPoints.Count; i++)
            {
                curPoint = (GraphPoint)DataPoints[i];

                edgeCount = curPoint.edges.Count;

                foreach(GraphPoint edge in curPoint.edges)
                {
                    if (edge == curPoint)
                        continue;

                    if (DrawMode == CLRGraph.DrawMode.Lines)
                    {
                        indexList.Add((uint)i);
                        indexList.Add((uint)edge.index);
                    }
                    else if (DrawMode == CLRGraph.DrawMode.Triangles)
                    {
                        indexList.AddRange(curPoint.FormTrianglesWith(edge));
                    }
                    else if (DrawMode == CLRGraph.DrawMode.Quads)
                    {
                        indexList.AddRange(curPoint.FormQuadsWith(edge));
                    }
                }
            }

            indices = indexList.ToArray();

            if (indices.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexVBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
        }

        public void SetLineWidth(float newWidth)
        {
            lastLineWidth = LineWidth = newWidth;
        }

        public void SetDrawMode(DrawMode mode)
        {
            lastDrawMode = DrawMode = mode;

            UpdateIndexVBO();
        }

        public void SetColorMode(ColorMode mode)
        {
            lastColorMode = ColorMode = mode;
        }

        public void SetTransparencyMode(TransparencyMode mode)
        {
            lastTransparencyMode = TransparencyMode = mode;
        }

        public void Draw(bool bSimpleRedraw = false)
        {
            if (Hidden || DataPoints.Count == 0)
                return;

            if (DrawMode == DrawMode.Histogram)
            {

            }
            else
            {
                GL.UseProgram(DrawShader.shaderProgramHandle);

                GL.LineWidth(LineWidth);
                GL.PointSize(LineWidth);

                GL.Uniform4(DrawShader.GetUniformLocation("uSeriesColor"), DrawColor);
                GL.Uniform1(DrawShader.GetUniformLocation("uSeriesColorScale"), 1.0f);
                GL.Uniform1(DrawShader.GetUniformLocation("uColorMode"), (int)ColorMode);

                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexVBO);
                GL.EnableVertexAttribArray(DrawShader.vertexAttribLocation);
                GL.VertexAttribPointer(DrawShader.vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

                PrimitiveType primMode = PrimitiveType.Points;
                PrimitiveType backupPrimMode = PrimitiveType.Points;
                bool bNeedsIndices = false;

                switch (DrawMode)
                {
                    default:
                    case CLRGraph.DrawMode.Points:
                        break;

                    case CLRGraph.DrawMode.Lines:
                        primMode = PrimitiveType.Lines;
                        backupPrimMode = PrimitiveType.LineStrip;
                        bNeedsIndices = true;
                        break;

                    case CLRGraph.DrawMode.Triangles:
                        primMode = PrimitiveType.Triangles;
                        bNeedsIndices = true;
                        break;

                    case CLRGraph.DrawMode.Quads:
                        primMode = PrimitiveType.Quads;
                        bNeedsIndices = true;
                        break;

                    case CLRGraph.DrawMode.ConnectedLines:
                        primMode = PrimitiveType.LineStrip;
                        backupPrimMode = PrimitiveType.LineStrip;
                        break;
                }

                if (bNeedsIndices && indices != null && indices.Length > 0)
                {
                    if (bSimpleRedraw)
                        primMode = PrimitiveType.Lines;

                    //Polys
                    if (TransparencyMode == CLRGraph.TransparencyMode.Transparent && primMode != PrimitiveType.Lines)
                        GL.Enable(EnableCap.Blend);
                    GL.Enable(EnableCap.PolygonOffsetFill);
                    GL.PolygonOffset(1.0f, 1.0f);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexVBO);
                    GL.DrawElements(primMode, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                    GL.Disable(EnableCap.PolygonOffsetFill);
                    GL.Disable(EnableCap.Blend);

                    //Edges
                    if (primMode != PrimitiveType.Lines) //No point drawing again
                    {
                        if (TransparencyMode == CLRGraph.TransparencyMode.Transparent)
                            GL.Disable(EnableCap.DepthTest);
                        GL.Uniform1(DrawShader.GetUniformLocation("uSeriesColorScale"), 0.6f);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.DrawElements(primMode, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        GL.Enable(EnableCap.DepthTest);
                        //GL.Disable(EnableCap.CullFace);
                    }

                    //Vertices
                    GL.DrawArrays(PrimitiveType.Points, 0, DataPoints.Count);
                }
                else
                {
                    GL.DrawArrays(bSimpleRedraw ? PrimitiveType.Points : backupPrimMode, 0, DataPoints.Count);
                }
                
                GL.DisableVertexAttribArray(DrawShader.vertexAttribLocation);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DrawShader.Dispose();
            }

            GL.DeleteBuffer(VertexVBO);
            AllDataSeries.Remove(this);
            disposed = true;
        }


        public int GetPointCount()
        {
            return DataPoints.Count;
        }

        public bool IsEmpty()
        {
            return DataPoints.Count == 0;
        }

        public Box GetDataExtents()
        {
            if (CachedBounds != null)
                return CachedBounds;

            Box bb = new Box(float.MaxValue, float.MinValue, float.MaxValue, float.MinValue, float.MaxValue, float.MinValue);
            GraphPoint curPoint;

            for (int i = 0; i < DataPoints.Count; i++)
            {
                curPoint = (GraphPoint)DataPoints[i];

                bb.minX = Math.Min(curPoint.pos.X, bb.minX);
                bb.minY = Math.Min(curPoint.pos.Y, bb.minY);
                bb.minZ = Math.Min(curPoint.pos.Z, bb.minZ);

                bb.maxX = Math.Max(curPoint.pos.X, bb.maxX);
                bb.maxY = Math.Max(curPoint.pos.Y, bb.maxY);
                bb.maxZ = Math.Max(curPoint.pos.Z, bb.maxZ);
            }

            CachedBounds = bb;
            return bb;
        }

        public GraphPoint[] GetEdgemostPoints(bool returnClones = false)
        {
            if(DataPoints.Count == 0)
                return null;

            GraphPoint[] ret = new GraphPoint[] { (GraphPoint)DataPoints[0], (GraphPoint)DataPoints[0], (GraphPoint)DataPoints[0], (GraphPoint)DataPoints[0], (GraphPoint)DataPoints[0], (GraphPoint)DataPoints[0] };
            GraphPoint curPoint = null;

            for (int i = 0; i < DataPoints.Count; i++)
            {
                curPoint = (GraphPoint)DataPoints[i];

                if (curPoint.pos.X < ret[0].pos.X)
                    ret[0] = curPoint;
                if (curPoint.pos.X > ret[1].pos.X)
                    ret[1] = curPoint;

                if (curPoint.pos.Y < ret[2].pos.Y)
                    ret[2] = curPoint;
                if (curPoint.pos.Y > ret[3].pos.Y)
                    ret[3] = curPoint;

                if (curPoint.pos.Z < ret[4].pos.Z)
                    ret[4] = curPoint;
                if (curPoint.pos.Z > ret[5].pos.Z)
                    ret[5] = curPoint;

            }

            if (returnClones)
            {
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = new GraphPoint(ret[i]);
            }
            
            return ret;
        }
    }

    public class GraphPoint
    {
        public int index = -1;
        public Vector3 pos = new Vector3();
        public float x { get { return pos.X; } set { pos.X = value; } }
        public float y { get { return pos.Y; } set { pos.Y = value; } }
        public float z { get { return pos.Z; } set { pos.Z = value; } }

        public HashSet<int> pendingEdges = new HashSet<int>();
        public HashSet<GraphPoint> edges = new HashSet<GraphPoint>();

        public GraphPoint(Vector3 nPos)
        {
            index = DataSeries_Funcs.GetCurrentDataSeries().GetPointCount();

            pos = nPos;
        }

        public GraphPoint(double nX, double nY, double nZ)
            : this( new Vector3((float)nX, (float)nY, (float)nZ))
        {
        }

        public GraphPoint(Vector3 nPos, IList<int> nEdges)
            : this(nPos)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public GraphPoint(double nX, double nY, double nZ, IList<int> nEdges)
            : this(nX, nY, nZ)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public GraphPoint(Vector3 nPos, IList<GraphPoint> nEdges)
            : this(nPos)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                edges.Add(nEdges[i]);
            }
        }

        public GraphPoint(double nX, double nY, double nZ, IList<GraphPoint> nEdges)
            : this(nX, nY, nZ)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                edges.Add(nEdges[i]);
            }
        }

        public GraphPoint(GraphPoint other)
        {
            if (other == null)
                return;

            index = DataSeries_Funcs.GetCurrentDataSeries().GetPointCount();

            pos = other.pos;

            foreach (int edge in other.pendingEdges)
                pendingEdges.Add(edge);
            foreach (GraphPoint edge in other.edges)
                edges.Add(edge);
        }

        public void AddEdge(int edge)
        {
            pendingEdges.Add(edge);
        }

        public void AddEdge(GraphPoint edge)
        {
            edges.Add(edge);
        }

        public void AddEdges(IList<int> nEdges)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public void AddEdges(IList<GraphPoint> nEdges)
        {
            for (int i = 0; i < nEdges.Count; i++)
            {
                edges.Add(nEdges[i]);
            }
        }

        public void AddEdges(params int[] nEdges)
        {
            for (int i = 0; i < nEdges.Length; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public void AddEdges(params GraphPoint[] nEdges)
        {
            for (int i = 0; i < nEdges.Length; i++)
            {
                edges.Add(nEdges[i]);
            }
        }

        public double DistanceFrom(GraphPoint other)
        {
            return (pos - other.pos).Length;
        }

        public double DistanceFromSq(GraphPoint other)
        {
            return (pos - other.pos).LengthSquared;
        }

        public double DistanceFromLineSegSq(GraphPoint segA, GraphPoint segB)
        {
            Vector3 ta = (segA.pos - this.pos);
            Vector3 tb = (segB.pos - this.pos);
            Vector3 ab = (segB.pos - segA.pos);

            float e = Vector3.Dot(tb, ta);

            if (e < 0)
                return Vector3.Dot(tb, tb);

            float f = Vector3.Dot(ta, ta);

            if (e >= f)
                return Vector3.Dot(ab, ab);

            return Vector3.Dot(tb, tb) - e * e / f;
        }


        public List<uint> FormTrianglesWith(GraphPoint other, bool bForQuadRendering = false)
        {
            List<GraphPoint> intersection = edges.Intersect(other.edges).ToList();
            List<uint> results = new List<uint>();

            for (int i = 0; i < intersection.Count; i++)
            {
                if (intersection[i] == this || intersection[i] == other)
                    continue;

                if (bForQuadRendering)
                    results.Add((uint)index);
                results.Add((uint)index);
                results.Add((uint)other.index);
                results.Add((uint)intersection[i].index);
            }

            return results;
        }

        public List<uint> FormQuadsWith(GraphPoint other)
        {
            List<uint> results = new List<uint>();

            foreach (GraphPoint edge1 in edges)
            {
                foreach (GraphPoint edge2 in edges)
                {
                    if (edge1 == edge2)
                        continue;

                    List<GraphPoint> intersection = edge1.edges.Intersect(edge2.edges).ToList();

                    if (intersection.Count > 0)
                    {
                        for (int i = 0; i < intersection.Count; i++)
                        {
                            if (intersection[i] == this || intersection[i] == edge1 || intersection[i] == edge2)
                                continue;

                            results.Add((uint)index);
                            results.Add((uint)edge1.index);
                            results.Add((uint)intersection[i].index);
                            results.Add((uint)edge2.index);
                        }
                    }
                    else if (edge1.edges.Contains(edge2)) //Add a tri instead if possible
                    {
                        results.Add((uint)index);
                        results.Add((uint)index);
                        results.Add((uint)edge1.index);
                        results.Add((uint)edge2.index);
                    }
                }
            }

            return results;
        }

        public override string ToString()
        {
            return "{" + pos.X + ", " + pos.Y + ", " + pos.Z + "}";
        }


        public static GraphPoint GetMidpoint(GraphPoint a, GraphPoint b)
        {
            return new GraphPoint(a.pos + (b.pos - a.pos) * 0.5f);
        }
    }
}
