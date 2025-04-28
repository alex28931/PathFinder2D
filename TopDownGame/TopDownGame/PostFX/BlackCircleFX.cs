using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class BlackCircleFX : PostProcessingEffect
    {
        static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main()
{
    vec4 color = vec4(0.f, 0.f, 0.f, 1.f);
    vec4 tex_col=texture(tex, uv);
    if((uv.x-0.5f)*(uv.x-0.5f)*2.5f+(uv.y-0.5f)*(uv.y-0.5f)<=0.01f)
    {
        color = tex_col;
    }

    out_color = color;
}

";
        public BlackCircleFX() : base(fragmentShader)
        {

        }
    }
}
