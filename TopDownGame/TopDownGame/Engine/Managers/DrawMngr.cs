using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace TopDownGame
{
    enum DrawLayer { Background, Middleground, Playground, Foreground, GUI, LAST }
    static class DrawMngr
    {
        private static List<I_Drawable>[] items;
        private static RenderTexture sceneTexture;
        private static Sprite scene;
        private static Dictionary<string, PostProcessingEffect> postFX;

        static DrawMngr()
        {
            items = new List<I_Drawable>[(int)DrawLayer.LAST];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<I_Drawable>();
            }
            sceneTexture = new RenderTexture(Game.Window.Width, Game.Window.Height);
            scene = new Sprite(Game.Window.OrthoWidth, Game.Window.OrthoHeight);
            scene.Camera = CameraMgr.GetCamera("GUI");
            postFX = new Dictionary<string, PostProcessingEffect>();
        }

        public static void AddItem(I_Drawable item)
        {
            items[(int)item.Layer].Add(item);
        }
        public static void AddFX(string fxName, PostProcessingEffect fx)
        {
            postFX.Add(fxName, fx);
        }
        public static void RemoveFX(string fxName)
        {
            postFX.Remove(fxName);
        }
        public static void RemoveItem(I_Drawable item)
        {
            items[(int)item.Layer].Remove(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }
        }
        public static void ApplyFX()
        {
            foreach (var item in postFX)
            {
                sceneTexture.ApplyPostProcessingEffect(item.Value);
            }
        }
        public static void Draw()
        {
            Game.Window.RenderTo(sceneTexture);
            for (int i = 0; i < items.Length; i++)
            {
                if ((DrawLayer)i == DrawLayer.GUI)
                {
                    ApplyFX();
                    Game.Window.RenderTo(null);
                    scene.DrawTexture(sceneTexture);
                }
                for (int j = 0; j < items[i].Count; j++)
                {
                    items[i][j].Draw();
                }
            }
        }
    }
}
