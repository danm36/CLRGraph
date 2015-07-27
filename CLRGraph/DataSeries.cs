using clojure.lang;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Concurrent;
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
        PointCubes,
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
        private int dataSourceChannel = 0;
        private Timer dataSourcePollTimer = null;
        private const int dataSourcePollTimerMin = 10;
        private int lastPollInterval = 1000;

        private Matrix4 pointTransform = Matrix4.Identity;

        private bool pollHistoryEnabled = false;
        private double pollHistoryOffset = 1;
        private int pollHistoryLimit = 10;

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

            GL.UseProgram(DrawShader.shaderProgramHandle);
            GL.UniformMatrix4(DrawShader.uVertexOffsetLocation, false, ref pointTransform);

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
            if(DrawShader != null)
                DrawShader.Dispose();
            DrawShader = newShader;
        }

        public DataSeries Reset()
        {
            StopDataSourcePoll();
            ClearDataPoints();

            if (dataSource != null)
                dataSource.GraphReset();

            ResetTransforms();

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

            DataPoints = PersistentVector.create1(dpList);// (PersistentVector)DataPoints.asTransient().conj(newPoints.ToArray()).persistent();

            Parallel.For(0, DataPoints.Count, (i) =>
                {
                    ((GraphPoint)DataPoints[i]).index = i;
                });

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

        public DataSeries SetDataSourceChannel(int channel)
        {
            dataSourceChannel = channel;
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

        public DataSeries SetSeriesPollHistory(bool bEnabled, double offset, int limit)
        {
            pollHistoryEnabled = bEnabled;
            pollHistoryOffset = offset;
            pollHistoryLimit = limit;

            return this;
        }

        bool bStillUpdating = false;
        public void UpdatePointsFromDataSource()
        {
            if (dataSource == null)
            {
                if (dataSourcePollTimer != null)
                    StopDataSourcePoll();

                return;
            }

            if (bStillUpdating || !dataSource.NeedToGetNewData(dataSourceChannel))
                return;

            bStillUpdating = true;

            if (pollHistoryEnabled)
            {
                //OffsetPoints(new Vector3(0, 0, (float)pollHistoryOffset));
                Filter((p) => {
                    p.z += (float)pollHistoryOffset;
                    return p.z < pollHistoryOffset * pollHistoryLimit;
                });
                AddDataPoints(dataSource.GetData(dataSourceChannel));
            }
            else
            {
                SetDataPoints(dataSource.GetData(dataSourceChannel));
            }

            bStillUpdating = false;
        }

        private void UpdateVertexVBO()
        {
            ValidateDatapointEdges();

            List<float> points = new List<float>();
            for (int i = 0; i < DataPoints.Count; i++)
                points.AddRange(((GraphPoint)DataPoints[i]).GetVertices(DrawMode));

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * points.Count), points.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            UpdateIndexVBO();
        }

        public void UpdateIndexVBO()
        {
            if (Hidden || DrawMode == CLRGraph.DrawMode.Points)
                return;

            ValidateDatapointEdges();

            Parallel.ForEach(DataPoints, p =>
                {
                    ((GraphPoint)p).hasSetIndices = false;
                });

            List<uint> indexList = new List<uint>();
            for (int i = 0; i < DataPoints.Count; i++)
                indexList.AddRange(((GraphPoint)DataPoints[i]).GetIndices(DrawMode));

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

            UpdateVertexVBO();
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
                GL.VertexAttribPointer(DrawShader.vertexAttribLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                GL.EnableVertexAttribArray(DrawShader.normalAttribLocation);
                GL.VertexAttribPointer(DrawShader.normalAttribLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

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
                    case CLRGraph.DrawMode.PointCubes:
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


        public DataSeries TranslatePoints(Vector3 offset)
        {
            return TransformPoints(Matrix4.CreateTranslation(offset));
        }

        public DataSeries ScalePoints(Vector3 scale)
        {
            return TransformPoints(Matrix4.CreateScale(scale));
        }

        public DataSeries RotatePoints(Vector3 angles)
        {
            return TransformPoints(Matrix4.CreateRotationZ(angles.Z) * Matrix4.CreateRotationY(angles.Y) * Matrix4.CreateRotationX(angles.X));
        }

        public DataSeries TransformPoints(Matrix4 transform)
        {
            pointTransform *= transform;
            GL.UseProgram(DrawShader.shaderProgramHandle);
            GL.UniformMatrix4(DrawShader.uVertexOffsetLocation, false, ref pointTransform);
            return this;
        }

        public DataSeries ResetTransforms()
        {
            pointTransform = Matrix4.Identity;
            GL.UseProgram(DrawShader.shaderProgramHandle);
            GL.UniformMatrix4(DrawShader.uVertexOffsetLocation, false, ref pointTransform);
            return this;
        }

        public DataSeries BakeTransforms()
        {
            Parallel.ForEach(DataPoints, p =>
                {
                    GraphPoint point = (GraphPoint)p;
                    point.pos = Vector3.Transform(point.pos, pointTransform);
                });

            UpdateVertexVBO();
            return ResetTransforms();
        }



        public DataSeries Filter(Func<GraphPoint, bool> func)
        {
            /*
            ConcurrentBag<GraphPoint> remaining = new ConcurrentBag<GraphPoint>();
            Parallel.ForEach(DataPoints, (p) =>
            {
                GraphPoint point = (GraphPoint)p;
                if (func.Invoke(point))
                    remaining.Add(point);
            });

            DataPoints = PersistentVector.create1(remaining);*/
            DataPoints = PersistentVector.create1(DataPoints.Where(p => func.Invoke((GraphPoint)p)).ToList());

            return this;
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
        public bool hasSetIndices = false;

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

        public List<float> GetVertices(DrawMode mode)
        {
            //Vector3 transformed = Vector3.Transform(pos, transform);
            Vector3 transformed = pos;

            if (mode == DrawMode.PointCubes)
            {
                return new List<float>() { //FIXME NORMALS
                    transformed.X - 0.5f, transformed.Y - 0.5f, transformed.Z - 0.5f,   0, 1, 0,
                    transformed.X + 0.5f, transformed.Y - 0.5f, transformed.Z - 0.5f,   0, 1, 0,
                    transformed.X + 0.5f, transformed.Y + 0.5f, transformed.Z - 0.5f,   0, 1, 0,
                    transformed.X - 0.5f, transformed.Y + 0.5f, transformed.Z - 0.5f,   0, 1, 0,

                    transformed.X - 0.5f, transformed.Y - 0.5f, transformed.Z + 0.5f,   0, 1, 0,
                    transformed.X + 0.5f, transformed.Y - 0.5f, transformed.Z + 0.5f,   0, 1, 0,
                    transformed.X + 0.5f, transformed.Y + 0.5f, transformed.Z + 0.5f,   0, 1, 0,
                    transformed.X - 0.5f, transformed.Y + 0.5f, transformed.Z + 0.5f,   0, 1, 0,

                };
            }

            Vector3 normal = new Vector3(0, 1, 0);
            if (edges.Count >= 2)
            {
                GraphPoint last = null;
                normal = new Vector3(0, 0, 0);
                float count = 0;
                foreach (GraphPoint point in edges)
                {
                    if (last == null)
                    {
                        last = point;
                        continue;
                    }

                    normal += Vector3.Cross(last.pos - pos, point.pos - pos);
                    ++count;
                    last = point;
                }

                normal = (normal / count).Normalized();
            }
            return new List<float>() { transformed.X, transformed.Y, transformed.Z, normal.X, normal.Y, normal.Z };
        }

        public List<uint> GetIndices(DrawMode mode)
        {
            List<uint> indices = new List<uint>();
            uint uindex = (uint)index;

            switch (mode)
            {
                default:
                    break;

                case DrawMode.Lines:
                    foreach (GraphPoint point in edges)
                    {
                        indices.Add((uint)index);
                        indices.Add((uint)point.index);
                    }
                    break;

                case DrawMode.Triangles:
                    foreach (GraphPoint point in edges)
                        indices.AddRange(FormTrianglesWith(point));
                    break;

                case DrawMode.Quads:
                    foreach (GraphPoint point in edges)
                        indices.AddRange(FormQuadsWith(point));
                    break;

                case DrawMode.PointCubes:
                    //Front
                    indices.Add(uindex * 8 + 3);
                    indices.Add(uindex * 8 + 2);
                    indices.Add(uindex * 8 + 1);
                    indices.Add(uindex * 8 + 0);

                    //Back
                    indices.Add(uindex * 8 + 4);
                    indices.Add(uindex * 8 + 5);
                    indices.Add(uindex * 8 + 6);
                    indices.Add(uindex * 8 + 7);

                    //Left
                    indices.Add(uindex * 8 + 3);
                    indices.Add(uindex * 8 + 0);
                    indices.Add(uindex * 8 + 4);
                    indices.Add(uindex * 8 + 7);

                    //Right
                    indices.Add(uindex * 8 + 1);
                    indices.Add(uindex * 8 + 2);
                    indices.Add(uindex * 8 + 6);
                    indices.Add(uindex * 8 + 5);

                    //Top
                    indices.Add(uindex * 8 + 2);
                    indices.Add(uindex * 8 + 3);
                    indices.Add(uindex * 8 + 7);
                    indices.Add(uindex * 8 + 6);

                    //Bottom
                    indices.Add(uindex * 8 + 0);
                    indices.Add(uindex * 8 + 1);
                    indices.Add(uindex * 8 + 5);
                    indices.Add(uindex * 8 + 4);
                    break;
            }

            hasSetIndices = true;
            return indices;
        }

        public List<uint> FormTrianglesWith(GraphPoint other, bool bForQuadRendering = false)
        {
            List<uint> results = new List<uint>();
            if (other.hasSetIndices)
                return results;

            List<GraphPoint> intersection = edges.Intersect(other.edges).ToList();

            for (int i = 0; i < intersection.Count; i++)
            {
                if (intersection[i].hasSetIndices || intersection[i] == this || intersection[i] == other)
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
            if (other.hasSetIndices)
                return results;

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
                            if (intersection[i].hasSetIndices || intersection[i] == this || intersection[i] == edge1 || intersection[i] == edge2)
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
