﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TopDownGame
{
    enum RigidBodyType { Player = 1, Door = 2, Objects= 4 }

    class RigidBody
    {
        protected uint collisionMask;

        public GameObject GameObject;   // Owner
        public Collider Collider;
        public bool IsCollisionAffected = true;
        public bool IsActive { get { return GameObject.IsActive; } }

        public Vector2 Velocity;

        public Vector2 Position { get { return GameObject.Position; } }

        public RigidBodyType Type;

        public RigidBody(GameObject owner)
        {
            GameObject = owner;
            PhysicsMngr.AddItem(this);
        }

        public void Update()
        {
            GameObject.Position += Velocity * Game.DeltaTime;
        }
        public bool Collides(RigidBody other, ref Collision collisionInfo)
        {
            return Collider.Collides(other.Collider, ref collisionInfo);
        }

        public void AddCollisionType(RigidBodyType type)
        {
            collisionMask |= (uint)type;//collisionMask = collisionMask | (uint)type
        }

        public void AddCollisionType(uint type)
        {
            collisionMask |= type;//collisionMask = collisionMask | type
        }

        public bool CollisionTypeMatches(RigidBodyType type)
        {
            return ((uint)type & collisionMask) != 0;
        }

        public void Destroy()
        {
            GameObject = null;
            if (Collider != null)
            {
                Collider.RigidBody = null;
                Collider = null;
            }

            PhysicsMngr.RemoveItem(this);
        }
    }
}
