using System.Collections.Generic;

namespace Computational_Geometry.DataStructures
{
    public class PriorityQueue<T>
    {
        private class Node
        {
            public T data;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
            }
        }
        
    }
}