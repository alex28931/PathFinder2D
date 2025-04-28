using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class TmxLayer
    {
        private Texture tilesetTexture;
        private Sprite layerSprite;
        private Texture layerTexture;
        private TmxTileset tileset;
        private byte[] layerBitmap;
        private byte[] tilesetBitmap;
        private int cols, rows;
        private int tileW, tileH;
        public string[] IDs { get; }
        public DrawLayer Layer { get; protected set; }

        public TmxLayer(XmlNode layerNode, TmxTileset tileset, int cols, int rows, int tileW, int tileH)
        {
            this.tileset = tileset;
            this.cols = cols;
            this.rows = rows;
            this.tileW = tileW;
            this.tileH = tileH;

            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

            string[] Ids = csvData.Split(',');
            IDs = Ids;

            tilesetTexture = GfxMngr.GetTexture(tileset.TextureName);       


            // Create a single texture for the whole layer
            layerTexture = new Texture(800, 400);
            layerBitmap = new byte[800 * 400 * 4];
            tilesetBitmap = tilesetTexture.Bitmap;

            int bytesPerPixel = 4;
            int tilesetBitmapRowLength = 256 * bytesPerPixel;
            int layerBitmapRowLength = 800 * bytesPerPixel;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int tileId = int.Parse(IDs[r * cols + c]);
                    ChangeLayerBitmap(new Vector2(c, r), tileId);
                }
            }

            layerTexture.Update(layerBitmap);

            layerSprite = new Sprite(Game.PixelsToUnits(1600), Game.PixelsToUnits(800));
            layerSprite.position = new Vector2(0, 0);
        }

        public void Draw()
        {
            layerSprite.DrawTexture(layerTexture);
        }
        public void ChangeLayerBitmap(Vector2 position, int tileId)
        {
            int bytesPerPixel = 4;
            int tilesetBitmapRowLength = 256 * bytesPerPixel;
            int layerBitmapRowLength = 800 * bytesPerPixel;
            if (tileId != 0)
            {
                // Get correct tilesetBitmap's section starting index
                // Get tilesetBitmapIndex offset X in pixels and convert it in bytes
                int tilesetXOff = tileset.GetAtIndex(tileId).X * bytesPerPixel;
                // Get tilesetBitmapIndex offset Y in pixels and convert it in bytes
                int tilesetYOff = tileset.GetAtIndex(tileId).Y * tilesetBitmapRowLength;
                // Calculate tilesetBitmap starting index
                int tilesetBitmapIndexInitial = tilesetYOff + tilesetXOff;

                // Get correct mapBitmap's section starting index
                int mapXOff = (int)position.X * tileW * bytesPerPixel;
                int mapYOff = (int)position.Y * tileH * layerBitmapRowLength;

                int layerBitmapIndexInitial = mapXOff + mapYOff;

                // This loop is to copy each single tile from tilesetBitmap to mapBitmap
                // Loop through each row copying a tile length every time (32 pixels = 32 * 4 bytes)
                for (int i = 0; i < tileH; i++)
                {
                    // How tilesetBitmapIndexInitial increments
                    int tilesetBitmapIndexUpdate = i * tilesetBitmapRowLength;
                    // How mapBitmapIndexInitial increments
                    int mapBitmapIndexUpdate = i * layerBitmapRowLength;

                    // Copy tilesetBitmap's tile section to mapBitmap in correct position
                    Array.Copy(tilesetBitmap,                                           // source array
                               tilesetBitmapIndexInitial + tilesetBitmapIndexUpdate,    // source index
                               layerBitmap,                                               // dest array
                               layerBitmapIndexInitial + mapBitmapIndexUpdate,                // dest index
                               tileW * bytesPerPixel);                               // length
                }
            }
        }
        public void ChangeLayerTexture(Vector2 position, int tileId)
        {
            ChangeLayerBitmap(position, tileId);
            layerTexture.Update(layerBitmap);
        }
    }
}
