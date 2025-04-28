using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
namespace TopDownGame
{
    class FollowPointBehaviour : CameraBehaviour
    {
        public float CameraSpeed = 5;
        public FollowPointBehaviour(Camera cam, Vector2 point) : base(cam)
        {
            pointToFollow = point;
        }
        public virtual void SetPointToFollow(Vector2 point)
        {
            pointToFollow = point;
        }
        public override void Update()
        {
            blendFactor = Game.DeltaTime * CameraSpeed;
            base.Update();
        }
    }
}
