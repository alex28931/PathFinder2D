using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace TopDownGame
{
    class WobbleFX : PostProcessingEffect
    {
        private float time;
        private float speed;
        static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;
uniform float time;

void main()
{
    vec2 uv_current=uv;
    float A=1.f/100.f;
    float B=30.f;
    float C=time/100.f;
    uv_current.x+=A*sin(B*(uv_current.y+C));
    vec4 tex_col=texture(tex, uv_current);
    out_color=tex_col;
}

";
        public WobbleFX() : base(fragmentShader)
        {
            speed = 5.0f;
        }
        public override void Update(Window window)
        {
            time += window.DeltaTime * speed;
            screenMesh.shader.SetUniform("time", time);
        }
    }
}
