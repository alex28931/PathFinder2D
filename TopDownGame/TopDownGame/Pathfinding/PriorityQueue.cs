using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGame
{
    class PriorityQueue
    {
        private Dictionary<Node, int> items;
        public bool IsEmpty { get { return items.Count == 0; } }
        public PriorityQueue()
        {
            items = new Dictionary<Node, int>();
        }
        public void Enqueue(Node n, int priority)
        {
            if (!items.ContainsKey(n))
            {
                items[n] = priority;
            }
        }
        public Node Dequeue()
        {
            int lowestPriority = int.MaxValue;
            Node selected = null;
            foreach (Node current in items.Keys)
            {
                int currentPriority = items[current];
                if (currentPriority < lowestPriority)
                {
                    lowestPriority = currentPriority;
                    selected = current;
                }
            }
            items.Remove(selected);
            return selected;
        }
    }
}
