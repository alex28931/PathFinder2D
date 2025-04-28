using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGame
{
    struct CameraLimits
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
        public CameraLimits(float minX, float maxX, float minY, float maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
    static class CameraMgr
    {
        private static Dictionary<string, Tuple<Camera, float>> cameras;
        private static CameraBehaviour[] behaviours;
        private static CameraBehaviour currentBehaviour;
        public static Camera MainCamera { get; private set; }
        private static Vector2 targetOffset;

        //public static GameObject Target;
        public static float CameraSpeed = 5f;
        public static CameraLimits CameraLimits;



        public static void Init(GameObject target,CameraLimits cameraLimits)
        {
            MainCamera = new Camera(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            MainCamera.pivot = new Vector2(Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f);
            targetOffset = new Vector2(0, 0);
            CameraLimits = cameraLimits;

            cameras = new Dictionary<string, Tuple<Camera, float>>();

            behaviours = new CameraBehaviour[(int)CameraBehaviourType.LAST];
            behaviours[(int)CameraBehaviourType.FollowTarget] = new FollowTargetBehaviour(MainCamera, target);
            behaviours[(int)CameraBehaviourType.FollowPoint] = new FollowPointBehaviour(MainCamera, Vector2.Zero);
            behaviours[(int)CameraBehaviourType.MoveToPoint] = new MoveToPointBehaviour(MainCamera);

            currentBehaviour = behaviours[(int)CameraBehaviourType.FollowTarget];
        }
        public static void SetTarget(GameObject target,bool changeBehaviour = true)
        {
            FollowTargetBehaviour followTargetBehaviour = (FollowTargetBehaviour)behaviours[(int)CameraBehaviourType.FollowTarget];
            followTargetBehaviour.Target = target;
            if (changeBehaviour)
            {
                currentBehaviour = followTargetBehaviour;
            }
        }
        public static void SetPointToFollow(Vector2 point, bool changeBehaviour = true)
        {
            FollowPointBehaviour followPointBehaviour = (FollowPointBehaviour)behaviours[(int)CameraBehaviourType.FollowTarget];
            followPointBehaviour.SetPointToFollow(point);
            if (changeBehaviour)
            {
                currentBehaviour = followPointBehaviour;
            }
        }
        public static void MoveTo(Vector2 point,float duration)
        {
            currentBehaviour = behaviours[(int)CameraBehaviourType.MoveToPoint];
            ((MoveToPointBehaviour)currentBehaviour).MoveTo(point, duration);
        }
        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }
            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }
        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return MainCamera;
        }

        public static void Update()
        {
            Vector2 oldCameraPos = MainCamera.position;
            currentBehaviour.Update();
            FixPosition();
            Vector2 cameraDelta = MainCamera.position - oldCameraPos;
            if (cameraDelta != Vector2.Zero)
            {
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;
                }
            }
        }
        public static void FixPosition()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }
        public static void OnMovementEnd()
        {
            currentBehaviour = behaviours[(int)CameraBehaviourType.FollowTarget];
        }
    }
}
