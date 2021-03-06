﻿using clojure.lang;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    [ClojureClass]
    public static class Graph_Funcs
    {
        [ClojureStaticMethod("reset-graph", "Resets the entire graph back to its default state, removing all points and series.")]
        public static void ResetGraph()
        {
            DataSeries.ResetAll();
            ClojureDefinedUI.Reset();

            GLGraph.self.SetBackgroundColor(Color.White);
            GLGraph.self.UpdateGraphAxes();
            GLGraph.self.UpdateMatrices(true);
        }

        #region Datapoint Control
        [ClojureStaticMethod("make-data-points", "Creates and returns a set of points from the given values")]
        public static List<GraphPoint> MakeDataPoints(IList yVals)
        {
            return MakeDataPoints(yVals, 0, 1);
        }

        [ClojureStaticMethod("make-data-points", "Creates and returns a set of points from the given values")]
        public static List<GraphPoint> MakeDataPoints(IList yVals, double xIncrement)
        {
            return MakeDataPoints(yVals, 0, xIncrement);
        }

        [ClojureStaticMethod("make-data-points", "Creates and returns a set of points from the given values")]
        public static List<GraphPoint> MakeDataPoints(IList inYVals, double xStart, double xIncrement)
        {
            double xVal = xStart;
            List<GraphPoint> existingGraphPoints = new List<GraphPoint>();
            List<double> xVals = new List<double>();
            List<double> yVals = new List<double>();
            List<double> zVals = new List<double>();

            int count = 0;

            foreach (object obj in inYVals)
            {
                if (obj is GraphPoint)
                {
                    existingGraphPoints.Add(new GraphPoint((GraphPoint)obj));
                    continue;
                }

                xVals.Add(xVal);
                xVal += xIncrement;
                zVals.Add(0);

                ++count;
                if (count > ClojureEngine.ClojureMaxIterations)
                {
                    ClojureEngine.Log("Supplied y values collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping further point additon.");
                    break;
                }
            }

            List<GraphPoint> otherPoints = MakeDataPoints3D(xVals, inYVals, zVals);
            existingGraphPoints.AddRange(otherPoints);
            return existingGraphPoints;
        }

        [ClojureStaticMethod("make-data-points", "Creates and returns a set of points from the given values")]
        public static List<GraphPoint> MakeDataPoints(IList xVals, IList yVals)
        {
            return MakeDataPoints(xVals, yVals, null);
        }

        [ClojureStaticMethod("make-data-points", "Creates and returns a set of points from the given values")]
        public static List<GraphPoint> MakeDataPoints(IList xVals, IList yVals, object indices)
        {
            List<double> zVals = new List<double>();

            int count = 0;

            foreach (object obj in xVals)
            {
                zVals.Add(0);

                ++count;
                if (count > ClojureEngine.ClojureMaxIterations)
                {
                    ClojureEngine.Log("Supplied y values collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping further point additon.");
                    break;
                }
            }

            return MakeDataPoints3D(xVals, yVals, zVals, indices);
        }

        [ClojureStaticMethod("make-data-points-3d", "Creates and returns a set of 3D points from the given values")]
        public static List<GraphPoint> MakeDataPoints3D(IList xVals, IList yVals, IList zVals)
        {
            return MakeDataPoints3D(xVals, yVals, zVals, null);
        }

        [ClojureStaticMethod("make-data-points-3d", "Creates and returns a set of 3D points from the given values")]
        public static List<GraphPoint> MakeDataPoints3D(IList xVals, IList yVals, IList zVals, object edgeInputVals)
        {
            if (edgeInputVals != null && !(edgeInputVals is IList))
            {
                ClojureEngine.Log("Supplied index list not an enumerable collection or null. Stopping.");
                return null;
            }

            List<GraphPoint> graphPoints = new List<GraphPoint>();

            IList edgeList = (IList)edgeInputVals;
            double xVal = 0;
            double yVal = 0;
            double zVal = 0;
            int edgeWorkVal = 0;
            List<int> edgesVal = new List<int>();
            int count = 0;

            foreach (object obj in xVals)
            {
                if (obj is GraphPoint)
                {
                    graphPoints.Add((GraphPoint)obj);
                    continue;
                }

                edgesVal = new List<int>();

                try
                {
                    if (!double.TryParse(yVals[count].ToString(), out yVal))
                    {
                        ClojureEngine.Log("Y value '" + yVals[count].ToString() + " could not be converted to double. Defaulted to '0'.");
                        yVal = 0;
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    ClojureEngine.Log("Y Value collection shorter than X Value collection. Stopping.");
                    break;
                }

                try
                {
                    if (!double.TryParse(zVals[count].ToString(), out zVal))
                    {
                        ClojureEngine.Log("Z value '" + zVals[count].ToString() + " could not be converted to double. Defaulted to '0'.");
                        zVal = 0;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    ClojureEngine.Log("Z Value collection shorter than X Value collection. Stopping.");
                    break;
                }

                if (!double.TryParse(obj.ToString(), out xVal))
                {
                    ClojureEngine.Log("X Value could not be converted to double. Could not add point to graph '" + obj.ToString() + "'");
                    continue;
                }

                if (edgeList != null)
                {
                    if (edgeList[count] is IList)
                    {
                        foreach (object indexObj in (IList)edgeList[count])
                        {
                            try
                            {
                                edgeWorkVal = int.Parse(indexObj.ToString());
                                edgesVal.Add(edgeWorkVal);
                            }
                            catch
                            {
                                ClojureEngine.Log("Edge '" + indexObj.ToString() + "' for point " + count + " not integer. Skipping.");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            edgeWorkVal = int.Parse(edgeList[count].ToString());
                            edgesVal.Add(edgeWorkVal);
                        }
                        catch
                        {
                            ClojureEngine.Log("Edge '" + edgeList.ToString() + "' for point " + count + " not integer. Skipping.");
                        }
                    }
                }

                graphPoints.Add(new GraphPoint(xVal, yVal, zVal, edgesVal));

                ++count;

                if (count > ClojureEngine.ClojureMaxIterations)
                {
                    ClojureEngine.Log("Supplied X values collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping further point additon.");
                    break;
                }
            }

            return graphPoints;
        }

        [ClojureStaticMethod("make-plot", "Creates a 2D plot from the given points")]
        public static List<GraphPoint> MakePlot(int xStart, double increment, IList vals)
        {
            List<GraphPoint> points = new List<GraphPoint>();

            for (int i = 0; i < vals.Count; i++)
            {
                points.Add(new GraphPoint(xStart + ((double)i * increment), double.Parse(vals[i].ToString()), 0));
            }

            return points;
        }

        [ClojureStaticMethod("make-plot-fn", "Creates a 2D plot from the given points")]
        public static List<GraphPoint> MakePlotFN(double xStart, double xEnd, double precision, IFn function)
        {
            return MakePlotFN(xStart, xEnd, precision, function, 0);
        }

        [ClojureStaticMethod("make-plot-fn", "Creates a 2D plot from the given points")]
        public static List<GraphPoint> MakePlotFN(double xStart, double xEnd, double precision, IFn function, double elapsedTime)
        {
            List<GraphPoint> points = new List<GraphPoint>();

            for (double x = xStart; x <= xEnd; x += precision)
            {
                try
                {
                    points.Add(new GraphPoint(x, (double)function.invoke(x, 0, 0, elapsedTime), 0));
                }
                catch { }
            }

            return points;
        }

        [ClojureStaticMethod("make-plot-3d-fn", "Creates a 2D plot from the given points")]
        public static List<GraphPoint> MakePlot3DFN(double xStart, double xEnd, double precision, IFn functionY, IFn functionZ)
        {
            return MakePlot3DFN(xStart, xEnd, precision, functionY, functionZ, 0);
        }

        [ClojureStaticMethod("make-plot-3d-fn", "Creates a 2D plot from the given points")]
        public static List<GraphPoint> MakePlot3DFN(double xStart, double xEnd, double precision, IFn functionY, IFn functionZ, double elapsedTime)
        {
            List<GraphPoint> points = new List<GraphPoint>();

            if(functionY == null && functionZ == null)
                return points;

            if(functionY != null && functionZ != null)
            {
                for (double x = xStart; x <= xEnd; x += precision)
                {
                    try { points.Add(new GraphPoint(x, (double)functionY.invoke(x, 0, 0, elapsedTime),(double)functionZ.invoke(x, 0, 0, elapsedTime))); }
                    catch { }
                }
            }
            else if (functionY == null)
            {
                for (double x = xStart; x <= xEnd; x += precision)
                {
                    try { points.Add(new GraphPoint(x, 0, (double)functionZ.invoke(x, 0, 0, elapsedTime))); }
                    catch { }
                }
            }
            else if (functionZ == null)
            {
                for (double x = xStart; x <= xEnd; x += precision)
                {
                    try { points.Add(new GraphPoint(x, (double)functionY.invoke(x, 0, 0, elapsedTime), 0)); }
                    catch { }
                }
            }

            return points;
        }

        [ClojureStaticMethod("make-3d-surface", "Creates and returns a set of points forming a square surface with the given Y values, optionally centered on the axes")]
        public static List<GraphPoint> Make3DSurface(int xCount, int zCount, IList vals)
        {
            return Make3DSurface(0, 0, xCount, zCount, xCount, zCount, vals);
        }

        [ClojureStaticMethod("make-3d-surface", "Creates and returns a set of points forming a square surface with the given Y values, optionally centered on the axes")]
        public static List<GraphPoint> Make3DSurface(int xCount, int zCount, IList vals, bool centered)
        {
            return Make3DSurface(centered ? -xCount / 2 : 0, centered ? -zCount / 2 : 0, xCount, zCount, xCount, zCount, vals);
        }

        [ClojureStaticMethod("make-3d-surface", "Creates and returns a set of points forming a square surface with the given Y values, optionally centered on the axes")]
        public static List<GraphPoint> Make3DSurface(double width, double depth, int xCount, int zCount, IList vals)
        {
            return Make3DSurface(0, 0, width, depth, xCount, zCount, vals);
        }

        [ClojureStaticMethod("make-3d-surface", "Creates and returns a set of points forming a square surface with the given Y values, optionally centered on the axes")]
        public static List<GraphPoint> Make3DSurface(double width, double depth, int xCount, int zCount, IList vals, bool centered)
        {
            return Make3DSurface(centered ? (int)(-width / 2) : 0, centered ? (int)(-depth / 2) : 0, width, depth, xCount, zCount, vals);
        }

        [ClojureStaticMethod("make-3d-surface", "Creates and returns a set of points forming a square surface with the given Y values, optionally centered on the axes")]
        public static List<GraphPoint> Make3DSurface(int xStart, int zStart, double width, double depth, int xCount, int zCount, IList vals)
        {
            List<GraphPoint> points = new List<GraphPoint>();
            List<int> indices = new List<int>();

            if (width <= 0)
                width = xCount;
            if (depth <= 0)
                depth = zCount;

            double testYVal = 0;

            for (int z = 0; z < zCount; z++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    if (z * xCount + x >= vals.Count)
                    {
                        points.Add(new GraphPoint(0, 0, 0));
                        continue;
                    }
                    
                    if (!double.TryParse(vals[z * xCount + x].ToString(), out testYVal))
                    {
                        ClojureEngine.Log("Y Value '" + vals[z * xCount + x].ToString() + "' is not numeric. Defaulting to 0");
                        testYVal = 0;
                    }

                    /*indices = new List<int>();
                    if (z < zCount - 1 && x < xCount - 1)
                    {
                        indices.Add((z + 1) * xCount + x + 0);
                        indices.Add((z + 1) * xCount + x + 1);
                        indices.Add((z + 0) * xCount + x + 1);
                    }
                    */

                    points.Add(new GraphPoint((((double)x / xCount) * width) + xStart, testYVal, (((double)z / zCount) * depth) + zStart));
                }
            }

            for (int z = 0; z < zCount - 1; z++)
            {
                for (int x = 0; x < xCount - 1; x++)
                {
                    List<GraphPoint> edges = new List<GraphPoint>();
                    edges.Add(points[(z + 1) * xCount + x + 0]);
                    edges.Add(points[(z + 1) * xCount + x + 1]);
                    edges.Add(points[(z + 0) * xCount + x + 1]);

                    points[(z + 1) * xCount + x + 0].AddEdge(points[z * xCount + x]);
                    points[(z + 1) * xCount + x + 1].AddEdge(points[z * xCount + x]);
                    points[(z + 0) * xCount + x + 1].AddEdge(points[z * xCount + x]);

                    points[z * xCount + x].AddEdges(edges);
                }
            }

            return points;
        }

        [ClojureStaticMethod("get-data-points", "Returns a vector of all the graph points in the given series (Defaults to the current series)")]
        public static PersistentVector GetDataPoints()
        {
            return GetDataPoints(DataSeries_Funcs.GetCurrentDataSeries());
        }

        [ClojureStaticMethod("get-data-points", "Returns a vector of all the graph points in the given series (Defaults to the current series)")]
        public static PersistentVector GetDataPoints(DataSeries series)
        {
            return PersistentVector.create1((series ?? DataSeries_Funcs.GetCurrentDataSeries()).DataPoints);
        }

        [ClojureStaticMethod("get-data-points", "Returns a vector of all the graph points in the given series (Defaults to the current series)")]
        public static PersistentVector GetDataPoints(int seriesID)
        {
            DataSeries ds = DataSeries_Funcs.GetSeries(seriesID);

            if (ds == null)
                return PersistentVector.EMPTY;

            return PersistentVector.create1(ds.DataPoints);
        }

        [ClojureStaticMethod("add-data-points", "Adds the given points to the current series")]
        public static void AddDataPoints(List<GraphPoint> dataPoints)
        {
            if (dataPoints == null)
                return;

            DataSeries currentSeries = DataSeries_Funcs.GetCurrentDataSeries();
            int added = currentSeries.AddDataPoints(dataPoints);
            GLGraph.self.UpdateGraphAxes();
            GLGraph.self.UpdateMatrices(true);

            //ClojureEngine.Log("Added " + added + " points to series '" + currentSeries.Name + "' (" + DataSeries_Funcs.TotalPointCount() + " total points in graph)");
        }

        [ClojureStaticMethod("set-data-points", "Sets the points of the current series to the given points")]
        public static void SetDataPoints(List<GraphPoint> dataPoints)
        {
            if (dataPoints == null)
                return;

            DataSeries currentSeries = DataSeries_Funcs.GetCurrentDataSeries();
            int added = currentSeries.SetDataPoints(dataPoints);
            GLGraph.self.UpdateGraphAxes();
            GLGraph.self.UpdateMatrices(true);

            ClojureEngine.Log("Set " + added + " points to series '" + currentSeries.Name + "' (" + DataSeries_Funcs.TotalPointCount() + " total points in graph)");
        }

        [ClojureStaticMethod("add-data-points", "Adds the given points to the current series")]
        public static void AddDataPoints(IList yVals)
        {
            AddDataPoints(MakeDataPoints(yVals));
        }

        [ClojureStaticMethod("set-data-points", "Sets the points of the current series to the given points")]
        public static void SetDataPoints(IList yVals)
        {
            SetDataPoints(MakeDataPoints(yVals));
        }

        [ClojureStaticMethod("add-data-points", "Adds the given points to the current series")]
        public static void AddDataPoints(IList yVals, double xIncrement)
        {
            AddDataPoints(MakeDataPoints(yVals, xIncrement));
        }

        [ClojureStaticMethod("set-data-points", "Sets the points of the current series to the given points")]
        public static void SetDataPoints(IList yVals, double xIncrement)
        {
            SetDataPoints(MakeDataPoints(yVals, xIncrement));
        }

        [ClojureStaticMethod("add-data-points", "Adds the given points to the current series")]
        public static void AddDataPoints(IList yVals, double xStart, double xIncrement)
        {
            AddDataPoints(MakeDataPoints(yVals, xStart, xIncrement));
        }

        [ClojureStaticMethod("set-data-points", "Sets the points of the current series to the given points")]
        public static void SetDataPoints(IList yVals, double xStart, double xIncrement)
        {
            SetDataPoints(MakeDataPoints(yVals, xStart, xIncrement));
        }

        [ClojureStaticMethod("add-data-points", "Adds the given points to the current series")]
        public static void AddDataPoints(IList xVals, IList yVals)
        {
            AddDataPoints(MakeDataPoints(xVals, yVals));
        }

        [ClojureStaticMethod("set-data-points", "Sets the points of the current series to the given points")]
        public static void SetDataPoints(IList xVals, IList yVals)
        {
            SetDataPoints(MakeDataPoints(xVals, yVals));
        }

        [ClojureStaticMethod("add-data-points-3d", "Adds the given 3D points to the current series")]
        public static void AddDataPoints3D(IList yVals, IList zVals)
        {
            AddDataPoints3D(yVals, zVals, 1);
        }

        [ClojureStaticMethod("set-data-points-3d", "Sets the points of the current series to the given 3D points")]
        public static void SetDataPoints3D(IList yVals, IList zVals)
        {
            SetDataPoints3D(yVals, zVals, 1);
        }

        [ClojureStaticMethod("add-data-points-3d", "Adds the given 3D points to the current series")]
        public static void AddDataPoints3D(IList yVals, IList zVals, double xIncrement)
        {
            AddDataPoints3D(yVals, zVals, 0, xIncrement);
        }

        [ClojureStaticMethod("set-data-points-3d", "Sets the points of the current series to the given 3D points")]
        public static void SetDataPoints3D(IList yVals, IList zVals, double xIncrement)
        {
            SetDataPoints3D(yVals, zVals, 0, xIncrement);
        }

        [ClojureStaticMethod("add-data-points-3d", "Adds the given 3D points to the current series")]
        public static void AddDataPoints3D(IList yVals, IList zVals, double xStart, double xIncrement)
        {
            double xVal = xStart;
            List<double> xVals = new List<double>();
            int count = 0;

            foreach (object obj in yVals)
            {
                xVals.Add(xVal);
                xVal += xIncrement;

                ++count;
                if (count > ClojureEngine.ClojureMaxIterations)
                {
                    ClojureEngine.Log("Supplied x values collection exceeded '" + ClojureEngine.ClojureMaxIterations + "' values. Stopping further point additon.");
                    break;
                }
            }

            AddDataPoints3D(xVals, yVals, zVals);
        }

        [ClojureStaticMethod("set-data-points-3d", "Sets the points of the current series to the given 3D points")]
        public static void SetDataPoints3D(IList yVals, IList zVals, double xStart, double xIncrement)
        {
            DataSeries_Funcs.ClearSeriesPoints();
            AddDataPoints3D(yVals, zVals, xStart, xIncrement);
        }

        [ClojureStaticMethod("add-data-points-3d", "Adds the given 3D points to the current series")]
        public static void AddDataPoints3D(IList xVals, IList yVals, IList zVals)
        {
            AddDataPoints(MakeDataPoints3D(xVals, yVals, zVals));
        }

        [ClojureStaticMethod("set-data-points-3d", "Sets the points of the current series to the given 3D points")]
        public static void SetDataPoints3D(IList xVals, IList yVals, IList zVals)
        {
            SetDataPoints(MakeDataPoints3D(xVals, yVals, zVals));
        }
        #endregion

        [ClojureStaticMethod("set-projection", "Sets the projection mode of the graph")]
        public static void SetProjectionType(ProjectionType newProjectionType)
        {
            GLGraph.self.SetProjectionType(newProjectionType);
        }

        [ClojureStaticMethod("axis-display", "Sets whether the axes are displayed or not")]
        public static void SetAxisDisplay(bool showAxis)
        {
            GLGraph.self.SetAxisDisplay(showAxis);
        }

        [ClojureStaticMethod("axis-draw-in-color", "Sets whether the axes display in color or not")]
        public static void SetAxisColor(bool showAxesInColor)
        {
            GLGraph.self.SetAxisColor(showAxesInColor);
        }

        [ClojureStaticMethod("set-background-color", "Sets the background color of the graph")]
        public static void SetBackgroundColor(int r, int g, int b)
        {
            GL.ClearColor(Color.FromArgb(r, g, b));
        }

        [ClojureStaticMethod("set-background-color", "Sets the background color of the graph")]
        public static void SetBackgroundColor(Color newCol)
        {
            GL.ClearColor(newCol);
        }
    }
}
