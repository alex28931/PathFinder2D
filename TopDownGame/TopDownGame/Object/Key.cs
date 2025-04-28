using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    class Key : GameObject
    {
        private bool clickedL;
        private AudioClip pickupSound;
        public Key(Vector2 position) : base("key", DrawLayer.Middleground, spriteWidth: 1f, spriteHeight: 1f)
        {
            pickupSound = GfxMngr.GetClip("Pickup");
            sprite.scale = new Vector2(0.5f);
            sprite.Rotation = MathHelper.DegreesToRadians(30);
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Objects;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player);
            Position = position;
            IsActive = true;
            clickedL = false;
            DrawMngr.AddItem(this);
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if (Game.Window.MouseLeft)
            {
                if (!clickedL)
                {
                    clickedL = true;
                    Vector2 mousePos = Game.MouseRelativePosition;
                    Vector2 dist = mousePos - Position;
                    if (dist.LengthSquared <= 0.03)
                    {
                        soundEmitter.Play(pickupSound);
                        IsActive = false;
                        ((Player)collisionInfo.Collider).objectsGui.AddGuiItem(ObjectItemType.Key);
                    }
                }
            }
            else
            {
                clickedL = false;
            }
        }
        public override void Load()
        {
            DrawMngr.AddItem(this);
            PhysicsMngr.AddItem(RigidBody);
        }
    }
}
