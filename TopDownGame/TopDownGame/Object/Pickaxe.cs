using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    class Pickaxe : GameObject
    {
        private AudioClip pickupSound;
        public Pickaxe(Vector2 position) : base("pickaxe", DrawLayer.Middleground, spriteWidth: 0.7f, spriteHeight: 0.7f)
        {
            
            pickupSound = GfxMngr.GetClip("Pickup");
            sprite.Rotation = MathHelper.DegreesToRadians(-30);
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Objects;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player);
            Position = position;
            IsActive = true;
            DrawMngr.AddItem(this);
        }
        public override void OnCollide(Collision collisionInfo)
        {
            soundEmitter.Play(pickupSound);
            IsActive = false;
            ((Player)collisionInfo.Collider).objectsGui.AddGuiItem(ObjectItemType.Pickaxe);
        }
        public override void Load()
        {
            DrawMngr.AddItem(this);
            PhysicsMngr.AddItem(RigidBody);
        }
    }
}
