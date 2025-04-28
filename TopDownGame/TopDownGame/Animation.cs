using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TopDownGame
{
    class Animation
    {
        protected int numFrames;
        protected float frameDuration;
        protected bool isPlaying;
        protected int currentFrame;
        protected float elapsedTime;

        protected int frameWidth;
        protected int frameHeight;

        public Vector2 Offset { get; protected set; }
        public bool Loop;
        public Animation(int frameW, int FrameH, float fps, int numFrames, bool loop = true, bool isPlaying = true)
        {
            frameWidth = frameW;
            frameHeight = FrameH;

            frameDuration = 1f / fps;

            this.numFrames = numFrames;
            this.isPlaying = isPlaying;
            Loop = loop;
        }
        public virtual void Play()
        {
            isPlaying = true;
        }
        public virtual void Restart()
        {
            currentFrame = 0;
            elapsedTime = 0.0f;
            Offset = Offset = new Vector2(currentFrame * frameWidth, Offset.Y);
        }
        public virtual void Stop()
        {
            currentFrame = 0;
            elapsedTime = 0.0f;
            isPlaying = false;
        }
        public virtual void Pause()
        {
            isPlaying = false;
        }
        public virtual void Update()
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
                        Pause();
                        return;
                    }
                }
                Offset = Offset = new Vector2(currentFrame * frameWidth, Offset.Y);
            }
        }
    }
}
