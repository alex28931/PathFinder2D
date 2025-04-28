using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace TopDownGame
{
    abstract class Actor : GameObject
    {
        // Variables

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, textOffsetX:textOffsetX, textOffsetY:textOffsetY, spriteWidth: spriteWidth, spriteHeight: spriteHeight)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
        }


        public override void Update()
        {

        }
    }
}
