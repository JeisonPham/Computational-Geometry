using System;
using System.Collections.Generic;
using Computational_Geometry.Graph_DataStructures;

namespace Computational_Geometry
{
    public static class ConvexHullTester
    {
        public static List<Vector> GenerateRandomPoints(int size = 10)
        {
            List<Vector> temp = new List<Vector>();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                temp.Add(new Vector((float)(rnd.NextDouble() * 10), (float)(rnd.NextDouble() * 10)));
            }

            return temp;
        }
    }
}