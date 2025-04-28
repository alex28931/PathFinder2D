using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TopDownGame
{
    class Node
    {
        public int X { get; }
        public int Y { get; }
        public Vector2 Position { get { return new Vector2(X + 0.5f, Y + 0.5f); } }
        public int Cost { get; }
        public List<Node> Neighbours { get; }
        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;

            Neighbours = new List<Node>();
        }
        public void AddNeigbour(Node n)
        {
            Neighbours.Add(n);
        }
        public void RemoveNeigbour(Node n)
        {
            Neighbours.Remove(n);
        }
    }
}
