using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    class Stone : GameObject
    {
        private bool clickedL;
        private AudioClip land;
        public Stone(Vector2 position) : base("stone", DrawLayer.Middleground, spriteWidth: 0.9f, spriteHeight: 0.9f)
        {
            land = GfxMngr.GetClip("Land");
            Position = position;
            IsActive = true;
            clickedL = false;
            DrawMngr.AddItem(this);
            UpdateMngr.AddItem(this);
            ((Playscene)Game.CurrentScene).Map.MapNodes.RemoveNode((int)position.X, (int)position.Y);
        }
        public override void Update()
        {
            if (IsActive)
            {
                Vector2 distFromPlayer = Game.Player.Position - Position;
                if (distFromPlayer.LengthSquared <= 1)
                {
                    if (Game.Window.MouseLeft)
                    {
                        if (!clickedL)
                        {
                            clickedL = true;
                            Vector2 mousePos = Game.MouseRelativePosition;
                            Vector2 distFromMouse = mousePos - Position;
                            if (distFromMouse.LengthSquared <= 0.05 && Game.Player.objectsGui.ObjectItems.ContainsKey(ObjectItemType.Pickaxe))  
                            {
                                soundEmitter.Play(land);
                                ((Playscene)Game.CurrentScene).Map.MapNodes.AddNode((int)Position.X, (int)Position.Y, 2);
                                IsActive = false;
                            }
                        }
                    }
                    else
                    {
                        clickedL = false;
                    }
                }
            }
        }
    }
}
