using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TopDownGame
{
    class GuiItem : GameObject
    {
        public bool IsSelected;
        public GuiItem(string textureName,Vector2 position, DrawLayer drawLayer = DrawLayer.GUI) : base(textureName, layer: drawLayer)
        {
            Position = position;
            IsActive = true;
            IsSelected = false;
            sprite.Camera = CameraMgr.GetCamera("GUI");
            DrawMngr.AddItem(this);
        }
        public void SetColor(Vector4 color)
        {
            sprite.SetMultiplyTint(color);
        }
    }
}
