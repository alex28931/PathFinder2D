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
    class TmxLayerAnimated
    {
        private Texture tilesetTexture;
        private Sprite layerSprite;
        private Texture[] layerTextures;
        protected int numFrames;
        protected float frameDuration;
        protected bool isPlaying;
        protected int currentFrame;
        protected float elapsedTime;
        public bool Loop;
        public string[] IDs { get; }
        public DrawLayer Layer { get; protected set; }

        public TmxLayerAnimated(XmlNode layerNode, TmxTileset tileset, int cols, int rows, int tileW, int tileH, int numFrames, float fps, bool loop = true, bool isPlaying = true)
        {
            frameDuration = 1f / fps;

            this.numFrames = numFrames;
            this.isPlaying = isPlaying;
            Loop = loop;

            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

            string[] Ids = csvData.Split(',');
            IDs = Ids;

            tilesetTexture = GfxMngr.GetTexture(tileset.TextureName);


            // Create a single texture for the whole layer
            layerTextures = new Texture[numFrames];
            List<byte[]> layerBitmaps = new List<byte[]>();
            for (int i = 0; i < numFrames; i++)
            {
                layerTextures[i] = new Texture(800, 400);
                layerBitmaps.Add(new byte[800 * 400 * 4]);
            }
            byte[] tilesetBitmap = tilesetTexture.Bitmap;

            int bytesPerPixel = 4;
            int tilesetBitmapRowLength = 256 * bytesPerPixel;
            int layerBitmapRowLength = 800 * bytesPerPixel;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int tileId = int.Parse(IDs[r * cols + c]);
                    if (tileId != 0)
                    {
                        for (int i = 0; i < numFrames; i++)
                        {
                            // Get correct tilesetBitmap's section starting index
                            // Get tilesetBitmapIndex offset X in pixels and convert it in bytes
                            int tilesetXOff = tileset.GetAtIndex(tileId + i).X * bytesPerPixel;
                            // Get tilesetBitmapIndex offset Y in pixels and convert it in bytes
                            int tilesetYOff = tileset.GetAtIndex(tileId + i).Y * tilesetBitmapRowLength;
                            // Calculate tilesetBitmap starting index
                            int tilesetBitmapIndexInitial = tilesetYOff + tilesetXOff;

                            // Get correct mapBitmap's section starting index
                            int mapXOff = c * tileW * bytesPerPixel;
                            int mapYOff = r * tileH * layerBitmapRowLength;

                            int layerBitmapIndexInitial = mapXOff + mapYOff;

                            // This loop is to copy each single tile from tilesetBitmap to mapBitmap
                            // Loop through each row copying a tile length every time (32 pixels = 32 * 4 bytes)
                            for (int j = 0; j < tileH; j++)
                            {
                                // How tilesetBitmapIndexInitial increments
                                int tilesetBitmapIndexUpdate = j * tilesetBitmapRowLength;
                                // How mapBitmapIndexInitial increments
                                int mapBitmapIndexUpdate = j * layerBitmapRowLength;

                                // Copy tilesetBitmap's tile section to mapBitmap in correct position
                                Array.Copy(tilesetBitmap,                                           // source array
                                           tilesetBitmapIndexInitial + tilesetBitmapIndexUpdate,    // source index
                                           layerBitmaps[i],                                               // dest array
                                           layerBitmapIndexInitial + mapBitmapIndexUpdate,                // dest index
                                           tileW * bytesPerPixel);                               // length
                            }
                            layerTextures[i].Update(layerBitmaps[i]);
                        }
                    }
                }
            }


            layerSprite = new Sprite(Game.PixelsToUnits(1600), Game.PixelsToUnits(800));
            layerSprite.position = new Vector2(0, 0);
        }
        public void Update()
        {
            if (isPlaying)
            {
                elapsedTime += Game.DeltaTime;
                if (elapsedTime >= frameDuration)
                {
                    currentFrame++;
                    elapsedTime = 0.0f;
                }
                if (currentFrame == numFrames)
                {
                    if (Loop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        isPlaying = false;
                        return;
                    }
                }
            }
        }

        public void Draw()
        {
            layerSprite.DrawTexture(layerTextures[currentFrame]);
        }
    }
}
