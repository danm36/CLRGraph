using clojure.lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    public static class Utility
    {
        public static List<GraphPoint> PVtoGPList(PersistentVector pv)
        {
            List<GraphPoint> points = new List<GraphPoint>();

            int count = 0;
            foreach (object obj in pv)
            {
                if (count > ClojureEngine.ClojureMaxIterations)
                    break;

                try
                {
                    points.Add((GraphPoint)obj);
                    ++count;
                }
                catch { }
            }

            return points;
        }
    }
}
