using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Wintellect.PowerCollections;

namespace Computational_Geometry.LineSegmentation
{
    public static class Ls
    {
        public static List<Point> Sweep(List<Segment> segments, bool debug = false)
        {
        //     OrderedDictionary<Point, Event> Q = new OrderedDictionary<Point, Event>();
        //     foreach (var s in segments)
        //     {
        //         if (Q.ContainsKey(s.Start))
        //         {
        //             Q[s.Start].Upper.Add(s);
        //         }
        //         else
        //         {
        //             Q[s.Start] = new Event(s, 0);
        //         }
        //         
        //         if (Q.ContainsKey(s.End))
        //         {
        //             Q[s.End].Lower.Add(s);
        //         }
        //         else
        //         {
        //             Q[s.End] = new Event(s, 1);
        //         }
        //     }
        //     List<Segment> T = new List<Segment>();
        //     List<Point> intersection = new List<Point>();
        //     while (Q.Count > 0)
        //     {
        //         Point p = Q.Keys.First();
        //         Event e = Q[p];
        //         Q.Remove(p);
        //
        //         if (e.LUCSize() > 1)
        //         {
        //             intersection.Add(p);
        //         }
        //
        //         foreach (var s in e.LC())
        //         {
        //             T.Remove(s);
        //         }
        //
        //         foreach (var s in e.UC())
        //         {
        //             T.Add(s);
        //         }
        //
        //         T.Sort((segment, segment1) =>
        //         {
        //             if (segment.SweepXValue(p) < segment1.SweepXValue(p)) return -1;
        //             return 1;
        //         });
        //
        //         if (e.UCSize() == 0)
        //         {
        //             Segment bLeft = NearestLeft(p, T);
        //             Segment bRight = NearestRight(p, T);
        //             if (!(bLeft is null) && !(bRight is null))
        //             {
        //                 Point i = FindNextEvent(bLeft, bRight, p, ref Q);
        //                 if (debug)
        //                 {
        //                     Console.WriteLine("Event: " + p);
        //                     DebugEventQueue(Q);
        //                     DebugSweep(T);
        //                     DebugIntersection(bLeft, bRight, i);
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             var list = e.UC();
        //             list.Sort((segment, segment1) =>
        //             {
        //                 if (segment.SweepXValue(p) < segment1.SweepXValue(p)) return -1;
        //                 return 1;
        //             });
        //
        //             var tTemp = T;
        //
        //             Segment sLeft = list[0];
        //             Segment sRight = list[list.Count - 1];
        //
        //             Segment bLeft = NearestLeft(sLeft, tTemp);
        //             Segment bRight = NearestRight(sRight, tTemp);
        //
        //             Point i = null, j = null;
        //             if (!(bRight is null))
        //             {
        //                 i = FindNextEvent(bRight, sRight, p, ref Q);
        //             }
        //
        //             if (!(bLeft is null))
        //             {
        //                 j = FindNextEvent(bLeft, sLeft, p, ref Q);
        //
        //             }
        //
        //             if (debug)
        //             {
        //                 Console.WriteLine("Event: " + p);
        //                 DebugEventQueue(Q);
        //                 DebugSweep(T);
        //                 DebugIntersection(bRight, sRight, i);
        //                 DebugIntersection(bLeft, sLeft, j);
        //             }
        //         }
        //     }
        //
        //     return intersection;
        //
        // }
            SortedDictionary<Point, Event> Q = new SortedDictionary<Point, Event>(new EC());
            foreach (var segment in segments)
            {
                if (Q.ContainsKey(segment.Start))
                    Q[segment.Start].segments.Add(segment);
                else
                    Q.Add(segment.Start, new Event(segment));
                if(!Q.ContainsKey(segment.End))
                    Q.Add(segment.End, new Event());
            }
            
            // List<Segment> T = new List<Segment>();
            SortedSet<Segment> T = new SortedSet<Segment>();
            List<Point> inter = new List<Point>();
            while (Q.Count > 0)
            {
                Point p = Q.Keys.First();
                Event e = Q[p];
                Q.Remove(p);
                
                List<Segment> lower = new List<Segment>();
                List<Segment> contain = new List<Segment>();
            
                foreach (var segment in T)
                {
                    if (segment.End == p) lower.Add(segment);
                    else if(segment.IsInSegment(p) && segment.Start != p) contain.Add(segment);
                }
            
                if (e.segments.Count + lower.Count + contain.Count > 1)
                    inter.Add(p);
                // lower.AddRange(contain);
                foreach (var segment in lower)
                    T.Remove(segment);
                
            
                contain.AddRange(e.segments);
                foreach (var segment in e.segments)
                    T.Add(segment);
                

                T = FixDistances(p, T);
            
                if (contain.Count == 0)
                {
                    Segment sl = NearestLeft(p, T);
                    Segment sr = NearestRight(p, T);
                    Point i = FindNewEvent(sl, sr, p, ref Q);
                    if (debug)
                    {
                        Console.WriteLine("Event: " + p);
                        DebugEventQueue(Q);
                        DebugSweep(T);
                        DebugIntersection(sl, sr, i);
                    }
                }
                else
                {
                    contain.Sort((segment, segment1) =>
                    {
                        if (segment.SweepXValue(p) < segment1.SweepXValue(p)) return -1;
                        return 1;
                    });
            
                    Segment sP = contain[0];
                    Segment sPP = contain[contain.Count - 1];
            
                    Segment sl = NearestLeft(sP, T);
                    Segment sr = NearestRight(sPP, T);
            
                    Point i = FindNewEvent(sP, sl, p, ref Q);
                    Point j = FindNewEvent(sPP, sr, p, ref Q);
                    
                    if (debug)
                    {
                        Console.WriteLine("Event: " + p);
                        DebugEventQueue(Q);
                        DebugSweep(T);
                        DebugIntersection(sP, sl, i);
                        DebugIntersection(sPP, sr, j);
                    }
                }
            }
            
            return inter;
        }

        private static Point FindNewEvent(Segment sl, Segment sr, Point point, ref SortedDictionary<Point, Event> sortedDictionary)
        {
            if (!(sl is null) && !(sr is null))
            {
                Point intersection = sl.Intersect(sr);
                if (!(intersection is null) &&
                    (intersection.y < point.y || intersection.y == point.y && intersection.x > point.x))
                {
                    intersection.Label = "x" + Math.Min(sl.num, sr.num) + "" + Math.Max(sl.num, sr.num);
                    if(!sortedDictionary.ContainsKey(intersection))
                        sortedDictionary[intersection] = new Event();
                    return intersection;
                }
            }
        
            return null;
        }

        private static SortedSet<Segment> FixDistances(Point p, SortedSet<Segment> T)
        {
            SegComp seg = new SegComp();
            seg.P = p;
            SortedSet<Segment> temp = new SortedSet<Segment>(seg);
            temp.UnionWith(T);
            return temp;
        }

        // private static Point FindNextEvent(Segment bLeft, Segment bRight, Point point, ref OrderedDictionary<Point, Event> orderedDictionary)
        // {
        //     Point intersection = bLeft.Intersect(bRight);
        //     if (!(intersection is null) && (intersection.y < point.y ||
        //         intersection.y == point.y && intersection.x > point.x))
        //     {
        //         intersection.Label = "X" + bLeft.num + "" + bRight.num;
        //         if (!orderedDictionary.ContainsKey(intersection))
        //         {
        //             orderedDictionary[intersection] = new Event();
        //         }
        //         if(intersection != bRight.End) orderedDictionary[intersection].Contain.Add(bRight);
        //         if(intersection != bLeft.End) orderedDictionary[intersection].Contain.Add(bLeft);
        //     }
        //
        //     return intersection;
        // }

        public static Segment NearestLeft(Segment s, List<Segment> T)
        {
            for (int i = 0; i < T.Count; i++)
            {
                if (T[i] == s)
                {
                    if (i == 0) return null;
                    return T[i - 1];
                }
            }

            return null;
        }

        public static Segment NearestLeft(Segment s, SortedSet<Segment> T)
        {
            Segment previous = null;
            foreach (var seg in T)
            {
                if (s == seg) return previous;
                else previous = seg;
            }

            return null;
        }

        public static Segment NearestRight(Segment s, SortedSet<Segment> T)
        {
            Segment previous = null;
            foreach (var seg in T.Reverse())
            {
                if (s == seg) return previous;
                else previous = seg;
            }

            return null;
        }

        public static Segment NearestRight(Segment s, List<Segment> T)
        {
            for (int i = 0; i < T.Count - 1; i++)
            {
                if (T[i] == s) return T[i + 1];
            }

            return null;
        }

        public static Segment NearestLeft(Point p, IEnumerable<Segment> T)
        {
            Segment minDistance = null;
            float min = float.MaxValue;
            foreach (var s in T)
            {
                if (min > Math.Abs(s.Distance(p)) && s.Distance(p) >= 0)
                {
                    min = Math.Abs(s.Distance(p));
                    minDistance = s;
                }
            }

            return minDistance;
        }
        
        

        public static Segment NearestRight(Point p, IEnumerable<Segment> T)
        {
            Segment minDistance = null;
            float min = float.MaxValue;
            foreach (var s in T)
            {
                if (min > Math.Abs(s.Distance(p)) && s.Distance(p) <= 0)
                {
                    min = Math.Abs(s.Distance(p));
                    minDistance = s;
                }
            }

            return minDistance;
        }

        private static void DebugEventQueue(OrderedDictionary<Point, Event> Q)
        {
            Console.WriteLine("Event Queue");
            foreach (var e in Q)
            {
                Console.Write(e.Key + " ");
            }

            Console.WriteLine();
        }
        private static void DebugEventQueue(SortedDictionary<Point, Event> Q)
        {
            Console.WriteLine("Event Queue");
            foreach (var e in Q)
            {
                Console.Write(e.Key + " ");
            }

            Console.WriteLine();
        }
        

        private static void DebugSweep(IEnumerable<Segment> T)
        {
            Console.WriteLine("Sweep");
            foreach (var s in T)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();
        }

        private static void DebugIntersection(Segment bRight, Segment bLeft, Point p)
        {
            Console.WriteLine("Testing Intesection for " + bLeft + " " + bRight + " = " + p);
        }
    }
}