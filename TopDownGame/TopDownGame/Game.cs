using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using Aiv.Audio;

namespace TopDownGame
{
    static class Game
    {
        // Variables
        public static Window Window;
        public static Player Player;
        public static List<Playscene> playScenes;
        public static AudioSource BgSource;

        // Properties
        public static float UnitSize { get; private set; }
        public static Vector2 ScreenCenter { get; private set; }
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static float ScreenCenterX { get { return Window.Width * 0.5f; } }
        public static float ScreenCenterY { get { return Window.Height * 0.5f; } }
        public static Vector2 MouseRelativePosition { get { return CameraMgr.MainCamera.position - new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f) + Window.MousePosition; } }

        public static void Init()
        {

            Window = new Window(1280, 800, "TopDawnGame", false);
            Window.SetDefaultViewportOrthographicSize(25);

            UnitSize = Window.Height / Window.OrthoHeight;

            ScreenCenter = new Vector2(Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f);
            LoadAssets();
            LoadClips();

            BgSource = new AudioSource();

            CameraLimits cameraLimits = new CameraLimits(Window.OrthoWidth * 0.5f, Window.OrthoWidth * 0.5f, Window.OrthoHeight * 0.5f, Window.OrthoHeight * 0.5f);
            CameraMgr.Init(null, cameraLimits);
            CameraMgr.AddCamera("GUI", new Camera());

            Player = new Player();
            Player.Position = new Vector2(23.5f, 18.5f);
            // SCENES
            playScenes = new List<Playscene>();
            playScenes.Add(new Playscene("Assets/Map.tmx", 0));
            playScenes.Add(new Playscene("Assets/Home1.tmx", 1));
            playScenes.Add(new Playscene("Assets/Home2.tmx", 2));
            playScenes.Add(new Playscene("Assets/Home3.tmx", 3));
            playScenes.Add(new Playscene("Assets/Cavern.tmx", 4));
            Scene title = new TitleScene();
            CurrentScene = title;
            title.NextScene = playScenes[0];
        }

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / UnitSize;
        }
        public static void LoadAssets()
        {
            GfxMngr.AddTexture("tileset", "Assets/Tileset.png");
            GfxMngr.AddTexture("playerD", "Assets/HEROS8bit_Adventurer Walk D.png");
            GfxMngr.AddTexture("playerU", "Assets/HEROS8bit_Adventurer Walk U.png");
            GfxMngr.AddTexture("playerR", "Assets/HEROS8bit_Adventurer Walk R.png");
            GfxMngr.AddTexture("pickaxe", "Assets/item8BIT_pickaxe.png");
            GfxMngr.AddTexture("key", "Assets/item8BIT_key.png");
            GfxMngr.AddTexture("lamp", "Assets/item8BIT_lamp.png");
            GfxMngr.AddTexture("gem", "Assets/item8BIT_gem.png");
            GfxMngr.AddTexture("stone", "Assets/stone.png");
            GfxMngr.AddTexture("title", "Assets/aivBG.png");
            GfxMngr.AddTexture("gameOver", "Assets/gameOverBg.png");
            GfxMngr.AddTexture("objectsGUI", "Assets/objects_GUI_frame.png");
            GfxMngr.AddTexture("selection", "Assets/weapon_GUI_selection.png");
        }
        public static void LoadClips()
        {
            GfxMngr.AddClip("MainTheme", "Assets/Sounds/1BITTopDownMusics - Track 01 (1BIT Adventure).wav");
            GfxMngr.AddClip("CavernTheme", "Assets/Sounds/1BITTopDownMusics - Track 02 (1BIT Dark Cave).wav");
            GfxMngr.AddClip("HomeTheme", "Assets/Sounds/1BITTopDownMusics - Track 03 (1BIT Eerie).wav");
            GfxMngr.AddClip("Pickup", "Assets/Sounds/Pickup01.wav");
            GfxMngr.AddClip("Hurt", "Assets/Sounds/Hurt01.wav");
            GfxMngr.AddClip("Land", "Assets/Sounds/Land01.wav");
        }
        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                Window.SetTitle($"FPS: {1f / Window.DeltaTime}");
                // Exit when ESC is pressed
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }
                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();



                Window.Update();
            }
        }
    }
}
