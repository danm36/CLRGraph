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
    public static class DataSeries_AnalysisFuncs
    {
        [ClojureStaticMethod("series-average", "Computes and returns a 3D point representing the average co-ordinate in a given series")]
        public static GraphPoint SeriesAverage()
        {
            return SeriesAverage(DataSeries_Funcs.GetCurrentDataSeries());
        }

        [ClojureStaticMethod("series-average", "Computes and returns a 3D point representing the average co-ordinate in a given series")]
        public static GraphPoint SeriesAverage(DataSeries series)
        {
            Vector3 averageCoord = new Vector3();
            int pointCount = series.GetPointCount();
            PersistentVector pointVec = series.GetDataPoints();

            foreach (object point in pointVec)
            {
                averageCoord.X += ((GraphPoint)point).x;
                averageCoord.Y += ((GraphPoint)point).y;
                averageCoord.Z += ((GraphPoint)point).z;
            }

            averageCoord /= pointCount;
            ClojureEngine.Log("Average co-ord for series " + series.Name + " is X: " + averageCoord.X + ", Y: " + averageCoord.Y + ", Z: " + averageCoord.Z);
            return new GraphPoint(averageCoord);
        }
    }
}
