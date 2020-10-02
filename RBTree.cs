// using System;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using System.Security.Cryptography;
// using System.Xml.Serialization.Configuration;
// using Computational_Geometry.Graph_DataStructures;
//
// namespace Computational_Geometry
// {
//     public class RedBlack<T> where T : IComparable<T>
//     {
//         #region Node Class
//
//         public class Node
//         {
//             public Node Left;
//             public Node Right;
//             public Node Parent;
//
//             public T Data;
//             public int Color;
//         }
//
//         #endregion
//
//         #region Properties
//
//         protected static readonly int BLACK = 0;
//         protected static readonly int RED = 0;
//
//         protected Node Nil;
//         protected Node Root;
//
//         public Node NIL => Nil;
//         
//         private int count;
//         public int Count => count;
//
//         public RedBlack()
//         {
//             Nil = new Node {Color = 0, Left = null, Right = null};
//             Root = Nil;
//         }
//
//         #endregion
//
//         #region Public Methods
//
//         public void PreOrder()
//         {
//             PreOrderHelper(this.Root);
//             Console.WriteLine();
//         }
//
//         public void InOrder()
//         {
//             InOrderHelper(this.Root);
//             Console.WriteLine();
//         }
//
//         public void PostOrder()
//         {
//             PostOrderHelper(this.Root);
//             Console.WriteLine();
//         }
//
//         public Node SearchTree(T k) => SearchTreeHelper(this.Root, k);
//
//         public Node Min(Node node)
//         {
//             while (node.Left != Nil)
//                 node = node.Left;
//             return node;
//         }
//
//         public Node Max(Node node)
//         {
//             while (node.Right != Nil)
//                 node = node.Right;
//             return node;
//         }
//
//         public Node Succ(Node x)
//         {
//             if (x.Right != Nil) return Min(x.Right);
//             Node y = x.Parent;
//             while (y != Nil && x == y.Right)
//             {
//                 x = y;
//                 y = y.Parent;
//             }
//
//             return y;
//         }
//
//         public Node Pred(Node x)
//         {
//             if (x.Left != Nil) return Max(x.Left);
//             Node y = x.Parent;
//             while (y != Nil && x == y.Left)
//             {
//                 x = y;
//                 y = y.Parent;
//             }
//
//             return y;
//         }
//         public void Insert(T key)
//         {
//             count++;
//             Node node = new Node()
//             {
//                 Parent = null,
//                 Data = key,
//                 Left = Nil,
//                 Right = Nil,
//                 Color = 1
//             };
//             Node y = null;
//             Node x = this.Root;
//             while (x != Nil)
//             {
//                 y = x;
//                 if (node.Data.CompareTo(x.Data) == -1) x = x.Left;
//                 else x = x.Right;
//             }
//
//             node.Parent = y;
//             if (y is null) this.Root = node;
//             else if (node.Data.CompareTo(y.Data) == -1) y.Left = node;
//             else y.Right = node;
//
//             if (node.Parent is null)
//             {
//                 node.Color = 0;
//                 return;
//             }
//
//             if (node.Parent.Parent is null) return;
//             FixInsert(node);
//         }
//         
//         public void Delete(T key)
//         {
//             DeleteNodeHelper(this.Root, key);
//         }
//         
//         public Node GetRoot() => this.Root;
//         public void PrintTree() => PrintHelper(this.Root, "", true);
//
//         #endregion
//         
//         #region protected Methods
//
//         protected void LeftRotate(Node x)
//         {
//             Node y = x.Right;
//             x.Right = y.Left;
//             if (y.Left != Nil) y.Left.Parent = x;
//             y.Parent = x.Parent;
//
//             if (x.Parent is null) this.Root = y;
//             else if (x == x.Parent.Left) x.Parent.Left = y;
//             else x.Parent.Right = y;
//
//             y.Left = x;
//             x.Parent = y;
//         }
//
//         protected void RightRotate(Node x)
//         {
//             Node y = x.Left;
//             x.Left = y.Right;
//             if (y.Right != Nil) y.Right.Parent = x;
//
//             y.Parent = x.Parent;
//             if (x.Parent is null) this.Root = y;
//             else if (x == x.Parent.Right) x.Parent.Right = y;
//             else x.Parent.Left = y;
//
//             y.Right = x;
//             x.Parent = y;
//         }
//
//         protected void PreOrderHelper(Node node)
//         {
//             if (node != Nil)
//             {
//                 Console.Write(node.Data + " ");
//                 PreOrderHelper(node.Left);
//                 PreOrderHelper(node.Right);
//             }
//         }
//
//         protected void InOrderHelper(Node node)
//         {
//             if (node != Nil)
//             {
//                 InOrderHelper(node.Left);
//                 Console.Write(node.Data + " ");
//                 InOrderHelper(node.Right);
//             }
//         }
//
//         protected void PostOrderHelper(Node node)
//         {
//             if (node != Nil)
//             {
//                 PostOrderHelper(node.Left);
//                 PostOrderHelper(node.Right);
//                 Console.Write(node.Data + " ");
//             }
//         }
//
//         protected Node SearchTreeHelper(Node node, T key)
//         {
//             if (node == Nil && key.Equals(node.Data)) return node;
//             if (key.CompareTo(node.Data) == -1) return SearchTreeHelper(node.Left, key);
//             return SearchTreeHelper(node.Right, key);
//         }
//
//         protected void FixDelete(Node x)
//         {
//             count--;
//             Node s = null;
//             while (x != Root && x.Color == 0)
//             {
//                 if (x == x.Parent.Left)
//                 {
//                     s = x.Parent.Right;
//                     if (s.Color == 1)
//                     {
//                         s.Color = 0;
//                         x.Parent.Color = 1;
//                         LeftRotate(x.Parent);
//                         s = x.Parent.Right;
//                     }
//
//                     if (s.Left.Color == 0 && s.Right.Color == 0)
//                     {
//                         s.Color = 1;
//                         x = x.Parent;
//                     }
//                     else
//                     {
//                         if (s.Right.Color == 0)
//                         {
//                             s.Left.Color = 0;
//                             s.Color = 1;
//                             RightRotate(s);
//                             s = x.Parent.Right;
//                         }
//
//                         s.Color = x.Parent.Color;
//                         x.Parent.Color = 0;
//                         s.Right.Color = 0;
//                         LeftRotate(x.Parent);
//                         x = Root;
//                     }
//                 }
//                 else
//                 {
//                     s = x.Parent.Left;
//                     if (s.Color == 1)
//                     {
//                         s.Color = 0;
//                         x.Parent.Color = 1;
//                         RightRotate(x.Parent);
//                         s = x.Parent.Left;
//                     }
//
//                     if (s.Right.Color == 0 && s.Right.Color == 0)
//                     {
//                         s.Color = 1;
//                         x = x.Parent;
//                     }
//                     else
//                     {
//                         if (s.Left.Color == 0)
//                         {
//                             s.Right.Color = 0;
//                             s.Color = 1;
//                             LeftRotate(s);
//                             s = x.Parent.Left;
//                         }
//
//                         s.Color = x.Parent.Color;
//                         x.Parent.Color = 0;
//                         s.Left.Color = 0;
//                         RightRotate(x.Parent);
//                         x = Root;
//                     }
//                 }
//             }
//
//             x.Color = 0;
//         }
//
//         
//
//         protected void PrintHelper(Node root, string indent, bool last)
//         {
//             if (root != Nil)
//             {
//                 Console.Write(indent);
//                 if (last)
//                 {
//                     Console.Write("R----");
//                     indent += "      ";
//                 }
//                 else
//                 {
//                     Console.Write("L----");
//                     indent += "|     ";
//                 }
//
//                 string sColor = root.Color == 1 ? "RED" : "BLACK";
//                 Console.Write(root.Data + "(" + sColor + ")");
//                 PrintHelper(root.Left, indent, false);
//                 PrintHelper(root.Right, indent, true);
//
//             }
//         }
//
//         protected void RbTransplant(Node u, Node v)
//         {
//             if (u.Parent is null) Root = v;
//             else if (u == u.Parent.Left) u.Parent.Left = v;
//             else u.Parent.Right = v;
//             v.Parent = u.Parent;
//         }
//
//         
//         protected void FixInsert(Node k)
//         {
//             Node u = null;
//             while (k.Parent.Color == 1)
//             {
//                 if(k.Parent is null)
//                     throw new NullReferenceException("k.Parent is null");
//                 if(k.Parent.Parent is null)
//                     throw new NullReferenceException("k.Parent.Parent is null");
//                 if (k.Parent == k.Parent.Parent.Right)
//                 {
//                     u = k.Parent.Parent.Left;
//                     if (u.Color == 1)
//                     {
//                         u.Color = 0;
//                         k.Parent.Color = 0;
//                         k.Parent.Parent.Color = 1;
//                         k = k.Parent.Parent;
//                     }
//                     else
//                     {
//                         if (k == k.Parent.Left)
//                         {
//                             k = k.Parent;
//                             RightRotate(k);
//                         }
//
//                         k.Parent.Color = 0;
//                         k.Parent.Parent.Color = 1;
//                         LeftRotate(k.Parent.Parent);
//                     }
//                 }
//                 else
//                 {
//                     u = k.Parent.Parent.Right;
//                     if (u.Color == 1)
//                     {
//                         u.Color = 0;
//                         k.Parent.Color = 0;
//                         k.Parent.Parent.Color = 1;
//                         k = k.Parent.Parent;
//                     }
//                     else
//                     {
//                         if (k == k.Parent.Right)
//                         {
//                             k = k.Parent;
//                             LeftRotate(k);
//                         }
//
//                         k.Parent.Color = 0;
//                         k.Parent.Parent.Color = 1;
//                         RightRotate(k.Parent.Parent);
//                     }
//                 }
//
//                 if (k == Root) break;
//             }
//
//             Root.Color = 0;
//         }
//
//         
//
//         protected void DeleteNodeHelper(Node node, T key)
//         {
//             Node z = Nil;
//             Node x = null, y = null;
//             while (node != Nil)
//             {
//                 if (node.Data.Equals(key)) z = node;
//                 node = node.Data.CompareTo(key) != 1 ? node.Right : node.Left;
//             }
//
//             if (z == Nil) return;
//             y = z;
//             int yOriginalColor = y.Color;
//             if (y.Left == Nil)
//             {
//                 x = z.Right;
//                 RbTransplant(z, z.Right);
//             }
//             else if (z.Right == Nil)
//             {
//                 x = z.Left;
//                 RbTransplant(z, z.Left);
//             }
//             else
//             {
//                 y = Min(z.Right);
//                 yOriginalColor = y.Color;
//                 x = y.Right;
//                 if (y.Parent == z) x.Parent = y;
//                 else
//                 {
//                     RbTransplant(y, y.Right);
//                     y.Right = z.Right;
//                     y.Right.Parent = y;
//                 }
//
//                 RbTransplant(z, y);
//                 y.Left = z.Left;
//                 y.Left.Parent = y;
//                 y.Color = z.Color;
//             }
//
//             if (yOriginalColor == 0) FixDelete(x);
//         }
//
//         #endregion
//     }
//
//     public class RbSegment : RedBlack<Segment>
//     {
//         public Segment ClosestLeftSegment( Point p, bool includeSegemt = true)
//         {
//             Node current = this.Root;
//             Node previous = null;
//             while (current != Nil)
//             {
//                 if (current.Data.IsInSegment(p) && includeSegemt) return current.Data;
//                 if (current.Data.IsPointToLeft(p))
//                 {
//                     previous = current;
//                     current = current.Right;
//                 }
//                 else current = current.Left;
//             }
//
//             return previous?.Data;
//         }
//
//         public Segment ClosestRightSegment(Point p, bool includeSegment = true)
//         {
//             Node current = this.Root;
//             Node previous = null;
//             while (current != Nil)
//             {
//                 if (current.Data.IsInSegment(p) && includeSegment) return current.Data;
//                 if (current.Data.IsPointToRight(p))
//                 {
//                     previous = current;
//                     current = current.Left;
//                 }
//                 else current = current.Right;
//             }
//
//             return previous?.Data;
//         }
//
//         public Segment ClosestRightSegment(Segment s)
//         {
//             Node current = this.Root;
//             Node previous = null;
//             while (current != Nil)
//             {
//                 if (current.Data.CompareTo(s) == 1)
//                 {
//                     previous = current;
//                     current = current.Left;
//                 }
//                 else current = current.Right;
//             }
//
//             return previous?.Data;
//         }
//         
//         public Segment ClosestLeftSegment(Segment s)
//         {
//             Node current = this.Root;
//             Node previous = null;
//             while (current != Nil)
//             {
//                 if (current.Data.CompareTo(s) == -1)
//                 {
//                     previous = current;
//                     current = current.Right;
//                 }
//                 else current = current.Left;
//             }
//
//             return previous?.Data;
//         }
//
//         public List<Segment> InOrderList()
//         {
//             List<Segment> list = new List<Segment>();
//             InOrderListHelper(this.Root, ref list);
//             return list;
//         }
//
//         private void InOrderListHelper(Node node, ref List<Segment> segments)
//         {
//             if (node != Nil)
//             {
//                 InOrderListHelper(node.Left, ref segments);
//                 segments.Add(node.Data);
//                 InOrderListHelper(node.Right, ref segments);
//             }
//         }
//
//         // public new void Insert(Segment key)
//         // {
//         //     base.Insert(key);
//         //     base.Insert(key);
//         // }
//
//         // public new void InOrder()
//         // {
//         //     InOrderHelp(this.Root);
//         //     Console.WriteLine();
//         // }
//         //
//         // private void InOrderHelp(Node node)
//         // {
//         //     if (node != Nil)
//         //     {
//         //         InOrderHelp(node.Left);
//         //         if(node.Left == Nil && node.Right == Nil)
//         //             Console.Write(node.Data + " ");
//         //         InOrderHelp(node.Right);
//         //     }
//         // }
//     }
// }