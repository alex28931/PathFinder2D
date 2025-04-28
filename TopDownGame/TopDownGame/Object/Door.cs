using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class Door : GameObject
    {
        public bool IsBlocked;
        public bool IsAnimated { get; protected set; }
        public int NextSceneId { get; protected set; }
        public bool IsGate { get; protected set; }
        public Vector2 NextScenePosition { get; protected set; }
        public Door(Vector2 position, Vector2 nextScenePosition, int nextSceneId, bool isAnimated, bool isGate, bool isBlocked) : base("tileset", spriteWidth: 1, spriteHeight: 1)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Door;
            RigidBody.AddCollisionType(RigidBodyType.Player);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            Position = position;
            IsBlocked = isBlocked;
            IsActive = true;
            IsAnimated = isAnimated;
            IsGate = isGate;
            NextScenePosition =nextScenePosition;
            NextSceneId = nextSceneId;
        }
        public virtual void CloseTheDoor()
        {
            if (IsAnimated)
            {
                if (IsGate)
                {
                    ((Playscene)Game.CurrentScene).Map.Layers[0].ChangeLayerTexture(Position, 254);
                    return;
                }
                ((Playscene)Game.CurrentScene).Map.Layers[0].ChangeLayerTexture(Position, 164);
            }
        }
        public virtual void OpenTheDoor()
        {
            if (IsAnimated)
            {
                if (IsGate)
                {
                    ((Playscene)Game.CurrentScene).Map.Layers[0].ChangeLayerTexture(Position, 238);
                    return;
                }
                 ((Playscene)Game.CurrentScene).Map.Layers[0].ChangeLayerTexture(Position, 182);
            }
        }
        public override void Load()
        {
            CloseTheDoor();
            PhysicsMngr.AddItem(RigidBody);
        }
    }
}
