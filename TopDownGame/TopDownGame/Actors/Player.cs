using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    class Player : Actor
    {
        private AudioClip hurt;
        bool clickedL = false;
        Vector2 mousePos;
        protected Animation walkAnimation;
        public ObjectsGui objectsGui { get; protected set; }
        public Agent Agent { get; protected set; }
        public bool hasTheGem;
        public Player() : base("playerU", 0, 0, 1, 1)
        {
            hurt = GfxMngr.GetClip("Hurt");
            IsActive = true;
            maxSpeed = 6f;
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType( RigidBodyType.Door);
            mousePos = Position;
            Agent = new Agent(this);
            walkAnimation = new Animation(16, 16, 8, 4);
            Vector2 objectsGuiPosition = new Vector2(0.2f, 0.5f);
            objectsGui = new ObjectsGui("objectsGUI", objectsGuiPosition, this);


            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public void Input()
        {
            if (Game.Window.MouseLeft)
            {
                if (!clickedL)
                {
                    clickedL = true;
                    mousePos = Game.MouseRelativePosition;
                    Vector2 dist = mousePos - Position;
                    List<Node> path = ((Playscene)Game.CurrentScene).Map.MapNodes.GetPath(Agent.X, Agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    Agent.SetPath(path);
                    if (((Playscene)Game.CurrentScene).Id == 4 && path.Count >= (!objectsGui.ObjectItems.ContainsKey(ObjectItemType.Lamp) ? 3 : int.MaxValue)) 
                    {
                        Agent.SetTarget(null);
                    }
                    if (!Agent.Move)
                    {
                        if (Math.Abs(dist.X) > Math.Abs(dist.Y))
                        {
                            if (dist.X > 0)
                            {
                                Forward = new Vector2(1, 0);
                            }
                            else
                            {
                                Forward = new Vector2(-1, 0);
                            }
                        }
                        else
                        {
                            if (dist.Y > 0)
                            {
                                Forward = new Vector2(0, 1);
                            }
                            else
                            {
                                Forward = new Vector2(0, -1);
                            }
                        }
                        SetImageDirection();
                    }
                }
            }
            else
            {
                clickedL = false;
            }
        }
        public override void Update()
        {
            if (IsActive)
            {
                Agent.Update(maxSpeed);
                if (Agent.Move)
                {
                    walkAnimation.Update();
                    SetImageDirection();
                }
                else
                {
                    walkAnimation.Restart();
                }
            }
        }
        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)walkAnimation.Offset.X, (int)walkAnimation.Offset.Y, 16, 16);
            }
        }
        public virtual void SetImageDirection()
        {
            if (Forward.X > 0.0f)
            {
                texture = GfxMngr.GetTexture("playerR");
                sprite.FlipX = false;
            }
            if (Forward.X < 0.0f)
            {
                texture = GfxMngr.GetTexture("playerR");
                sprite.FlipX = true;
            }
            if (Forward.Y > 0.0f)
            {
                texture = GfxMngr.GetTexture("playerD");
                sprite.FlipX = false;
            }
            if (Forward.Y < 0)
            {
                texture = GfxMngr.GetTexture("playerU");
                sprite.FlipX = false;
            }
        }
        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider.RigidBody.Type == RigidBodyType.Door)
            {
                Vector2 dist = collisionInfo.Collider.Position - Position;
                if (((Door)collisionInfo.Collider).IsBlocked && dist.Length < 1) 
                {
                    if (!objectsGui.ObjectItems.ContainsKey(ObjectItemType.Key)) 
                    {
                        soundEmitter.Play(hurt);
                        Agent.SetTarget(null);
                        Position = ((Playscene)Game.CurrentScene).Map.MapNodes.GetNode((int)Position.X, (int)Position.Y).Position;
                    }
                    else
                    {
                        ((Door)collisionInfo.Collider).IsBlocked = false;
                    }
                }
                if (dist.Length <= 0.5f)
                {
                    ((Door)collisionInfo.Collider).OpenTheDoor();
                    if (dist.Length < 0.001f)
                    {
                        IsActive = false;
                        Agent.SetTarget(null);
                        CameraMgr.SetTarget(null);
                        Position = ((Door)collisionInfo.Collider).NextScenePosition;
                        Game.CurrentScene.IsPlaying = false;
                        Game.CurrentScene.NextScene = Game.playScenes[((Door)collisionInfo.Collider).NextSceneId];
                    }
                }
                else
                {
                    ((Door)collisionInfo.Collider).CloseTheDoor();
                }
            }
        }
        public override void Load()
        {
            base.Load();
            objectsGui.Load();
        }
        public override void Destroy()
        {
            objectsGui.Destroy();
            base.Destroy();
        }
    }
}
