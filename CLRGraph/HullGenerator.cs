using clojure.lang;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    [ClojureClass]
    public static class HullGenerator
    {
        const int Default_MaxIterationCount = 16;
        static int MaxIterationCount = Default_MaxIterationCount;


        class HullFace
        {
            public GraphPoint a, b, c;
            public HullEdge ab, ca, bc;
            public Vector3 normal;
            public Vector3 center;
            public List<GraphPoint> points;

            public HullFace(GraphPoint nA, GraphPoint nB, GraphPoint nC)
            {
                a = nA;
                b = nB;
                c = nC;

                ab = new HullEdge(a, b);
                bc = new HullEdge(b, c);
                ca = new HullEdge(c, a);

                normal = GetTriangleNormal(a.pos, b.pos, c.pos);
                center = GetTriangleCenter(a.pos, b.pos, c.pos);

                points = new List<GraphPoint>();
            }

            public HullFace(GraphPoint nA, GraphPoint nB, GraphPoint nC, List<GraphPoint> nMyPoints)
                :   this(nA, nB, nC)
            {
                points = nMyPoints;
            }

            public bool IsAVertex(GraphPoint p)
            {
                return p == a || p == b || p == c;
            }

            public List<HullEdge> GetSharedEdges(HullFace other)
            {
                List<HullEdge> result = new List<HullEdge>();

                if (ab.IsContainedIn(other))
                    result.Add(ab);
                if (ca.IsContainedIn(other))
                    result.Add(ca);
                if (bc.IsContainedIn(other))
                    result.Add(bc);

                return result;
            }
        }

        class HullEdge
        {
            public GraphPoint a;
            public GraphPoint b;

            public HullEdge(GraphPoint nA, GraphPoint nB)
            {
                a = nA;
                b = nB;
            }

            public bool Equivalent(HullEdge other)
            {
                return (a == other.a && b == other.b) || (b == other.a && a == other.b);
            }

            public bool IsContainedIn(HullFace other)
            {
                return Equivalent(other.ab) || Equivalent(other.ca) || Equivalent(other.bc);
            }
        }

        //static Queue<HullFace> PendingFaces = new Queue<HullFace>();
        static List<HullFace> PendingFaces = new List<HullFace>();
        static List<HullFace> ActiveFaces = new List<HullFace>();

        [ClojureStaticMethod("make-hull", "Creates a hull from the given series with the given number of iterations. If no series is supplied, the current series is used. If no iteration count is supplied, then the default of 16 iterations will be used")]
        public static DataSeries CreateHullFromSeries()
        {
            return CreateHullFromSeries(DataSeries_Funcs.GetCurrentDataSeries(), Default_MaxIterationCount);
        }

        [ClojureStaticMethod("make-hull", "Creates a hull from the given series with the given number of iterations. If no series is supplied, the current series is used. If no iteration count is supplied, then the default of 16 iterations will be used")]
        public static DataSeries CreateHullFromSeries(int iterations)
        {
            return CreateHullFromSeries(DataSeries_Funcs.GetCurrentDataSeries(), iterations);
        }

        [ClojureStaticMethod("make-hull", "Creates a hull from the given series with the given number of iterations. If no series is supplied, the current series is used. If no iteration count is supplied, then the default of 16 iterations will be used")]
        public static DataSeries CreateHullFromSeries(DataSeries source)
        {
            return CreateHullFromSeries(source, Default_MaxIterationCount);
        }

        [ClojureStaticMethod("make-hull", "Creates a hull from the given series with the given number of iterations. If no series is supplied, the current series is used. If no iteration count is supplied, then the default of 16 iterations will be used")]
        public static DataSeries CreateHullFromSeries(DataSeries source, int iterationCount)
        {
            GraphPoint[] edges = source.GetEdgemostPoints(true);

            if (edges == null)
                return null;

            MaxIterationCount = iterationCount;

            for (int i = 0; i < edges.Length; i++)
                edges[i] = new GraphPoint(edges[i]);

            PendingFaces.Clear();
            ActiveFaces.Clear();

            List<GraphPoint> AvailablePoints = new List<GraphPoint>();
            foreach (object point in source.DataPoints)
                AvailablePoints.Add(new GraphPoint((GraphPoint)point));

            //DataSeries hullSeries = new DataSeries(source.Name + "_Hull", source.DrawColor);
            DataSeries hullSeries = new DataSeries(source.Name + "_Hull", null);
            GraphPoint baseLine1 = null, baseLine2 = null, farthestBaseLine = null, farthestBaseFace = null;
            
            double bestDist = 0, curDist = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                for (int j = 1; j < edges.Length; j++)
                {
                    if (i == j)
                        continue;

                    curDist = edges[i].DistanceFromSq(edges[j]);
                    if (curDist > bestDist)
                    {
                        baseLine1 = edges[i];
                        baseLine2 = edges[j];
                        bestDist = curDist;
                    }
                }
            }

            bestDist = 0;
            for (int i = 0; i < edges.Length; i++)
            {
                if (edges[i].pos == baseLine1.pos || edges[i].pos == baseLine2.pos)
                    continue;

                curDist = edges[i].DistanceFromLineSegSq(baseLine1, baseLine2);
                if (curDist > bestDist)
                {
                    farthestBaseLine = edges[i];
                    bestDist = curDist;
                }
            }

            Vector3 triangleNormal = GetTriangleNormal(baseLine1.pos, baseLine2.pos, farthestBaseLine.pos);
            Vector3 triangleCenter = GetTriangleCenter(baseLine1.pos, baseLine2.pos, farthestBaseLine.pos);
            float triangleDot = Vector3.Dot(triangleNormal, baseLine1.pos);
            bestDist = 0;

            bool bFlipWinding = false;

            for (int i = 0; i < AvailablePoints.Count; i++)
            {
                if (AvailablePoints[i] == baseLine1 || AvailablePoints[i] == baseLine2 || AvailablePoints[i] == farthestBaseLine)
                    continue;

                //curDist = (AvailablePoints[i].pos - triangleCenter).LengthSquared;
                curDist = Math.Abs(Vector3.Dot(AvailablePoints[i].pos, triangleNormal) - triangleDot);
                if (curDist > bestDist)
                {
                    farthestBaseFace = AvailablePoints[i];
                    bestDist = curDist;

                    bFlipWinding = Vector3.Dot(triangleNormal, (AvailablePoints[i].pos - triangleCenter).Normalized()) > 0;
                }
            }

            if (bFlipWinding)
            {
                GraphPoint temp = baseLine1;
                baseLine1 = baseLine2;
                baseLine2 = temp;
            }

            List<HullFace> initialFaces = new List<HullFace>() { 
                new HullFace(baseLine2, baseLine1, farthestBaseLine),
                new HullFace(baseLine1, baseLine2, farthestBaseFace),
                new HullFace(baseLine1, farthestBaseFace, farthestBaseLine),
                new HullFace(baseLine2, farthestBaseLine, farthestBaseFace),
             };

            AssignPointsToFaces(AvailablePoints, initialFaces);

            for (int i = 0; i < initialFaces.Count; i++)
            {
                ActiveFaces.Add(initialFaces[i]);
                if(initialFaces[i].points.Count > 0)
                    PendingFaces.Add(initialFaces[i]);
            }

            BuildFromStack(0);

            int unassignedPoints = 0;
            for (int i = 0; i < ActiveFaces.Count; i++)
            {
                ActiveFaces[i].a.AddEdges(ActiveFaces[i].b, ActiveFaces[i].c);
                ActiveFaces[i].b.AddEdges(ActiveFaces[i].a, ActiveFaces[i].c);
                ActiveFaces[i].c.AddEdges(ActiveFaces[i].a, ActiveFaces[i].b);

                hullSeries.AddDataPoints(new List<GraphPoint>() { ActiveFaces[i].a, ActiveFaces[i].b, ActiveFaces[i].c });

                unassignedPoints += ActiveFaces[i].points.Count;

                //if(ActiveFaces[i].points.Count > 0)
                //    new DataSeries(PersistentVector.create1(ActiveFaces[i].points));
            }

            if(unassignedPoints > 0)
                ClojureEngine.Log("[HULL BUILD] " + unassignedPoints + " points remain outside the hull");
            else
                ClojureEngine.Log("[HULL BUILD] All points are contained within the hull");

            hullSeries.SetDrawMode(DrawMode.Triangles);
            return hullSeries;
        }

        private static void BuildFromStack(int iteration)
        {
            if (iteration >= MaxIterationCount || PendingFaces.Count == 0)
            {
                ClojureEngine.Log("[HULL BUILD] Hull built in " + iteration + " iterations");
                return;
            }

            PendingFaces = PendingFaces.OrderByDescending(o => o.points.Count).ToList();

            HullFace curFace = null;
            while(PendingFaces.Count > 0)
            {
                curFace = PendingFaces[0];

                if (ActiveFaces.Contains(curFace))
                    break;

                PendingFaces.RemoveAt(0);
            }

            if (PendingFaces.Count == 0)
            {
                ClojureEngine.Log("[HULL BUILD] Hull built in " + iteration + " iterations");
                return;
            }

            //Temp
            DBG_AddCurrentFaceState("itr_" + iteration);
            //End Temp

            List<GraphPoint> pointsNeedingReassign = new List<GraphPoint>();

            GraphPoint furthestPoint = null;
            float bestDist = 0;
            float curDist = 0;
            Vector3 triangleNormal = curFace.normal;
            float triangleDot = Vector3.Dot(triangleNormal, curFace.a.pos);
            for (int i = 0; i < curFace.points.Count; i++)
            {
                if (curFace.points[i].pos == curFace.a.pos || curFace.points[i].pos == curFace.b.pos || curFace.points[i].pos == curFace.c.pos)
                    continue;

                curDist = Math.Abs(Vector3.Dot(curFace.points[i].pos, triangleNormal) - triangleDot);
                if (curDist > bestDist)
                {
                    furthestPoint = curFace.points[i];
                    bestDist = curDist;
                }
            }

            //Lit faces are those with a normal pointing towards us
            List<HullFace> litFaces = new List<HullFace>();
            for (int i = 0; i < ActiveFaces.Count; i++)
            {
                if (Vector3.Dot(ActiveFaces[i].normal, (furthestPoint.pos - ActiveFaces[i].center)) < 0)
                {
                    litFaces.Add(ActiveFaces[i]);
                }
            }

            //Find horizon ring and detach old faces
            //Since this is a convex hull, we can assume that the horizon of all lit faces are convex
            //FIXME: Some internal triangles are still built
            List<Tuple<GraphPoint, GraphPoint>> horizonPairs = new List<Tuple<GraphPoint, GraphPoint>>();
            for (int i = 0; i < litFaces.Count; i++)
            {
                pointsNeedingReassign.AddRange(litFaces[i].points);
                ActiveFaces.Remove(litFaces[i]);

                List<HullEdge> edges = new List<HullEdge>();
                //int acount = 0, bcount = 0, ccount = 0;
                for (int j = 0; j < litFaces.Count; j++)
                {
                    if (i == j)
                        continue;

                    edges = edges.Union(litFaces[i].GetSharedEdges(litFaces[j])).ToList();

                    if (edges.Count >= 3)
                        break;
                }

                if(edges.Count >= 3)
                {
                    litFaces.RemoveAt(i);
                    --i;
                    continue;
                }

                if (!edges.Contains(litFaces[i].ab))
                    horizonPairs.Add(new Tuple<GraphPoint,GraphPoint>(litFaces[i].a, litFaces[i].b));
                
                if (!edges.Contains(litFaces[i].bc))
                    horizonPairs.Add(new Tuple<GraphPoint, GraphPoint>(litFaces[i].b, litFaces[i].c));
                
                if (!edges.Contains(litFaces[i].ca))
                    horizonPairs.Add(new Tuple<GraphPoint, GraphPoint>(litFaces[i].c, litFaces[i].a));
            }

            //Temp
            DBG_AddCurrentFaceState("itr_" + iteration + "_nolit");
            //End Temp


            List<HullFace> newFaces = new List<HullFace>();
            //Create new faces
            for (int i = 0; i < horizonPairs.Count; i++)
            {
                newFaces.Add(new HullFace(horizonPairs[i].Item1, horizonPairs[i].Item2, furthestPoint));
            }

            AssignPointsToFaces(pointsNeedingReassign, newFaces);

            for (int i = 0; i < newFaces.Count; i++)
            {
                ActiveFaces.Add(newFaces[i]);

                if(newFaces[i].points.Count > 0)
                    PendingFaces.Add(newFaces[i]);
            }

            BuildFromStack(iteration + 1);
        }

        private static void AssignPointsToFaces(List<GraphPoint> points, List<HullFace> faces)
        {
            float dot = 0, bestDot = 0;
            HullFace bestCandidateFace = null;
            for (int i = 0; i < points.Count; i++)
            {
                bestCandidateFace = null;
                bestDot = 0;

                for (int j = 0; j < faces.Count; j++)
                {
                    if (points[i].pos == faces[j].a.pos || points[i].pos == faces[j].b.pos || points[i].pos == faces[j].c.pos)
                        continue;

                    dot = Vector3.Dot(faces[j].normal, (faces[j].center - points[i].pos).Normalized());

                    if (dot > bestDot)
                    {
                        bestDot = dot;
                        bestCandidateFace = faces[j];
                    }
                }

                if (bestCandidateFace != null)
                    bestCandidateFace.points.Add(points[i]);
            }
        }

        private static Vector3 GetTriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 v = p2 - p1;
            Vector3 w = p3 - p1;

            Vector3 n = Vector3.Cross(v, w);
            float magSq = n.LengthSquared;

            if (magSq == 0)
            {
                ClojureEngine.Log("Triangle is degenerate. Could not get normal!");
                return Vector3.Zero;
            }

            return n.Normalized();
        }

        private static Vector3 GetTriangleCenter(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return new Vector3( (p1.X + p2.X + p3.X) / 3,
                                (p1.Y + p2.Y + p3.Y) / 3,
                                (p1.Z + p2.Z + p3.Z) / 3);
        }

        private static void DBG_AddCurrentFaceState(string name)
        {
            return;

            List<GraphPoint> itrPoints = new List<GraphPoint>();
            for (int i = 0; i < ActiveFaces.Count; i++)
            {
                GraphPoint a = new GraphPoint(ActiveFaces[i].a);
                GraphPoint b = new GraphPoint(ActiveFaces[i].b);
                GraphPoint c = new GraphPoint(ActiveFaces[i].c);
                a.AddEdges(b, c);
                b.AddEdges(a, c);
                c.AddEdges(a, b);

                itrPoints.Add(a);
                itrPoints.Add(b);
                itrPoints.Add(c);
            }
            new DataSeries(PersistentVector.create1(itrPoints), name);
        }
    }
}
