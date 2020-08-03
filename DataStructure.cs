// using System;
// using System.ComponentModel;
// using System.Dynamic;
// using System.Reflection;
// using System.Security.Cryptography;
// using System.Web;
// using System.Xml;
//
// namespace Computational_Geometry
// {
//     public class SegmentList
//     {
//         public Segment Seg;
//         public SegmentList Next;
//
//         public SegmentList(Segment seg, SegmentList n)
//         {
//             this.Seg = seg;
//             this.Next = n;
//         }
//
//         public static int length(SegmentList run)
//         {
//             int n = 0;
//             while (run != null)
//             {
//                 n++;
//                 run = run.Next;
//             }
//
//             return n;
//         }
//
//         public override string ToString()
//         {
//             string x = "seglist[" + Seg;
//             SegmentList run = Next;
//             while (run != null)
//             {
//                 x = x + ", " + run.Seg;
//                 run = run.Next;
//             }
//
//             return x + "]";
//         }
//     }
//
//     public class QueueEvent
//     {
//         public Point pt;
//         public SegmentList U;
//         public SegmentList L;
//
//         public int n_ins;
//
//         public QueueEvent(Point p)
//         {
//             this.pt = p;
//             this.U = null;
//             this.L = null;
//
//             this.n_ins = 1;
//         }
//
//         private SegmentList AddToList(Segment s, SegmentList l)
//         {
//             return new SegmentList(s, l);
//         }
//
//         public void Add(Segment s)
//         {
//             if (pt.CompareTo(s.Start) == 0)
//                 U = AddToList(s, U);
//             else
//                 L = AddToList(s, L);
//         }
//
//         public override string ToString()
//         {
//             return "evt[" + pt + " /U: " + U + " / L: " + "]";
//         }
//     }
//
//     public class EventTreeNode
//     {
//         public EventTreeNode Left;
//         public EventTreeNode Right;
//         public EventTreeNode Parent;
//
//         public QueueEvent Evt;
//         public int color;
//     }
//
//     public class EventTree
//     {
//         private static readonly int BLACK = 0;
//         private static readonly int RED   = 1;
//
//         private EventTreeNode Nil;
//         private EventTreeNode Root;
//
//         public EventTree()
//         {
//             this.Nil = new EventTreeNode {color = BLACK};
//
//             this.Root = this.Nil;
//         }
//
//         public bool IsEmpty()
//         {
//             return this.Root == this.Nil;
//         }
//
//         public void Insert(Point p)
//         {
//             Insert(p, null);
//         }
//
//         public void Insert(Point p, Segment s)
//         {
//             EventTreeNode y = this.Nil;
//             EventTreeNode r = this.Root;
//
//             int c = 0;
//
//             while (r != this.Nil)
//             {
//                 y = r;
//                 c = r.Evt.pt.CompareTo(p);
//                 if (c > 0) r = r.Left;
//                 else if (c < 0) r = r.Right;
//                 else
//                 {
//                     if (s != null)
//                         r.Evt.Add(s);
//                     r.Evt.n_ins++;
//                     return;
//                 }
//             }
//             EventTreeNode x = new EventTreeNode();
//             x.Left = this.Nil;
//             x.Right = this.Nil;
//
//             x.Evt = new QueueEvent(p);
//             if (s != null)
//                 x.Evt.Add(s);
//             x.Parent = y;
//             x.color = RED;
//
//             if (y == this.Nil)
//                 this.Root = x;
//             else if (c > 0)
//                 y.Left = x;
//             else
//                 y.Right = x;
//
//             while (x != this.Root && x.Parent.color == RED)
//             {
//                 if (x.Parent == x.Parent.Parent.Left)
//                 {
//                     if (y.color == RED)
//                     {
//                         x.Parent.color = BLACK;
//                         y.color = BLACK;
//                         x = x.Parent.Parent;
//                         x.color = RED;
//                     }
//                     else
//                     {
//                         if (x == x.Parent.Right)
//                         {
//                             x = x.Parent;
//                             LeftRotate(x);
//                         }
//
//                         x.Parent.color = BLACK;
//                         x.Parent.Parent.color = RED;
//                         RightRotate(x.Parent.Parent);
//                     }
//                 }
//                 else
//                 {
//                     y = x.Parent.Parent.Left;
//                     if (y.color == RED)
//                     {
//                         x.Parent.color = BLACK;
//                         y.color = BLACK;
//                         x = x.Parent.Parent;
//                         x.color = RED;
//                     }
//                     else
//                     {
//                         if (x == x.Parent.Left)
//                         {
//                             x = x.Parent;
//                             RightRotate(x);
//                         }
//
//                         x.Parent.color = BLACK;
//                         x.Parent.Parent.color = RED;
//                         LeftRotate(x.Parent.Parent);
//                     }
//                 }
//             }
//
//             this.Root.color = BLACK;
//         }
//
//         public QueueEvent Get()
//         {
//             EventTreeNode x = this.Root;
//             if (x == this.Nil)
//             {
//                 return null;
//             }
//
//             while (x.Left != this.Nil)
//                 x = x.Left;
//             return Delete(x);
//         }
//
//         private QueueEvent Delete(EventTreeNode z)
//         {
//             QueueEvent res = z.Evt;
//
//             EventTreeNode y = z.Left == this.Nil || z.Right == this.Nil ? z : TreeSuccessor(z);
//             EventTreeNode x = y.Left != this.Nil ? y.Left : y.Right;
//
//             x.Parent = y.Parent;
//             if (y.Parent == this.Nil)
//                 this.Root = x;
//             else if (y == y.Parent.Left)
//                 y.Parent.Left = x;
//             else
//                 y.Parent.Right = x;
//
//             if (y != z)
//                 z.Evt = y.Evt;
//             if (y.color != BLACK)
//                 return res;
//
//             while (x != this.Root && x.color == BLACK)
//             {
//                 EventTreeNode w;
//                 if (x == x.Parent.Left)
//                 {
//                     w = x.Parent.Right;
//
//                     if (w.color == RED)
//                     {
//                         w.color = BLACK;
//                         x.Parent.color = RED;
//                         LeftRotate(x.Parent);
//                         w = x.Parent.Left;
//                     }
//
//                     if (w.Left.color == BLACK && w.Right.color == BLACK)
//                     {
//                         w.color = RED;
//                         x = x.Parent;
//                     }
//                     else
//                     {
//                         if (w.Right.color == BLACK)
//                         {
//                             w.Left.color = BLACK;
//                             w.color = RED;
//                             RightRotate(w);
//                             w = x.Parent.Right;
//                         }
//
//                         w.color = x.Parent.color;
//                         x.Parent.color = BLACK;
//                         w.Right.color = BLACK;
//                         LeftRotate(x.Parent);
//                         x = this.Root;
//                     }
//                 }
//                 else
//                 {
//                     w = x.Parent.Left;
//                     if (w.color == RED)
//                     {
//                         w.color = BLACK;
//                         x.Parent.color = RED;
//                         RightRotate(x.Parent);
//                         w = x.Parent.Left;
//                     }
//
//                     if (w.Left.color == BLACK && w.Right.color == BLACK)
//                     {
//                         w.color = RED;
//                         x = x.Parent;
//                     }
//                     else
//                     {
//                         if (w.Left.color == BLACK)
//                         {
//                             w.Right.color = BLACK;
//                             w.color = RED;
//                             LeftRotate(w);
//                             w = x.Parent.Left;
//                         }
//
//                         w.color = x.Parent.color;
//                         x.Parent.color = BLACK;
//                         w.Left.color = BLACK;
//                         RightRotate(x.Parent);
//                         x = this.Root;
//                     }
//                 }
//             }
//
//             x.color = BLACK;
//             return res;
//         }
//
//         private EventTreeNode TreeSuccessor(EventTreeNode x)
//         {
//             if (x.Right != this.Nil)
//             {
//                 x = x.Right;
//                 while (x.Left != this.Nil)
//                     x = x.Left;
//                 return x;
//             }
//
//             EventTreeNode y = x.Parent;
//             while (y != this.Nil && x == y.Right)
//             {
//                 x = y;
//                 y = y.Parent;
//             }
//
//             return y;
//         }
//
//         private void RightRotate(EventTreeNode y)
//         {
//             EventTreeNode x = y.Left;
//             y.Left = x.Right;
//
//             if (x.Right != this.Nil)
//                 x.Right.Parent = y;
//             x.Parent = y.Parent;
//
//             if (x.Parent == this.Nil)
//                 this.Root = x;
//             else if (y == y.Parent.Right)
//                 y.Parent.Right = x;
//             else
//                 y.Parent.Left = x;
//             x.Right = y;
//             y.Parent = x;
//         }
//
//         private void LeftRotate(EventTreeNode x)
//         {
//             EventTreeNode y = x.Right;
//             x.Right = y.Left;
//
//             if (y.Left != this.Nil)
//                 y.Left.Parent = x;
//             y.Parent = x.Parent;
//             if (x.Parent == this.Nil)
//                 this.Root = y;
//             else
//             {
//                 if (x == x.Parent.Left)
//                     x.Parent.Left = y;
//                 else x.Parent.Right = y;
//                 y.Left = x;
//                 x.Parent = y;
//             }
//             
//         }
//     }
//
//     public class IntersectionPoint
//     {
//         public Point Pt;
//         private SegmentList S1;
//
//         public IntersectionPoint Next;
//
//         public IntersectionPoint(Point p, IntersectionPoint n)
//         {
//             this.Pt = p;
//             this.Next = n;
//         }
//
//         public void AddSegList(SegmentList s)
//         {
//             while (s != null)
//             {
//                 S1 = new SegmentList(s.Seg, S1);
//                 s = s.Next;
//             }
//         }
//
//         public void AddSegment(Segment s)
//         {
//             S1 = new SegmentList(s, S1);
//         }
//         
//     }
//
//     public class StatusTreeNode
//     {
//         public StatusTreeNode Parent;
//         
//         public StatusTreeNode Left;
//         public StatusTreeNode Right;
//
//         public Segment SegStored;
//
//         public Segment SegMax;
//         public Segment SegMin;
//
//         public int Color;
//
//         public int Id;
//         public static int IdCounter;
//
//         public StatusTreeNode()
//         {
//             this.Id = ++IdCounter;
//         }
//
//         public override string ToString() => "Node " + Id;
//     }
//
//     public class StatusTree
//     {
//         private static readonly int BLACK = 0;
//         private static readonly int RED = 1;
//
//         private StatusTreeNode Root;
//
//         public Point PSweep;
//
//         public StatusTreeNode Succ(StatusTreeNode n)
//         {
//             if (n == null)
//             {
//                 if (this.Root == null) return null;
//                 n = this.Root;
//
//                 while (n.SegStored == null)
//                     n = n.Left;
//                 return n;
//             }
//
//             while (n != this.Root && n == n.Parent.Right)
//                 n = n.Parent;
//             if (n == this.Root) return null;
//             n = n.Parent.Right;
//
//             while (n.Left != null)
//                 n = n.Left;
//             return n;
//         }
//
//         public StatusTreeNode Pred(StatusTreeNode n)
//         {
//             if (n == null)
//             {
//                 if (this.Root == null) return null;
//                 n = this.Root;
//
//                 while (n.SegStored == null)
//                     n = n.Right;
//                 return n;
//             }
//
//             while (n != this.Root && n == n.Parent.Left)
//                 n = n.Parent;
//             if (n == this.Root) return null;
//             n = n.Parent.Left;
//             while (n.Right != null)
//                 n = n.Right;
//             return n;
//         }
//
//         public void DeleteNode(StatusTreeNode n)
//         {
//             if (n == this.Root)
//             {
//                 this.Root = null;
//                 return;
//             }
//
//             StatusTreeNode x = n == n.Parent.Left ? n.Parent.Right : n.Parent.Left;
//             if (n.Parent == this.Root)
//             {
//                 this.Root = x;
//                 this.Root.Color = BLACK;
//                 this.Root.Parent = null;
//
//                 return;
//             }
//
//             StatusTreeNode y = n.Parent;
//             x.Parent = y;
//             if (n.Parent == y.Left)
//                 y.Left = x;
//             else
//                 y.Right = x;
//
//             while (y != null)
//             {
//                 bool changed = false;
//                 if (y.SegMin != y.Left.SegMin)
//                 {
//                     changed = true;
//                     y.SegMin = y.Left.SegMin;
//                 }
//
//                 if (y.SegMin != y.Right.SegMax)
//                 {
//                     changed = true;
//                     y.SegMax = y.Right.SegMax;
//                 }
//
//                 if (!changed) break;
//                 y = y.Parent;
//             }
//
//             y = n.Parent;
//             if (y.Color == BLACK)
//             {
//                 while (x != this.Root && x.Color == BLACK)
//                 {
//                     if (x == x.Parent.Left)
//                     {
//                         StatusTreeNode w = x.Parent.Right;
//                         if (w.Color == RED)
//                         {
//                             w.Color = BLACK;
//                             x.Parent.Color = RED;
//
//                             LeftRotate(x.Parent);
//                             w = x.Parent.Right;
//                         }
//
//                         if (w.Left.Color == BLACK && w.Right.Color == BLACK)
//                         {
//                             w.Color = RED;
//                             x = x.Parent;
//                         }
//                         else
//                         {
//                             if (w.Right.Color == BLACK)
//                             {
//                                 w.Left.Color = BLACK;
//                                 w.Color = RED;
//                                 RightRotate(w);
//                                 w = x.Parent.Right;
//                             }
//
//                             w.Color = x.Parent.Color;
//                             x.Parent.Color = BLACK;
//                             w.Right.Color = BLACK;
//                             LeftRotate(x.Parent);
//                             x = this.Root;
//                         }
//                     }
//                     else
//                     {
//                         StatusTreeNode w = x.Parent.Left;
//
//                         if (w.Color == RED)
//                         {
//                             w.Color = BLACK;
//                             x.Parent.Color = RED;
//
//                             RightRotate(x.Parent);
//                             w = x.Parent.Left;
//                         }
//
//                         if (w.Left.Color == BLACK && w.Right.Color == BLACK)
//                         {
//                             w.Color = RED;
//                             x = x.Parent;
//                         }
//                         else
//                         {
//                             if (w.Left.Color == BLACK)
//                             {
//                                 w.Right.Color = BLACK;
//                                 w.Color = RED;
//                                 LeftRotate(w);
//                                 w = x.Parent.Left;
//                             }
//
//                             w.Color = x.Parent.Color;
//                             x.Parent.Color = BLACK;
//                             w.Left.Color = BLACK;
//                             RightRotate(x.Parent);
//                             x = this.Root;
//                         }
//                     }
//                 }
//
//                 x.Color = BLACK;
//             }
//         }
//
//         public int CmpSegments(Segment s1, Segment s2)
//         {
//             Point p = s1.Intersect(s2);
//             if (p != null)
//             {
//                 if (PSweep.CompareTo(p) < 0)
//                     return -s1.Orientation(s2.Start);
//                 else return -s1.Orientation(s2.End);
//             }
//
//             return -s1.Orientation(PSweep);
//         }
//
//         public void InsertSeg(Segment s)
//         {
//             StatusTreeNode m = new StatusTreeNode();
//
//             m.SegStored = m.SegMax = m.SegMin = s;
//             m.Color = BLACK;
//             if (this.Root == null)
//             {
//                 this.Root = m;
//                 return;
//             }
//
//             StatusTreeNode n = this.Root;
//             while (n.SegStored == null)
//             {
//                 if (CmpSegments(n.Right.SegMin, s) > 0)
//                     n = n.Left;
//                 else n = n.Right;
//             }
//             
//             StatusTreeNode np = new StatusTreeNode();
//             np.Parent = n.Parent;
//             n.Parent = m.Parent = np;
//
//             if (n == this.Root) this.Root = np;
//             else
//             {
//                 if (np.Parent.Left == n)
//                     np.Parent.Left = np;
//                 else np.Parent.Right = np;
//
//                 if (CmpSegments(n.SegStored, s) > 0)
//                 {
//                     np.Left = m;
//                     np.Right = n;
//                 }
//                 else
//                 {
//                     np.Left = n;
//                     np.Right = m;
//                 }
//
//                 np.SegMin = np.Left.SegMin;
//                 np.SegMax = np.Right.SegMax;
//
//                 np = np.Parent;
//                 PropagateUp(np);
//
//                 StatusTreeNode x = m.Parent;
//                 x.Color = RED;
//                 while ((x != this.Root) && (x.Parent.Color == RED))
//                 {
//                     if (x.Parent == x.Parent.Parent.Left)
//                     {
//                         StatusTreeNode y = x.Parent.Parent.Right;
//                         if (y.Color == RED)
//                         {
//                             x.Parent.Color        = BLACK;
//                             y.Color               = BLACK;
//                             x.Parent.Parent.Color = RED;
//
//                             x = x.Parent.Parent;
//                         }
//                         else
//                         {
//                             if (x == x.Parent.Right)
//                             {
//                                 x = x.Parent;
//                                 LeftRotate (x);
//                             }
//                             x.Parent.Color        = BLACK;
//                             x.Parent.Parent.Color = RED;
//                             RightRotate (x.Parent.Parent);
//                         }
//                     }
//                     else
//                     {
//                         StatusTreeNode y = x.Parent.Parent.Left;
//                         if (y.Color == RED)
//                         {
//                             x.Parent.Color        = BLACK;
//                             y.Color               = BLACK;
//                             x.Parent.Parent.Color = RED;
//
//                             x = x.Parent.Parent;
//                         }
//                         else
//                         {
//                             if (x == x.Parent.Left)
//                             {
//                                 x = x.Parent;
//                                 RightRotate (x);
//                             }
//                             x.Parent.Color        = BLACK;
//                             x.Parent.Parent.Color = RED;
//                             LeftRotate (x.Parent.Parent);
//                         }
//                     }
//                 }
//
//                 this.Root.Color = BLACK;
//             }
//         }
//         
//
//         private void PropagateUp(StatusTreeNode np)
//         {
//             while (np != null)
//             {
//                 bool changed = false;
//
//                 if (np.SegMin != np.Left.SegMin)
//                 {
//                     changed = true;
//                     np.SegMin = np.Left.SegMin;
//                 }
//
//                 if (np.SegMax != np.Right.SegMax)
//                 {
//                     changed = true;
//                     np.SegMax = np.Right.SegMax;
//                 }
//
//                 if (! changed)
//                     break;
//
//                 np = np.Parent;
//             }
//         }
//
//         public StatusTreeNode RightNodeForPoint(Point p)
//         {
//             StatusTreeNode n = this.Root;
//             if (n == null) return null;
//             while (n.SegStored == null)
//             {
//                 if (!n.Left.SegMax.IsPointToLeft(p))
//                     n = n.Right;
//                 else n = n.Left;
//             }
//
//             if (n.SegStored.IsPointToLeft(p)) return n;
//             return null;
//         }
//
//         public StatusTreeNode LeftNodeForPoint(Point p)
//         {
//             StatusTreeNode n = this.Root;
//             if (n == null) return null;
//             while (n.SegStored == null)
//             {
//                 if (!n.Right.SegMin.IsPointToRight(p))
//                     n = n.Left;
//                 else n = n.Right;
//             }
//
//             if (n.SegStored.IsPointToRight(p)) return n;
//             return null;
//         }
//         
//
//         private void RightRotate(StatusTreeNode y)
//         {
//             StatusTreeNode x = y.Left;
//
//             y.Left = x.Right;
//             if (x.Right != null) x.Right.Parent = y;
//
//             x.Parent = y.Parent;
//
//             if (x.Parent == null) this.Root = x;
//             else if (y == y.Parent.Right) y.Parent.Right = x;
//             else y.Parent.Left = x;
//
//             x.Right = y;
//             y.Parent = x;
//
//             y.SegMax = y.Right.SegMax;
//             y.SegMin = y.Left.SegMin;
//             x.SegMax = x.Right.SegMax;
//             x.SegMin = x.Left.SegMin;
//         }
//
//         private void LeftRotate(StatusTreeNode x)
//         {
//             StatusTreeNode y = x.Right;
//             x.Right = y.Left;
//
//             if (y.Left != null) y.Left.Parent = x;
//             y.Parent = x.Parent;
//
//             if (x.Parent == null) this.Root = y;
//             else if (x == x.Parent.Left) x.Parent.Left.GetType();
//             else x.Parent.Right = y;
//
//             y.Left = x;
//             x.Parent = y;
//
//             x.SegMax = x.Right.SegMax;
//             x.SegMin = x.Left.SegMin;
//             y.SegMax = y.Right.SegMax;
//             y.SegMin = y.Left.SegMin;
//         }
//
//         private void CheckNodeIntegrity(StatusTreeNode n)
//         {
//             bool bugs = false;
//             if (n.Left == null && n.Right != null || n.Right == null && n.Left != null)
//             {
//                 Console.WriteLine("Node " + n + " has left child null");
//                 bugs = true;
//             }
//
//             if (n.Left != null && n.Left.Parent != n || n.Right != null && n.Right.Parent != n)
//             {
//                 Console.WriteLine("Incorrect child/parent linkage at left of " + n);
//                 bugs = true;
//             }
//
//             if (n.SegStored != null)
//             {
//                 if (n.SegMin != n.SegStored) Console.WriteLine("Min != stored for child " + n);
//                 if (n.SegMax != n.SegStored) Console.WriteLine("Max != stored for child " + n);
//                 if (n.Left != null) Console.WriteLine("Left not null @child " + n);
//                 if (n.Right != null) Console.WriteLine("Right not null @child " + n);
//                 if (n.Color != BLACK) Console.WriteLine("Color integrity violated @child " + n);
//                 return;
//             }
//             else if (n.Color == RED && !bugs)
//             {
//                 if (n.Left.Color == RED)
//                 {
//                     bugs = true;
//                     Console.WriteLine("Color integrity violated at left of " + n);
//                 }
//
//                 if (n.Right.Color == RED)
//                 {
//                     bugs = true;
//                     Console.WriteLine("Color integrity violated at right of " + n);
//                 }
//             }
//
//             if (!bugs)
//             {
//                 CheckNodeIntegrity(n.Left);
//                 CheckNodeIntegrity(n.Right);
//             }
//         }
//
//         public void CheckIntegrity()
//         {
//             if (this.Root != null)
//             {
//                 if (this.Root.Color == BLACK)
//                     CheckNodeIntegrity(this.Root);
//                 else Console.WriteLine("Error: Color Integrity of the tree violated at root.");
//             }
//         }
//     }
// }