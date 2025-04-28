using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    class Playscene : Scene
    {
        private string tmxFilePath;
        protected List<GameObject> objects;
        public TmxMap Map { get; protected set; }
        public int Id { get; private set; }
        protected AudioClip music;
        public Playscene(string tmxFilePath, int id) : base()
        {
            this.tmxFilePath = tmxFilePath;
            objects = new List<GameObject>();
            Id = id;
        }

        public override void Start()
        {
            if (Id == 0)
            {
                CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.75f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
                music = GfxMngr.GetClip("MainTheme");
                Game.BgSource.Play(music, true);
            }
            else if (Id == 4)
            {
                if (!Game.Player.objectsGui.ObjectItems.ContainsKey(ObjectItemType.Lamp))
                {
                    CameraMgr.CameraLimits = new CameraLimits(0, Game.Window.OrthoWidth * 2, 0, Game.Window.OrthoHeight * 2);
                    DrawMngr.AddFX("BlackCircle", new BlackCircleFX());
                }
                else
                {
                    CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.75f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
                }
                music = GfxMngr.GetClip("CavernTheme");
                Game.BgSource.Play(music, true);
            }
            else
            {
                CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.5f);
                music = GfxMngr.GetClip("HomeTheme");
                Game.BgSource.Play(music, true);
            }
            if (!isJustLoaded)
            {
                Map = new TmxMap(tmxFilePath);
                LoadObjects();
                isJustLoaded = true;
            }
            else
            {
                DrawMngr.AddItem(Map);
                UpdateMngr.AddItem(Map);
                ReloadObjects();
            }
            CameraMgr.MainCamera.position = Game.Player.Position;
            CameraMgr.SetTarget(Game.Player);
            Game.Player.IsActive = true;
            base.Start();
        }
        public void LoadObjects()
        {
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

           
            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            XmlNodeList objecstNodes = mapNode.SelectNodes("objectgroup");
            for (int i = 0; i < objecstNodes.Count; i++)
            {
                string objectGroupName = objecstNodes[i].Attributes.GetNamedItem("name").Value;
                switch (objectGroupName)
                {
                    case "Door":
                        XmlNodeList doorNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < doorNodes.Count; j++)
                        {
                            int x = int.Parse(doorNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(doorNodes[j].Attributes.GetNamedItem("y").Value);
                            int nextX= int.Parse(doorNodes[j].Attributes.GetNamedItem("nextX").Value);
                            int nextY= int.Parse(doorNodes[j].Attributes.GetNamedItem("nextY").Value);
                            int nextSceneId= int.Parse(doorNodes[j].Attributes.GetNamedItem("nextSceneId").Value);
                            bool isAnimated= bool.Parse(doorNodes[j].Attributes.GetNamedItem("isAnimated").Value);
                            bool isGate = bool.Parse(doorNodes[j].Attributes.GetNamedItem("isGate").Value);
                            bool isBlocked= bool.Parse(doorNodes[j].Attributes.GetNamedItem("isBlocked").Value);
                            Door door = new Door(new Vector2(x + 0.5f, y + 0.5f), new Vector2(nextX + 0.5f, nextY + 0.5f), nextSceneId, isAnimated, isGate, isBlocked);
                            objects.Add(door);
                        }
                        break;
                    case "Pickaxe":
                        XmlNodeList pickaxeNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < pickaxeNodes.Count; j++)
                        {
                            int x = int.Parse(pickaxeNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(pickaxeNodes[j].Attributes.GetNamedItem("y").Value);
                            Pickaxe pickaxe = new Pickaxe(new Vector2(x + 0.5f, y + 0.5f));
                            objects.Add(pickaxe);
                        }
                        break;
                    case "Key":
                        XmlNodeList keyNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < keyNodes.Count; j++)
                        {
                            int x = int.Parse(keyNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(keyNodes[j].Attributes.GetNamedItem("y").Value);
                            Key key = new Key(new Vector2(x + 0.5f, y + 0.5f));
                            objects.Add(key);
                        }
                        break;
                    case "Lamp":
                        XmlNodeList lampNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < lampNodes.Count; j++)
                        {
                            int x = int.Parse(lampNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(lampNodes[j].Attributes.GetNamedItem("y").Value);
                            Lamp lamp = new Lamp(new Vector2(x + 0.5f, y + 0.5f));
                            objects.Add(lamp);
                        }
                        break;
                    case "Gem":
                        XmlNodeList gemNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < gemNodes.Count; j++)
                        {
                            int x = int.Parse(gemNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(gemNodes[j].Attributes.GetNamedItem("y").Value);
                            Gem gem = new Gem(new Vector2(x + 0.5f, y + 0.5f));
                            objects.Add(gem);
                        }
                        break;
                    case "Stone":
                        XmlNodeList stoneNodes = objecstNodes[i].SelectNodes("object");
                        for (int j = 0; j < stoneNodes.Count; j++)
                        {
                            int x = int.Parse(stoneNodes[j].Attributes.GetNamedItem("x").Value);
                            int y = int.Parse(stoneNodes[j].Attributes.GetNamedItem("y").Value);
                            Stone stone = new Stone(new Vector2(x + 0.5f, y + 0.5f));
                            objects.Add(stone);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public override void Input()
        {
            Game.Player.Input();
        }

        public override void Update()
        {
            PhysicsMngr.Update();
            UpdateMngr.Update();

            PhysicsMngr.CheckCollisions();
            CameraMgr.Update();
        }

        public override Scene OnExit()
        {
            Game.BgSource.Stop();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            DrawMngr.RemoveFX("BlackCircle");
            DrawMngr.RemoveFX("BlackBigCircle");
            Game.Player.Load();
    

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();
        }

        public virtual void ReloadObjects()
        {
            if (objects != null)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].Load();
                }
            }
        }
        public virtual void Reset()
        {
            isJustLoaded = false;
            objects.Clear();
        }
    }
}
