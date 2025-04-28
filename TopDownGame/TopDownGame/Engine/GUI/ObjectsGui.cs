using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    enum ObjectItemType { Key, Pickaxe, Lamp }
    class ObjectsGui : GameObject
    {
        
        public Dictionary<ObjectItemType, GuiItem> ObjectItems { get; protected set; }
        protected Sprite selection;
        protected Texture selectionTexture;
        protected Player owner;
        protected float itemWidth;
        public ObjectsGui(string textureName,Vector2 position,Player owner, DrawLayer drawLayer = DrawLayer.GUI) : base(textureName, layer: drawLayer)
        {
            IsActive = true;
            this.owner = owner;
            sprite.pivot = Vector2.Zero;
            Position = position;
            sprite.Camera = CameraMgr.GetCamera("GUI");
            selectionTexture = GfxMngr.GetTexture("selection");
            selection = new Sprite(Game.PixelsToUnits(selectionTexture.Width), Game.PixelsToUnits(selectionTexture.Height));
            itemWidth = selection.Width;
            DrawMngr.AddItem(this);
            ObjectItems = new Dictionary<ObjectItemType, GuiItem>();
        }
        public override void Draw()
        {
            base.Draw();
        }
        public virtual void AddGuiItem(ObjectItemType type)
        {
            switch (type)
            {
                case ObjectItemType.Key:
                    ObjectItems.Add(type, new GuiItem("key", Position + new Vector2(itemWidth * (ObjectItems.Count + 0.5f), HalfHeight)));
                    break;
                case ObjectItemType.Pickaxe:
                    ObjectItems.Add(type, new GuiItem("pickaxe", Position + new Vector2(itemWidth * (ObjectItems.Count + 0.5f), HalfHeight)));
                    break;
                case ObjectItemType.Lamp:
                    ObjectItems.Add(type, new GuiItem("lamp", Position + new Vector2(itemWidth * (ObjectItems.Count + 0.5f), HalfHeight)));
                    break;
                default:
                    break;
            }
        }
        public override void Load()
        {
            DrawMngr.AddItem(this);
            foreach (var item in ObjectItems)
            {
                DrawMngr.AddItem(item.Value);
            }
        }
        public override void Destroy()
        {
            foreach (var item in ObjectItems)
            {
                item.Value.Destroy();
            }
            base.Destroy();
        }
    }
}
