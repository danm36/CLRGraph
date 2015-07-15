using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRGraph
{
    public class Box
    {
        public float minX = 0, maxX = 0, minY = 0, maxY = 0, minZ = 0, maxZ = 0;

        public Box()
        {

        }

        public Box(float MaxX, float MaxY, float MaxZ)
        {
            maxX = MaxX;
            maxY = MaxY;
            maxZ = MaxZ;
        }

        public Box(float MinX, float MaxX, float MinY, float MaxY, float MinZ, float MaxZ)
        {
            minX = MinX;
            maxX = MaxX;

            minY = MinY;
            maxY = MaxY;

            minZ = MinZ;
            maxZ = MaxZ;
        }

        public void Combine(Box other)
        {
            minX = Math.Min(minX, other.minX);
            minY = Math.Min(minY, other.minY);
            minZ = Math.Min(minZ, other.minZ);

            maxX = Math.Max(maxX, other.maxX);
            maxY = Math.Max(maxY, other.maxY);
            maxZ = Math.Max(maxZ, other.maxZ);
        }

        public void MakeValid()
        {
            float tMinX = minX;
            float tMinY = minY;
            float tMinZ = minZ;
            float tMaxX = maxX;
            float tMaxY = maxY;
            float tMaxZ = maxZ;

            if (float.IsPositiveInfinity(tMinX))
                tMinX = float.MaxValue;
            else if (float.IsNegativeInfinity(tMinX))
                tMinX = -float.MaxValue;
            else if (float.IsNaN(tMinX))
                tMinX = 0;

            if (float.IsPositiveInfinity(tMinY))
                tMinY = float.MaxValue;
            else if (float.IsNegativeInfinity(tMinY))
                tMinY = -float.MaxValue;
            else if (float.IsNaN(tMinY))
                tMinY = 0;

            if (float.IsPositiveInfinity(tMinZ))
                tMinZ = float.MaxValue;
            else if (float.IsNegativeInfinity(tMinZ))
                tMinZ = -float.MaxValue;
            else if (float.IsNaN(tMinZ))
                tMinZ = 0;

            if (float.IsPositiveInfinity(tMaxX))
                tMaxX = float.MaxValue;
            else if (float.IsNegativeInfinity(tMaxX))
                tMaxX = -float.MaxValue;
            else if (float.IsNaN(tMaxX))
                tMaxX = 0;

            if (float.IsPositiveInfinity(tMaxY))
                tMaxY = float.MaxValue;
            else if (float.IsNegativeInfinity(tMaxY))
                tMaxY = -float.MaxValue;
            else if (float.IsNaN(tMaxY))
                tMaxY = 0;

            if (float.IsPositiveInfinity(tMaxZ))
                tMaxZ = float.MaxValue;
            else if (float.IsNegativeInfinity(tMaxZ))
                tMaxZ = -float.MaxValue;
            else if (float.IsNaN(tMaxZ))
                tMaxZ = 0;

            minX = Math.Min(tMinX, tMaxX);
            minY = Math.Min(tMinY, tMaxY);
            minZ = Math.Min(tMinZ, tMaxZ);

            maxX = Math.Max(tMinX, tMaxX);
            maxY = Math.Max(tMinY, tMaxY);
            maxZ = Math.Max(tMinZ, tMaxZ);
        }
    }
}
