using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    enum CameraBehaviourType { FollowTarget, FollowPoint, MoveToPoint, LAST}
    abstract class CameraBehaviour
    {
        protected Camera camera;
        protected Vector2 pointToFollow;
        protected float blendFactor;
        public CameraBehaviour(Camera cam)
        {
            camera = cam;
        }
        public virtual void Update()
        {
            camera.position = Vector2.Lerp(camera.position, pointToFollow, blendFactor);
        }
    }
}
