using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using Computational_Geometry.Graph_DataStructures;

namespace Computational_Geometry.Art_Gallery_Problem
{
    public static class Monotone
    {
        public static int Orientation(Vector p, Vector q, Vector r)
        {
            float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
            return (val >= 0) ? -1 : 1;
        }
        public static DCEL MakeMonotone(DCEL polygon)
        {
            SortedSet<Vertex> Q = new SortedSet<Vertex>(new VertexComparer());
            Dictionary<Edge, Vertex> helper = new Dictionary<Edge, Vertex>();
            Q.UnionWith(polygon.Vertices);
            SortedSet<Edge> T = new SortedSet<Edge>(new EdgeComparer());
            while (Q.Count > 0)
            {
                
                Vertex v = Q.First();
                Q.Remove(v);
                
                switch(v.Type)
                {
                    case Vertex.Vtype.Start:
                        T.Add(v.IncidentEdge);
                        helper[v.IncidentEdge] = v;
                        break;
                    case Vertex.Vtype.End:
                        if (helper.ContainsKey(v.IncidentEdge.Prev) && helper[v.IncidentEdge.Prev].Type == Vertex.Vtype.Merge)
                        {

                            polygon.AddDiagonal(v, helper[v.IncidentEdge.Prev]);
                        }

                        T.Remove(v.IncidentEdge.Prev);
                        break;
                    case Vertex.Vtype.Regular:

                        if (v.IncidentEdge.Next.Origin.Coordinate.y < v.IncidentEdge.Origin.Coordinate.y)
                        {
                            if (helper.ContainsKey(v.IncidentEdge.Prev) &&
                                helper[v.IncidentEdge.Prev].Type == Vertex.Vtype.Merge)
                            {
                                polygon.AddDiagonal(v, helper[v.IncidentEdge.Prev]);
                                T.Remove(v.IncidentEdge.Prev);
                                T.Add(v.IncidentEdge);
                                helper[v.IncidentEdge] = v;
                            }

                            else
                            {
                                if (helper[T.First()].Type == Vertex.Vtype.Merge)
                                {
                                    polygon.AddDiagonal(v, helper[T.First()]);
                                }

                                helper[T.First()] = v;
                            }
                        }

                        break;
                    case Vertex.Vtype.Split:
                        polygon.AddDiagonal(v, helper[T.First()]);

                        helper[T.First()] = v;
                        helper[v.IncidentEdge] = v;
                        T.Add(v.IncidentEdge);
                        
                        break;
                    case Vertex.Vtype.Merge:
                        if (helper.ContainsKey(v.IncidentEdge.Prev) && helper[v.IncidentEdge.Prev].Type == Vertex.Vtype.Merge)
                        {
                            polygon.AddDiagonal(v, helper[v.IncidentEdge.Prev]);
                        }

                        T.Remove(v.IncidentEdge.Prev);
                        if (helper[T.First()].Type == Vertex.Vtype.Merge)
                        {
                            polygon.AddDiagonal(v, helper[T.First()]);
                        }

                        helper[T.First()] = v;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // for (int i = 0; i < v1.Count; i++)
            // {
            //     polygon.AddDiagonal(v1[i], v2[i]);
            // }
            return polygon;
        }
        
        public static float GetXFromPoints(Vector p1, Vector p2, Vector vertex)
        {
            Segment temp = new Segment(p1, p2);
            return temp.SweepXValue(vertex, 0);
        }
        
    }
    


    public class VertexComparer : IComparer<Vertex>
    {
        public int Compare(Vertex x, Vertex y)
        {
            if (x.Coordinate == y.Coordinate) return 0;
            if (x.Coordinate.y > y.Coordinate.y || x.Coordinate.y == y.Coordinate.y && x.Coordinate.x < y.Coordinate.x) return -1;
            return 1;
        }
    }

    public class EdgeComparer : IComparer<Edge>
    {
        public int Compare(Edge x, Edge y)
        {
            if (x.Origin.Coordinate == y.Origin.Coordinate) return 0;
            if (x.Origin.Coordinate.x < y.Origin.Coordinate.x) return -1;
            return 1;
        }
    }
}