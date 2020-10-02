﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
 using Computational_Geometry.Graph_DataStructures;

 namespace Computational_Geometry
{
    public static class ConvexHull
    {
        #region Helper methods
       
        public static List<Vector> LexiSort(List<Vector> points)
        {
            points.Sort((a, b) =>
            {
                if (a.x < b.x || a.x.Equals(b.x) && a.y < b.y) return -1;
                return 1;
            });
            return points;
        }

        #endregion
        

        public static List<Vector> DivideAndConquerConvexHull(List<Vector> points)
        {
            
            if(points.Count < 3) return new List<Vector>();
            points = LexiSort(points);
            var upper = UpperConvexHull(points);
            upper.AddRange(LowerConvexHull(points));
            return upper;
            return LowerConvexHull(points);
        }

        private static List<Vector> UpperConvexHull(List<Vector> points)
        {
            List<Vector> upper = new List<Vector> {points[0], points[1]};

            for (int i = 2; i < points.Count; i++)
            {
                upper.Add(points[i]);
                while (upper.Count > 2 &&
                       MathUtility.Orientation(upper[upper.Count - 2], upper[upper.Count - 3], upper[upper.Count - 1]) != 1)
                {
                    upper[upper.Count - 2] = upper[upper.Count - 1];
                    upper.RemoveAt(upper.Count - 1);
                }
            }

            return upper;
        }

        private static List<Vector> LowerConvexHull(List<Vector> points)
        {
            List<Vector> lower = new List<Vector> {points[points.Count -1 ], points[points.Count - 2]};

            for (int i = points.Count - 2; i >= 0; i--)
            {
                lower.Add(points[i]);
                while (lower.Count > 2 &&
                       MathUtility.Orientation(lower[lower.Count - 2], lower[lower.Count - 3], lower[lower.Count - 1]) != 1)
                {
                    lower[lower.Count - 2] = lower[lower.Count - 1];
                    lower.RemoveAt(lower.Count - 1);
                }
            }

            lower.RemoveAt(0);
            lower.RemoveAt(lower.Count - 1);
            return lower;

        }

        public static List<Vector> ConvexHullPoints()
        {
            return new List<Vector>()
            {
                new Vector(1.1f, 1.2f), new Vector(-2, 4), new Vector(-3, 12), new Vector(4, -16), new Vector(12,8)
            };
        }
    }
}