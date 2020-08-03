using System;
using System.Collections.Generic;

namespace Computational_Geometry.LineSegmentation
{
    // public class Event
    // {
    //     public List<Segment> Upper = new List<Segment>();
    //     public List<Segment> Lower = new List<Segment>();
    //     public List<Segment> Contain = new List<Segment>();
    //
    //     public int LUCSize()
    //     {
    //         return Upper.Count + Lower.Count + Contain.Count;
    //     }
    //
    //     public Event(Segment s, int type = -1)
    //     {
    //         if (type == 0) Upper.Add(s);
    //         else if (type == 1) Lower.Add(s);
    //         else if (type == 2) Contain.Add(s);
    //     }
    //
    //     public Event()
    //     {
    //         
    //     }
    //
    //     public int UCSize()
    //     {
    //         return Upper.Count + Contain.Count;
    //     }
    //
    //     public List<Segment> LC()
    //     {
    //         var temp = new List<Segment>();
    //         temp.AddRange(Lower);
    //         temp.AddRange(Contain);
    //         return temp;
    //     }
    //
    //     public List<Segment> UC()
    //     {
    //         var list = new List<Segment>();
    //         list.AddRange(Upper);
    //         list.AddRange(Contain);
    //
    //         return list;
    //     }
    // }

    // public class Event
    // {
    //     public Point Pt;
    //     public Segment S;
    //     public int type;
    //
    //     public Event(Point p, Segment s, int type = -1)
    //     {
    //         this.Pt = p;
    //         this.S = s;
    //         this.type = type;
    //     }
    // }

    public class Event
    {
        public List<Segment> segments = new List<Segment>();
    
        public Event(Segment s)
        {
            this.segments.Add(s);
        }
    
        public Event()
        {
            
        }
    }

    public class EC : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x == y) return 0;
            if (x.y > y.y || x.y == y.y && x.x < y.x) return -1;
            return 1;
        }
    }

    public class SegComp : IComparer<Segment>
    {
        public Point P { get; set; }
        public int Compare(Segment x, Segment y)
        {
            if (x.Start == y.Start && x.End == y.End) return 0;
            if (x.SweepXValue(P) < y.SweepXValue(P)) return -1;
            return 1;
        }
    }

    // public class EventComp : IComparer<Event>
    // {
    //
    //     public int Compare(Event x, Event y)
    //     {
    //         Point p1 = x.Pt, p2 = y.Pt;
    //         if (p1.y > p2.y || p1.y == p2.y && p1.x < p2.x) return -1;
    //         return 1;
    //     }
    // }
}