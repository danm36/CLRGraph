using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    public class GraphPoint
    {
        public int index = -1;
        public Vector3 pos = new Vector3();
        public float x { get { return pos.X; } set { pos.X = value; } }
        public float y { get { return pos.Y; } set { pos.Y = value; } }
        public float z { get { return pos.Z; } set { pos.Z = value; } }

        public HashSet<int> pendingEdges = null;
        public HashSet<GraphPoint> edges = null;
        public bool hasSetIndices = false;

        public GraphPoint(Vector3 nPos)
        {
            index = DataSeries_Funcs.GetCurrentDataSeries().GetPointCount();

            pos = nPos;
        }

        public GraphPoint(double nX, double nY, double nZ)
            : this(new Vector3((float)nX, (float)nY, (float)nZ))
        {
        }

        public GraphPoint(Vector3 nPos, IList<int> nEdges)
            : this(nPos)
        {
            AddEdges(nEdges);
        }

        public GraphPoint(double nX, double nY, double nZ, IList<int> nEdges)
            : this(nX, nY, nZ)
        {
            AddEdges(nEdges);
        }

        public GraphPoint(Vector3 nPos, IList<GraphPoint> nEdges)
            : this(nPos)
        {
            AddEdges(nEdges);
        }

        public GraphPoint(double nX, double nY, double nZ, IList<GraphPoint> nEdges)
            : this(nX, nY, nZ)
        {
            AddEdges(nEdges);
        }

        public GraphPoint(GraphPoint other)
        {
            if (other == null)
                return;

            index = DataSeries_Funcs.GetCurrentDataSeries().GetPointCount();

            pos = other.pos;

            if (other.pendingEdges != null)
            {
                foreach (int edge in other.pendingEdges)
                    AddEdge(edge);
            }

            if (other.edges != null)
            {
                foreach (GraphPoint edge in other.edges)
                    AddEdge(edge);
            }
        }

        public void AddEdge(int edge)
        {
            if(pendingEdges == null)
                pendingEdges = new HashSet<int>();

            pendingEdges.Add(edge);
        }

        public void AddEdge(GraphPoint edge)
        {
            if (edges == null)
                edges = new HashSet<GraphPoint>();

            edges.Add(edge);
        }

        public void AddEdges(IList<int> nEdges)
        {
            if (pendingEdges == null)
                pendingEdges = new HashSet<int>();

            for (int i = 0; i < nEdges.Count; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public void AddEdges(IList<GraphPoint> nEdges)
        {
            if (edges == null)
                edges = new HashSet<GraphPoint>();

            for (int i = 0; i < nEdges.Count; i++)
            {
                edges.Add(nEdges[i]);
            }
        }

        public void AddEdges(params int[] nEdges)
        {
            if (pendingEdges == null)
                pendingEdges = new HashSet<int>();

            for (int i = 0; i < nEdges.Length; i++)
            {
                pendingEdges.Add(nEdges[i]);
            }
        }

        public void AddEdges(params GraphPoint[] nEdges)
        {
            if (edges == null)
                edges = new HashSet<GraphPoint>();

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
                    transformed.X - 0.5f, transformed.Y - 0.5f, transformed.Z - 0.5f,   -1, -1, -1,
                    transformed.X + 0.5f, transformed.Y - 0.5f, transformed.Z - 0.5f,   1, -1, -1,
                    transformed.X + 0.5f, transformed.Y + 0.5f, transformed.Z - 0.5f,   1, 1, -1,
                    transformed.X - 0.5f, transformed.Y + 0.5f, transformed.Z - 0.5f,   -1, 1, -1,

                    transformed.X - 0.5f, transformed.Y - 0.5f, transformed.Z + 0.5f,   -1, -1, 1,
                    transformed.X + 0.5f, transformed.Y - 0.5f, transformed.Z + 0.5f,   1, -1, 1,
                    transformed.X + 0.5f, transformed.Y + 0.5f, transformed.Z + 0.5f,   1, 1, 1,
                    transformed.X - 0.5f, transformed.Y + 0.5f, transformed.Z + 0.5f,   -1, 1, 1,

                };
            }

            Vector3 normal = new Vector3(0, 1, 0);
            if (edges != null && edges.Count >= 2)
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
                    if (edges != null)
                    {
                        foreach (GraphPoint point in edges)
                        {
                            indices.Add((uint)index);
                            indices.Add((uint)point.index);
                        }
                    }
                    break;

                case DrawMode.Triangles:
                    if (edges != null)
                    {
                        foreach (GraphPoint point in edges)
                            indices.AddRange(FormTrianglesWith(point));
                    }
                    break;

                case DrawMode.Quads:
                    if (edges != null)
                    {
                        foreach (GraphPoint point in edges)
                            indices.AddRange(FormQuadsWith(point));
                    }
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
            if (edges == null || other.edges == null || other.hasSetIndices)
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
            if (edges == null || other.hasSetIndices)
                return results;

            foreach (GraphPoint edge1 in edges)
            {
                if (edge1.edges == null)
                    continue;

                foreach (GraphPoint edge2 in edges)
                {
                    if (edge1 == edge2 || edge2.edges == null)
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
