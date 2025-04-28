using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class TitleScene : Scene
    {
        protected Texture texture;
        protected Sprite sprite;
        protected Sprite text;
        protected Texture textureText;

        protected string textureName;
        protected KeyCode exitKey;

        public TitleScene(string t_Name="title", KeyCode exit = KeyCode.Return)
        {
            CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
            textureName = t_Name;
            exitKey = exit;

        }

        public override void Start()
        {
            texture = GfxMngr.GetTexture(textureName);
            sprite = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            textureText = new Texture("Assets/pressEnter.png");
            text = new Sprite(Game.PixelsToUnits(textureText.Width), Game.PixelsToUnits(textureText.Height));
            text.pivot = new Vector2(text.Width * 0.5f, text.Height * 0.5f);
            text.scale = new Vector2(0.5f);
            text.position = Game.ScreenCenter + new Vector2(0.0f, 3.0f);

            base.Start();
        }

        public override void Input()
        {
            if (Game.Window.GetKey(exitKey))
            {
                IsPlaying = false;
            }
        }

        public override Scene OnExit()
        {
            texture = null;
            sprite = null;

            return base.OnExit();
        }

        public override void Draw()
        {
            sprite.DrawTexture(texture);
            text.DrawTexture(textureText);
        }
    }
}
