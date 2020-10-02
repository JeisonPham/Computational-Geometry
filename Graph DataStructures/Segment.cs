using System;

namespace Computational_Geometry.Graph_DataStructures
{
    public class Segment
    {
        public Vector Start;
        public Vector End;

        public Segment(Vector x, Vector y)
        {
            if (x.y < y.y)
            {
                this.Start = y;
                this.End = x;
            }
            else
            {
                this.Start = x;
                this.End = y;
            }
        }

        public Segment(float x1, float y1, float x2, float y2)
        {
            Vector x = new Vector(x1, y1);
            Vector y = new Vector(x2, y2);
            if (y1 < y2)
            {
                this.Start = y;
                this.End = x;
            }
            else
            {
                this.Start = x;
                this.Start = y;
            }
        }

        public float DistanceToPoint(Vector v)
        {
            Vector v1 = End - Start, v2 = v - Start;
            return v1.CrossVal(v2)/v1.magnitude;
        }

        public bool IsIntersecting(Segment s, bool includeEndpoints = true)
        {
            Vector v1 = this.End - this.Start, v2 = s.End - s.Start;
            float rxs = v1.CrossVal(v2);
            if (rxs.Equals(0)) return false;
            float u = (this.Start - s.Start).CrossVal(v1) / -rxs;
            float t = (s.Start - this.Start).CrossVal(v2) / rxs;
            if(includeEndpoints)
                return u >= 0 && u <= 1 && t >= 0 && t <= 1;
            else
                return u > 0 && u < 1 && t > 0 && t < 1;
        }

        public float SweepXValue(Vector v, float belowLine = 0.0005f)
        {
            float y = v.y - belowLine;
            Vector direction = End - Start;
            var t = (y - Start.y) / direction.y;
            if (Math.Abs(t) > 1) return float.MaxValue;
            return t * direction.x + Start.x;
        }

        public Vector Intersection(Segment s)
        {
            if (s == null || !IsIntersecting(s)) return null;
            Vector v1 = this.End - this.Start, v2 = s.End - s.Start;
            float rxs = v1.CrossVal(v2);
            float t = (s.Start - this.Start).CrossVal(v2) / rxs;
            return this.Start + new Vector(t * v1.x, t * v1.y);
        }

        public bool IsPointInSegment(Vector v)
        {
            Vector direction = this.End - this.Start;
            Vector result = v - Start;
            if (result.x == 0 && direction.x == 0 || result.y == 0 && direction.y == 0) return true;
            float first = result.x / direction.x;
            float second = result.y / direction.y;
            return Math.Abs(first - second) < 0.00001f;
        }

        public int Orientation(Vector v)
        {
            float x = (End - Start).CrossVal(v - Start);
            if (x > 0) return -1;
            if (x < 0) return 1;
            return 0;
        }
        
        

        public static bool operator ==(Segment t, Segment seg)
        {
            if (t is null && seg is null) return true;
            if (t is null || seg is null) return false;
            return t.Start == seg.Start && t.End == seg.End;
        }

        public static bool operator !=(Segment t, Segment seg)
        {
            return !(t == seg);
        }
    }
}