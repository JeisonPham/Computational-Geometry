using System;
using System.Collections.Generic;

namespace Computational_Geometry.Art_Gallery_Problem
{
    public class Edge: IComparable<Edge>
    {
        public Vertex Origin;
        public Edge Twin;
        public Face IncidentFace;
        public Edge Next;
        public Edge Prev;

        public string Label;

        public Vertex GetDestination()
        {
            return Twin.Origin;
        }
        
        public Edge(){}

        public Edge(string label)
        {
            Label = label;
        }

        public override string ToString()
        {
            return Label;
        }

        public int CompareTo(Edge other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var twinComparison = Comparer<Edge>.Default.Compare(Twin, other.Twin);
            if (twinComparison != 0) return twinComparison;
            var nextComparison = Comparer<Edge>.Default.Compare(Next, other.Next);
            if (nextComparison != 0) return nextComparison;
            var prevComparison = Comparer<Edge>.Default.Compare(Prev, other.Prev);
            if (prevComparison != 0) return prevComparison;
            return string.Compare(Label, other.Label, StringComparison.Ordinal);
        }
    }
}