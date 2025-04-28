using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class Agent
    {
        
        List<Node> path;
        Node current;
        Node target;

        public bool Move { get { return target != null; } }

        Actor owner;

        public int X { get { return (int)owner.X; } }
        public int Y { get { return (int)owner.Y; } }

        //Debug
        Sprite pathSpr;
        Vector4 pathCol;
        public Agent(Actor owner)
        {
            this.owner = owner;
            target = null;

            pathSpr = new Sprite(0.25f, 0.25f);
            pathSpr.pivot = new Vector2(pathSpr.Width * 0.5f, pathSpr.Height * 0.5f);
            pathCol = new Vector4(0.9f, 0.9f, 0.0f, 1.0f);

        }
        public void SetPath(List<Node> newpPath)
        {
            path = newpPath;

            if(path.Count > 0 && target == null)
            {
                target = path[0];
                path.RemoveAt(0);
            }
            else if (path.Count > 0)
            {
                float dist = Math.Abs(path[0].X - target.X) + Math.Abs(path[0].Y - target.Y);
                if (dist > 1)
                {
                    path.Insert(0, current);
                }
            }
        }
        public void Update(float speed)
        {
            if(target != null)
            {
                Vector2 destination = target.Position;
                Vector2 direction = (destination - owner.Position);
                float distance = direction.Length;

                if (distance < 0.05f)
                {
                    current = target;
                    owner.Position = destination;
                    if (path.Count == 0)
                    {
                        target = null;
                    }
                    else
                    {
                        target = path[0];
                        path.RemoveAt(0);
                    }
                }
                else
                {
                    owner.Position += direction.Normalized() * speed * Game.DeltaTime;
                    owner.Forward = direction;
                }
            }
        }
        public void SetTarget(Node node)
        {
            target = node;
        }
        private void DrawPath()
        {
            if(path != null && path.Count > 0)
            {
                foreach(Node n in path)
                {
                    pathSpr.position = new Vector2(n.X + 0.25f, n.Y + 0.25f);
                    pathSpr.DrawColor(pathCol);
                }
            }
        }
    }
}
