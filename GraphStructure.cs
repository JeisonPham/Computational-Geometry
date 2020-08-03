﻿using System;
 using System.Runtime.CompilerServices;
 using System.Security.Cryptography.X509Certificates;

namespace Computational_Geometry
{
    public class Edge
    {
        public Point start;
        public Point End;

        public Edge(Point start, Point end)
        {
            this.start = start;
            this.End = end;
        }
    }

    public class Point : IComparable<Point>
    {
        protected bool Equals(Point other)
        {
            if (other == null) return false;
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public int CompareTo(object obj)
        {
            Point temp = (Point) obj;
            return temp.CompareTo(temp);
        }

        public float x;
        public float y;

        public string Label = "";

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point x, Point y)
        {
            return new Point(x.x + y.x, x.y + y.y);
        }

        public static Point operator -(Point x)
        {
            x.x *= -1;
            x.y *= -1;
            return x;
        }

        public static Point operator -(Point x, Point y)
        {
            return new Point(x.x - y.x, x.y - y.y);
        }

        public static bool operator ==(Point x, Point y)
        {
            if (x is null || y is null) return false;
            return x.x.Equals(y.x) && x.y.Equals(y.y);
        }

        public static bool operator !=(Point x, Point y)
        {
            return !(x == y);
        }

        public int CompareTo(Point other)
        {
            if (y > other.y || (y.Equals(other.y) && x < other.x)) return -1;
            if (y < other.y || (y.Equals(other.y) && x > other.x)) return 1;
            return 0;
        }
        

        public override string ToString()
        {
            return (Label == "") ? "(" + x + ", " + y + ")" : Label;
        }

        public float Magnitude()
        {
            return (float) Math.Sqrt(x * x + y * y);
        }
    }

    public class Segment : IComparable<Segment>
    {
        public Point Start;
        public Point End;

        public readonly string ID;
        public readonly int num;
        public int InsertOrder = 0;

        public bool zLen;

        private float a, b, c;

        public float Value;

        public Segment(Point start, Point end, int id = -1)
        {
            if (start.CompareTo(end) > 0)
            {
                Point swap = start;
                start = end;
                end = swap;
            }

            this.zLen = start.CompareTo(end) == 0;
            this.Start = start;
            this.End = end;

            a = this.Start.y - this.End.y; b = this.Start.x - this.End.x;
            c = (this.Start.x - this.End.x) * this.Start.y + (this.Start.y - this.End.y) * this.Start.x;
            if (id > -1)
            {
                this.Start.Label = "a" + id;
                this.End.Label = "b" + id;
                this.ID = "S" + id;
                this.num = id;
            }
        }

        public Segment(float x1, float y1, float x2, float y2, int id = -1)
        {
            Point p1 = new Point(x1, y1), p2 = new Point(x2, y2);
            if (p1.CompareTo(p2) > 0)
            {
                Point swap = p1;
                p1 = p2;
                p2 = swap;
            }

            zLen = p1.CompareTo(p2) == 0;
            this.Start = p1;
            this.End = p2;
            a = this.Start.y - this.End.y; b = this.Start.x - this.End.x;
            c = (this.Start.x - this.End.x) * this.Start.y + (this.Start.y - this.End.y) * this.Start.x;
            if (id > -1)
            {
                this.Start.Label = "a" + id;
                this.End.Label = "b" + id;
                this.ID = "S" + id;
                this.num = id;
            }
        }

        public float Distance(Point p)
        {
            Point p1 = End - Start, p2 = p - Start;
            return MathUtility.Cross2D(p1, p2) / p1.Magnitude();
        }

        public bool IsIntersecting(Segment s, bool includeEndpoints = true)
        {
            Point v1 = this.End - this.Start, v2 = s.End - s.Start;
            float rxs = MathUtility.Cross2D(v1, v2);
            if (rxs.Equals(0)) return false;
            float u = MathUtility.Cross2D(this.Start - s.Start, v1) / -rxs;
            float t = MathUtility.Cross2D(s.Start - this.Start, v2) / rxs;
            if(includeEndpoints)
                return u >= 0 && u <= 1 && t >= 0 && t <= 1;
            else
                return u > 0 && u < 1 && t > 0 && t < 1;
        }

        public float SweepXValue(Point p, float belowLine = 0.005f)
        {
            float y = p.y - belowLine;
            Point direction = End - Start;
            var t = (y - Start.y) / direction.y;
            return t * direction.x + Start.x;
        }

        public Point Intersect(Segment s)
        {
            if (s is null || !IsIntersecting(s)) return null;
            Point v1 = this.End - this.Start, v2 = s.End - s.Start;
            float rxs = MathUtility.Cross2D(v1, v2);
            float t = MathUtility.Cross2D(s.Start - this.Start, v2) / rxs;
            return this.Start + new Point(t * v1.x, t * v1.y);
        }

        public bool IsInSegment(Point p)
        {
            Point direction = End - Start;
            Point result = p - Start;
            if (result.x == 0 && direction.x == 0 || result.y == 0 && direction.y == 0) return true;
            float first = result.x / direction.x;
            float second = result.y / direction.y;
            return Math.Abs(first - second) < 0.00001f;

        }

        public float DistanceP(Point p1, Point p2)
        {
            return (float) Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
        }

        public bool IsPointToRight(Point p) => Orientation(p) == 1;
        public bool IsPointToLeft(Point p) => Orientation(p) == -1;

        public int Orientation(Point p)
        {
            float x = MathUtility.Cross2D(End - Start, p - Start);
            if (x > 0) return -1;
            if (x < 0) return 1;
            return 0;
        }
        

        public static bool operator ==(Segment t, Segment seg)
        {
            if (t is null || seg is null) return false;
            return t.Start == seg.Start && t.End == seg.End;
        }

        public static bool operator !=(Segment t, Segment seg) => !(t == seg);

        public int CompareTo(Segment other)
        {
            if (this.Start == other.Start && this.End == other.End) return 0;
            if (this.Start == this.End) return 1;
            if (this.Start == other.Start)
            {
                if (this.End == other.End) return 0;
                else if (this.End.x < other.End.x || this.End.x.Equals(other.End.x) && this.End.y > other.End.y)
                    return -1;
                return 1;
            }

            return this.Start.CompareTo(other.Start);
        }
        

        protected bool Equals(Segment other)
        {
            return this.Start == other.Start && this.End == other.End;
        }



        public override string ToString() => this.ID;
    }
    
}