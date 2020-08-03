﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Computational_Geometry
{
    public static class ConvexHull
    {
        #region Helper methods
       
        public static List<Point> LexiSort(List<Point> points)
        {
            points.Sort((a, b) =>
            {
                if (a.x < b.x || a.x.Equals(b.x) && a.y < b.y) return -1;
                return 1;
            });
            return points;
        }

        #endregion
        
        public static List<Point> SlowConvexHull(List<Point> points)
        {
            List<Point> hull = new List<Point>();
            List<Edge> edges = new List<Edge>();
            List<Edge> edgeHull = new List<Edge>();

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j) continue;
                    edges.Add(new Edge(points[i], points[j]));
                }
            }

            for (int i = 0; i < edges.Count; i++)
            {
                Edge e = edges[i];
                bool isValid = true;
                for (int j = 0; j < points.Count; j++)
                {
                    Point r = points[j];
                    if (r == e.start || r == e.End) continue;
                    int orientation = MathUtility.Orientation(e.start, e.End, r);
                    if (orientation != -1)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    edgeHull.Add(e);
                }
            }
            hull.Add(edgeHull[0].start);
            for (int i = 0; i < edgeHull.Count; i++)
            {
                if(!hull.Contains(edgeHull[i].End))
                    hull.Add(edgeHull[i].End);
            }
            return hull;
        }

        public static List<Point> DivideAndConquerConvexHull(List<Point> points)
        {
            
            if(points.Count < 3) return new List<Point>();
            points = LexiSort(points);
            var upper = UpperConvexHull(points);
            upper.AddRange(LowerConvexHull(points));
            return upper;
            return LowerConvexHull(points);
        }

        private static List<Point> UpperConvexHull(List<Point> points)
        {
            List<Point> upper = new List<Point> {points[0], points[1]};

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

        private static List<Point> LowerConvexHull(List<Point> points)
        {
            List<Point> lower = new List<Point> {points[points.Count -1 ], points[points.Count - 2]};

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

        public static List<Point> ConvexHullPoints()
        {
            return new List<Point>()
            {
                new Point(1.1f, 1.2f), new Point(-2, 4), new Point(-3, 12), new Point(4, -16), new Point(12,8)
            };
        }
    }
}