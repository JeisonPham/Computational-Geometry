using System.Collections.Generic;
using System.Linq;
using Computational_Geometry.Graph_DataStructures;

namespace Computational_Geometry.Art_Gallery_Problem
{
    public class DCEL
    {
        public List<Vertex> Vertices = new List<Vertex>();
        public List<Edge> Edges = new List<Edge>();
        public List<Face> Faces = new List<Face>();

        public DCEL() {}

        public DCEL(List<Vector> points)
        {
            if (IsClockwise(points))
                points.Reverse();
            int size = points.Count;
            Face face = new Face();
            Faces.Add(face);

            Edge prevLeftEdge = null;
            Edge prevRightEdge = null;

            for (int i = 0; i < size; i++)
            {
                Vector point = points[i];

                var vertex = new Vertex(point);
                var left = new Edge("e" + (i + 1));
                var right = new Edge("e" + i+"2");

                vertex.Label = "v" + (i + 1);

                left.IncidentFace = face;
                left.Next = null;
                left.Origin = vertex;
                left.Twin = right;
                

                right.IncidentFace = null;
                right.Next = prevRightEdge;
                right.Origin = null;
                right.Twin = left;

                Edges.Add(left);
                Edges.Add(right);
                vertex.IncidentEdge = left;
                Vertices.Add(vertex);

                if (prevLeftEdge != null)
                {
                    prevLeftEdge.Next = left;
                    left.Prev = prevLeftEdge;
                }

                if (prevRightEdge != null)
                {
                    prevRightEdge.Origin = vertex;
                    prevRightEdge.Prev = right;
                }

                prevLeftEdge = left;
                prevRightEdge = right;



            }

            var firstLeftEdge = Edges[0];
            prevLeftEdge.Next = firstLeftEdge;
            firstLeftEdge.Prev = prevLeftEdge;

            var firstRightEdge = Edges[1];
            firstRightEdge.Next = prevRightEdge;
            prevRightEdge.Prev = firstRightEdge;

            prevRightEdge.Origin = Vertices[0];
            face.OuterComponent = firstLeftEdge;

            for (int i = 0; i < Vertices.Count; i++)
            {
                var indexPrev = (i - 1 + Vertices.Count) % Vertices.Count;
                var indexNext = (i + 1) % Vertices.Count;
                Vertices[i].DetermineType(Vertices[indexPrev], Vertices[indexNext]);
            }
        }

        public void AddDiagonal(Vertex upper, Vertex lower)
        {
            Edge left = new Edge();
            Edge right = new Edge();
            
            left.Origin = lower;
            right.Origin = upper;

            left.Twin = right;
            right.Twin = left;

            var prevLowerEdge = lower.IncidentEdge.Prev;
            var prevUpperEdge = upper.IncidentEdge.Prev;

            prevLowerEdge.Next = left;
            left.Prev = prevLowerEdge;
            left.Next = upper.IncidentEdge;
            upper.IncidentEdge.Prev = left;

            prevUpperEdge.Next = right;
            right.Prev = prevUpperEdge;
            right.Next = lower.IncidentEdge;
            lower.IncidentEdge.Prev = right;

            left.IncidentFace = left.Next.IncidentFace;
            left.IncidentFace.OuterComponent = left;
            

            Face rightFace = new Face {OuterComponent = right};

            Edge current = right;
            do
            {
                current.IncidentFace = rightFace;
                current = current.Next;
            } while (current != right);
            Faces.Add(rightFace);
        }

        private bool IsClockwise(List<Vector> points)
        {
            var area = 0f;
            for (int i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                area += points[i].x * points[j].y;
                area -= points[j].x * points[i].y;
            }

            return area < 0;
        }

        // public void AddEdges(int i, int j)
        // {
        //     var vertex1 = Vertices[i];
        //     var vertex2 = Vertices[j];
        //     AddEdges(vertex1, vertex2);
        // }
        //
        // public void AddEdges(Vertex v1, Vertex v2)
        // {
        //     
        // }
        //
        // public void RemoveEdges(int index)
        // {
        //     
        // }
        //
        // public void RemoveEdges(Edge edge)
        // {
        //     
        // }
        //
        //
        // public static Face GetReferenceFace(Vertex v1, Vertex v2)
        // {
        //     return null;
        // }
        //
    }
}