using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class GameOverScene : TitleScene
    {
        protected Texture winTexture;
        protected Sprite winSprite;
        public GameOverScene() : base("gameOver", KeyCode.Y)
        {
            winTexture = new Texture("Assets/Win.png");
            winSprite = new Sprite(Game.PixelsToUnits(winTexture.Width), Game.PixelsToUnits(winTexture.Height));
            winSprite.pivot = new Vector2(winSprite.Width * 0.5f, winSprite.Height * 0.5f);
            winSprite.scale = new Vector2(3.5f);
            winSprite.position = Game.ScreenCenter + new Vector2(0.0f, -4.0f);
        }
        public override void Input()
        {
            if (IsPlaying && Game.Window.GetKey(exitKey))
            {
                IsPlaying = false;
                NextScene = Game.playScenes[0];
            }

            if (IsPlaying && Game.Window.GetKey(KeyCode.N))
            {
                IsPlaying = false;
                NextScene = null;
            }
        }
        public override Scene OnExit()
        {
            Game.Player.Destroy();
            Game.Player = new Player();
            Game.Player.Position = new Vector2(23.5f, 18.5f);
            foreach (Playscene p in Game.playScenes)
            {
                p.Reset();
            }
            return base.OnExit();
        }
        public override void Draw()
        {
            sprite.DrawTexture(texture);
            winSprite.DrawTexture(winTexture);
        }
    }
}
