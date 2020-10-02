using System;
using Computational_Geometry.Graph_DataStructures;

namespace Computational_Geometry
{
    public static class MathUtility
    {
        public static int Orientation(Vector vertex, Vector a, Vector b)
        {
            Vector p1 = a - vertex, p2 = b - vertex;
            float area = Cross2D(p1, p2);

            if (area > 0) return 1; // counter clockwise <1,0> -> <1, 1> should return 1
            if (area < 0) return -1; // clockwise 
            return 0;
        }

        public static float Cross2D(Vector a, Vector b) => a.x * b.y - a.y * b.x;
        public static float Dot2D(Vector a, Vector b) =>  a.x * b.x + a.y * b.y;
        public static float SqrDist(Vector a) => a.x * a.x + a.y * a.y; 
        public static float Dist(Vector a)  => (float) Math.Sqrt(SqrDist(a));

    }
}