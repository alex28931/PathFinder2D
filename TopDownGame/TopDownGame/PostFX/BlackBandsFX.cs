using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TopDownGame
{
    class BlackBandsFX : PostProcessingEffect
    {
        static string fragmentShader = @"
#version 330 core

in vec2 uv;
uniform sampler2D tex;
out vec4 out_color;

void main()
{
    vec4 color = vec4(1.f, 1.f, 1.f, 0.f);
    vec4 tex_col=texture(tex, uv);
    if(uv.x <= 0.5f)
    {
        color = vec4(0.f, 0.f, 0.f, 1.f);
    }

    out_color = mix(color, tex_col, 0.5f);
}

";
        public BlackBandsFX() : base(fragmentShader)
        {

        }
    }
}
