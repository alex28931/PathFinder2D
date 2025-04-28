using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TopDownGame
{
    abstract class Scene
    {
        public bool IsPlaying;
        public Scene NextScene;
        protected bool isJustLoaded;

        public Scene()
        {
            isJustLoaded = false;
        }

        public virtual void Start()
        {
            IsPlaying = true;
        }

        public virtual Scene OnExit()
        {
            IsPlaying = false;
            return NextScene;
        }

        public abstract void Input();
        public virtual void Update()
        {

        }
        public abstract void Draw();
    }
}
