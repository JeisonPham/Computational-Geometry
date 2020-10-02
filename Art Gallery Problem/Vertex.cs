using System;
using System.Collections.Generic;
using Computational_Geometry.Graph_DataStructures;

namespace Computational_Geometry.Art_Gallery_Problem
{
    public class Vertex
    {
        public Vector Coordinate;
        public Edge IncidentEdge;
        public Vtype Type;

        public string Label = "";
        
        public enum Vtype
        {
            Start, // if two neighbors lie below it and interior angle is less than pi
            End, // if two neighbors lie above it and interior angle is less than pi
            Regular,  // one neighbor above or below
            Split, // if two neighbors lie below it and interior angle is greater than pi
            Merge // if two neighbors lie above it and interior angle is greater than pi
        }

        public Vertex(Vector p)
        {
            Coordinate = p;
        }

        public override string ToString()
        {
            return Label == "" ? Coordinate.ToString() : Label;
        }

        public void DetermineType(Vertex first, Vertex second)
        {
            Vector v1 = first.Coordinate - Coordinate; // "Vector 1"
            Vector v2 = second.Coordinate - Coordinate; // "Vector 2"
            var angle = Vector.SignedAngleRad(v1, v2);

            if (first.Coordinate.y < this.Coordinate.y && second.Coordinate.y < this.Coordinate.y) // start or split
            {
                if (angle < Math.PI) this.Type = Vtype.Start;
                if (angle > Math.PI) this.Type = Vtype.Split;
            }
            else if (first.Coordinate.y > this.Coordinate.y && second.Coordinate.y > this.Coordinate.y) // end or merge
            {
                if (angle < Math.PI) this.Type = Vtype.End;
                if (angle > Math.PI) this.Type = Vtype.Merge;
            }
            else if (first.Coordinate.y > this.Coordinate.y && second.Coordinate.y < this.Coordinate.y ||
                     first.Coordinate.y < this.Coordinate.y && second.Coordinate.y > this.Coordinate.y)
            {
                this.Type = Vtype.Regular;
            }
        }
    }
}