using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;

namespace TopDownGame
{
    class TmxMap : I_Drawable, I_Updatable
    {
        private string tmxFilePath;
        public DrawLayer Layer { get; }
        //Map Nodes
        public MapNodes MapNodes { get; private set; }
        // Tileset
        TmxTileset tileset;
        // Tilelayer
        public TmxLayer[] Layers { get; protected set; }
        //TilelayerAnimated
        TmxLayerAnimated[] layersAnimated;

        public TmxMap(string filePath)
        {
            // Map Drawing Settings
            Layer = DrawLayer.Background;
            DrawMngr.AddItem(this);
            UpdateMngr.AddItem(this);

            // CREATE AND LOAD XML DOCUMENT FROM TMX MAP FILE
            tmxFilePath = filePath;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(tmxFilePath);
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            // PROCEED TO XML DOCUMENT NODES PARSING
            // Map Node and Attributes
            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            int mapCols = GetIntAttribute(mapNode, "width");
            int mapRows = GetIntAttribute(mapNode, "height");
            int mapTileW = GetIntAttribute(mapNode, "tilewidth");
            int mapTileH = GetIntAttribute(mapNode, "tileheight");

            // Tileset Node and Attributes
            XmlNode tilesetNode = mapNode.SelectSingleNode("tileset");
            int tilesetTileW = GetIntAttribute(tilesetNode, "tilewidth");
            int tilesetTileH = GetIntAttribute(tilesetNode, "tileheight");
            int tileCount = GetIntAttribute(tilesetNode, "tilecount");
            int tilesetCols = GetIntAttribute(tilesetNode, "columns");
            int tilesetRows = tileCount / tilesetCols;
            // Create Tileset from collected data
            tileset = new TmxTileset("tileset", tilesetCols, tilesetRows, tilesetTileW, tilesetTileH);
            XmlNodeList layerAnimatedNodes = mapNode.SelectNodes("layerAnimated");
            if (layerAnimatedNodes != null)
            {
                layersAnimated = new TmxLayerAnimated[layerAnimatedNodes.Count];
                for (int i = 0; i < layerAnimatedNodes.Count; i++)
                {
                    int numFrames = GetIntAttribute(layerAnimatedNodes[i], "numFrames");
                    layersAnimated[i] = new TmxLayerAnimated(layerAnimatedNodes[i], tileset, mapCols, mapRows, mapTileW, mapTileH, numFrames, numFrames);
                }
            }

            XmlNodeList layerNodes = mapNode.SelectNodes("layer");
            Layers = new TmxLayer[layerNodes.Count];
            for (int i = 0; i < layerNodes.Count; i++)
            {
                Layers[i] = new TmxLayer(layerNodes[i], tileset, mapCols, mapRows, mapTileW, mapTileH);
            }
            XmlNode layerNodesNode = mapNode.SelectSingleNode("layerNodes");
            MapNodes = new MapNodes(layerNodesNode, mapCols, mapRows);
        }

        public static int GetIntAttribute(XmlNode node, string attrName)
        {
            return int.Parse(GetStringAttribute(node, attrName));
        }

        public static bool GetBoolAttribute(XmlNode node, string attrName)
        {
            return bool.Parse(GetStringAttribute(node, attrName));
        }

        public static string GetStringAttribute(XmlNode node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName).Value;
        }

        public void Draw()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                if (Layers[i] != null)
                {
                    Layers[i].Draw();
                }
            }
            for (int i = 0; i < layersAnimated.Length; i++)
            {
                if (layersAnimated[i] != null)
                {
                    layersAnimated[i].Draw();
                }
            }
        }
        public void Update()
        {
            for (int i = 0; i < layersAnimated.Length; i++)
            {
                if (layersAnimated[i] != null)
                {
                    layersAnimated[i].Update();
                }
            }
        }
    }
}
