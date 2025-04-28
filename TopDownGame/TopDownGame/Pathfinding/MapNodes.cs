using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using System.Xml;

namespace TopDownGame
{
    class MapNodes
    {
        Dictionary<Node, Node> cameFrom;  //parents
        Dictionary<Node, int> costSoFar;  //distances
        PriorityQueue frontier;           //toVisit

        int width, height;
        int[] cells;
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Node[] Nodes { get; }
        public MapNodes(XmlNode layerNode, int w, int h)
        {
            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
            string[] Ids = csvData.Split(',');
            cells = new int[Ids.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = int.Parse(Ids[i]);
            }
            this.width = w;
            this.height = h;
            

            Nodes = new Node[cells.Length];

            // Build Nodes from cells
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] <= 0) { continue; }
                int x = i % width;
                int y = i / width;
                Nodes[i] = new Node(x, y, cells[i]);
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (Nodes[index] == null) { continue; }
                    AddNeighbours(Nodes[index], x, y);
                }
            }
        }
        public void AddNeighbours(Node n, int x, int y)
        {
            //Top
            CheckNeighbour(n, x, y - 1);
            //Bottom
            CheckNeighbour(n, x, y + 1);
            //Left
            CheckNeighbour(n, x - 1, y);
            //Right
            CheckNeighbour(n, x + 1, y);
        }
        public void CheckNeighbour(Node n, int x, int y)
        {
            if (x < 0 || x >= width) { return; }
            if (y < 0 || y >= height) { return; }

            int index = y * width + x;

            Node neighbour = Nodes[index];

            if(neighbour != null)
            {
                n.AddNeigbour(neighbour);
            }
        }
        public void AddNode(int x, int y, int cost = 1)
        {
            if (x < 0 || x >= width) { return; }
            if (y < 0 || y >= height) { return; }
            int index = y * width + x;
            Node node = new Node(x, y, cost);
            Nodes[index] = node;
            AddNeighbours(node, x, y);
            foreach(Node adj in node.Neighbours)
            {
                adj.AddNeigbour(node);
            }
            cells[index] = cost;
        }
        public void RemoveNode(int x, int y)
        {
            if (x < 0 || x >= width) { return; }
            if (y < 0 || y >= height) { return; }
            int index = y * width + x;
            Node node = GetNode(x, y);
            foreach (Node adj in node.Neighbours)
            {
                adj.RemoveNeigbour(node);
            }
            Nodes[index] = null;
            cells[index] = 0;
        }
        public Node GetNode(int x, int y)
        {
            if (x < 0 || x >= width) { return null; }
            if (y < 0 || y >= height) { return null; }
            return Nodes[y * width + x];
        }
        public Node GetRandomNode()
        {
            Node randomNode = null;

            do
            {
                randomNode = Nodes[RandomGenerator.GetRandomInt(0, Nodes.Count())];
            } while (randomNode == null);

            return randomNode;
        }
        public void ToggleNode(int x, int y)
        {
            Node node = GetNode(x, y);
            if (node == null)
            {
                AddNode(x, y);
            }
            else
            {
                RemoveNode(x, y);
            }
        }
        public void AStar(Node start, Node dest)
        {
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();
            frontier = new PriorityQueue();
            cameFrom[start] = start;
            costSoFar[start] = 0;
            frontier.Enqueue(start, 0);
            while (!frontier.IsEmpty)
            {
                Node current = frontier.Dequeue();
                if (current == dest) { return; }
                foreach (Node neighbour in current.Neighbours)
                {
                    int potencialCost = costSoFar[current] + neighbour.Cost;
                    int heuristicCost = potencialCost + Heuristic(neighbour, dest);
                    if (!cameFrom.ContainsKey(neighbour))
                    {
                        frontier.Enqueue(neighbour, heuristicCost);
                        cameFrom[neighbour] = current;
                        costSoFar[neighbour] = potencialCost;
                    }
                    else if (potencialCost < costSoFar[neighbour])
                    {
                        cameFrom[neighbour] = current;
                        costSoFar[neighbour] = potencialCost;
                    }
                }
            }
        }
        public int Heuristic(Node start, Node dest)
        {
            return Math.Abs(start.X - dest.X) + Math.Abs(start.Y - dest.Y);
        }
        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            List<Node> path = new List<Node>();
            Node start = GetNode(startX, startY);
            Node end = GetNode(endX, endY);
            if (start == null || end == null)
            {
                return path;
            }
            AStar(start, end);
            if (!cameFrom.ContainsKey(end))
            {
                return path;
            }
            Node currentNode = end;
            while(currentNode != cameFrom[currentNode])
            {
                path.Add(currentNode);
                currentNode = cameFrom[currentNode];
            }
            path.Reverse();
            return path;
        }
    }
}
